-- SQL Diagnostic Queries for Partner Approval Issue
-- Run these queries to identify the data inconsistency

-- 1. Find Channel Partners without corresponding Users
SELECT 
    cp.PartnerId,
    cp.CompanyName,
    cp.ContactPerson,
    cp.Email,
    cp.Status,
    cp.UserId,
    cp.CreatedOn,
    cp.ApprovedOn
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.Email = u.Email
WHERE cp.Status = 'Approved' 
  AND u.UserId IS NULL;

-- 2. Find Channel Partners with UserId but no actual User record
SELECT 
    cp.PartnerId,
    cp.CompanyName,
    cp.Email,
    cp.UserId,
    cp.Status
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.UserId = u.UserId
WHERE cp.UserId IS NOT NULL 
  AND u.UserId IS NULL;

-- 3. Find Users with Partner role but no ChannelPartner record
SELECT 
    u.UserId,
    u.Username,
    u.Email,
    u.Role,
    u.ChannelPartnerId,
    u.CreatedDate
FROM Users u
LEFT JOIN ChannelPartners cp ON u.ChannelPartnerId = cp.PartnerId
WHERE u.Role = 'Partner' 
  AND cp.PartnerId IS NULL;

-- 4. Find duplicate emails between ChannelPartners and Users
SELECT 
    cp.Email,
    COUNT(cp.PartnerId) as PartnerCount,
    COUNT(u.UserId) as UserCount
FROM ChannelPartners cp
FULL OUTER JOIN Users u ON cp.Email = u.Email
GROUP BY cp.Email
HAVING COUNT(cp.PartnerId) > 1 OR COUNT(u.UserId) > 1;

-- 5. Check for orphaned records (Partners approved but no user created)
SELECT 
    cp.PartnerId,
    cp.CompanyName,
    cp.Email,
    cp.Status,
    cp.ApprovedOn,
    CASE 
        WHEN u.UserId IS NULL THEN 'MISSING USER'
        ELSE 'USER EXISTS'
    END as UserStatus
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.Email = u.Email AND u.Role = 'Partner'
WHERE cp.Status = 'Approved'
ORDER BY cp.ApprovedOn DESC;

-- 6. Check for inconsistent ChannelPartnerId linking
SELECT 
    u.UserId,
    u.Email,
    u.ChannelPartnerId,
    cp.PartnerId,
    cp.Email as PartnerEmail,
    CASE 
        WHEN u.ChannelPartnerId != cp.PartnerId THEN 'MISMATCH'
        WHEN u.ChannelPartnerId IS NULL THEN 'NULL LINK'
        ELSE 'CORRECT'
    END as LinkStatus
FROM Users u
LEFT JOIN ChannelPartners cp ON u.Email = cp.Email
WHERE u.Role = 'Partner';

-- 7. Find recent partner approvals to check pattern
SELECT 
    cp.PartnerId,
    cp.CompanyName,
    cp.Email,
    cp.Status,
    cp.ApprovedOn,
    u.UserId,
    u.CreatedDate,
    DATEDIFF(minute, cp.ApprovedOn, u.CreatedDate) as MinutesDifference
FROM ChannelPartners cp
LEFT JOIN Users u ON cp.Email = u.Email
WHERE cp.ApprovedOn >= DATEADD(day, -30, GETDATE())
ORDER BY cp.ApprovedOn DESC;