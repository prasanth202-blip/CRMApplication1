-- SQL Insert Queries to Fix Missing User Records
-- Use these queries to create missing user accounts for approved partners

-- 1. Insert missing users for approved partners (Manual approach)
-- First, check which partners need users created:
SELECT 
    cp.PartnerId,
    cp.CompanyName,
    cp.ContactPerson,
    cp.Email,
    cp.Phone,
    -- Generate password: first 4 chars of email + "@" + last 4 digits of phone
    UPPER(LEFT(cp.Email, 4)) + '@' + RIGHT(REPLACE(cp.Phone, ' ', ''), 4) as GeneratedPassword
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.Email = u.Email
WHERE cp.Status = 'Approved' 
  AND u.UserId IS NULL;

-- 2. Insert users for approved partners without user accounts
INSERT INTO Users (
    Username, 
    Email, 
    Password, 
    Role, 
    Phone, 
    IsActive, 
    CreatedDate, 
    LastActivity, 
    ChannelPartnerId
)
SELECT 
    cp.ContactPerson as Username,
    cp.Email,
    UPPER(LEFT(cp.Email, 4)) + '@' + RIGHT(REPLACE(cp.Phone, ' ', ''), 4) as Password,
    'Partner' as Role,
    cp.Phone,
    1 as IsActive,
    GETDATE() as CreatedDate,
    GETDATE() as LastActivity,
    cp.PartnerId as ChannelPartnerId
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.Email = u.Email
WHERE cp.Status = 'Approved' 
  AND u.UserId IS NULL;

-- 3. Update ChannelPartners table with the newly created UserId
UPDATE cp 
SET UserId = u.UserId
FROM ChannelPartners cp
INNER JOIN Users u ON cp.Email = u.Email AND u.Role = 'Partner'
WHERE cp.UserId IS NULL 
  AND cp.Status = 'Approved';

-- 4. Fix ChannelPartnerId linking for existing Partner users
UPDATE u 
SET ChannelPartnerId = cp.PartnerId
FROM Users u
INNER JOIN ChannelPartners cp ON u.Email = cp.Email
WHERE u.Role = 'Partner' 
  AND u.ChannelPartnerId IS NULL;

-- 5. Create trial subscriptions for partners without subscriptions
INSERT INTO PartnerSubscriptions (
    ChannelPartnerId,
    PlanId,
    BillingCycle,
    Amount,
    StartDate,
    EndDate,
    Status,
    PaymentMethod,
    PaymentTransactionId,
    LastPaymentDate,
    NextPaymentDate,
    AutoRenew,
    CreatedOn,
    UpdatedOn
)
SELECT 
    cp.PartnerId,
    (SELECT TOP 1 PlanId FROM SubscriptionPlans WHERE IsActive = 1 ORDER BY MonthlyPrice ASC) as PlanId,
    'Trial' as BillingCycle,
    0 as Amount,
    GETDATE() as StartDate,
    DATEADD(day, 7, GETDATE()) as EndDate,
    'Active' as Status,
    'Trial' as PaymentMethod,
    'trial_' + CAST(GETDATE() as VARCHAR) as PaymentTransactionId,
    GETDATE() as LastPaymentDate,
    DATEADD(day, 7, GETDATE()) as NextPaymentDate,
    0 as AutoRenew,
    GETDATE() as CreatedOn,
    GETDATE() as UpdatedOn
FROM ChannelPartners cp
LEFT JOIN PartnerSubscriptions ps ON cp.PartnerId = ps.ChannelPartnerId
WHERE cp.Status = 'Approved' 
  AND ps.SubscriptionId IS NULL;

-- 6. Verification query - Run after fixes to confirm
SELECT 
    cp.PartnerId,
    cp.CompanyName,
    cp.Email,
    cp.Status,
    cp.UserId,
    u.UserId as ActualUserId,
    u.Username,
    u.Role,
    u.ChannelPartnerId,
    ps.SubscriptionId,
    ps.Status as SubscriptionStatus,
    CASE 
        WHEN u.UserId IS NULL THEN 'MISSING USER'
        WHEN cp.UserId != u.UserId THEN 'USERID MISMATCH'
        WHEN u.ChannelPartnerId != cp.PartnerId THEN 'PARTNER LINK MISMATCH'
        WHEN ps.SubscriptionId IS NULL THEN 'MISSING SUBSCRIPTION'
        ELSE 'ALL GOOD'
    END as Status_Check
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.Email = u.Email AND u.Role = 'Partner'
LEFT JOIN PartnerSubscriptions ps ON cp.PartnerId = ps.ChannelPartnerId
WHERE cp.Status = 'Approved'
ORDER BY cp.ApprovedOn DESC;