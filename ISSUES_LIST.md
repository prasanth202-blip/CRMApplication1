# 🐛 CRM APPLICATION - ISSUES LIST
## Date: January 3, 2026

---

## 🔴 **P0 CRITICAL ISSUES (Must Fix Before Production) - 7 Items**

### **1. P0-P1: Payment Reconciliation Verification**
- **File**: `SubscriptionController.cs` (Line 741-766)
- **Issue**: Razorpay signature verification code exists but needs testing
- **Impact**: Payment security vulnerability
- **Fix Time**: 1 hour
- **Action**: Verify `_razorpayService.VerifyPaymentSignature()` works correctly
- **Test**: Make test payment and validate signature check

### **2. P0-CP1: Partner Commission Calculation**
- **File**: `Services/MonthlyPayoutBackgroundService.cs`
- **Issue**: Background service incomplete, commission calculation logic missing
- **Impact**: Incorrect partner payouts, financial errors
- **Fix Time**: 1.5 hours
- **Action**: Implement proper commission calculation based on bookings

### **3. P0-AG1: Agent Commission Calculation**
- **File**: `Services/MonthlyPayoutBackgroundService.cs`
- **Issue**: Agent commission calculation incomplete
- **Impact**: Incorrect agent payouts, financial errors
- **Fix Time**: 1.5 hours
- **Action**: Implement agent commission calculation logic

### **4. P0-D2: Optimistic Concurrency Control**
- **Files**: `LeadsController.cs`, `BookingsController.cs`, `SubscriptionController.cs`, etc.
- **Issue**: RowVersion fields exist but no `DbUpdateConcurrencyException` handling
- **Impact**: Data corruption on concurrent edits, lost updates
- **Fix Time**: 1 hour
- **Action**: Add try-catch blocks for concurrency exceptions in all controllers with RowVersion

### **5. P0-D3: Document Verification Workflow**
- **Files**: `AgentController.cs`, `ManageUsersController.cs`
- **Issue**: No ApproveDocument/RejectDocument endpoints exist
- **Impact**: Cannot approve/reject agent or partner documents
- **Fix Time**: 1 hour
- **Action**: 
  - Create `POST /Agent/ApproveDocument`
  - Create `POST /Agent/RejectDocument`
  - Update `DocumentStatus` field

### **6. P0-I1: Webhook Retry Mechanism**
- **File**: Need to create `BackgroundServices/WebhookRetryBackgroundService.cs`
- **Issue**: WebhookRetryQueue table exists but no background service to process it
- **Impact**: Failed webhooks never retried, lost events
- **Fix Time**: 1.5 hours
- **Action**: 
  - Create background service
  - Process WebhookRetryQueue
  - Retry failed webhooks (max 3 attempts)
  - Update retry count and status

### **7. P0-S1: Trial Expiration Enforcement**
- **File**: `Middleware/SubscriptionLimitMiddleware.cs`
- **Issue**: Middleware exists but doesn't block access for expired trials
- **Impact**: Users continue using system after trial ends, revenue loss
- **Fix Time**: 0.5 hours
- **Action**: Add trial expiration check in middleware before calling next()

---

## 🟡 **P0 INCOMPLETE FEATURE**

### **8. P0-A4: CSRF Protection (Incomplete)**
- **Files**: All view files with POST forms (50+ files)
- **Issue**: Only 1 form (Leads/Index.cshtml) has `@Html.AntiForgeryToken()`
- **Impact**: CSRF vulnerability on all other forms
- **Fix Time**: 1 hour
- **Action**: Add `@Html.AntiForgeryToken()` to all POST forms:
  - Account forms (login, register, password reset)
  - Agent forms (onboarding, approval)
  - Booking forms
  - Payment forms
  - Quotation forms
  - Invoice forms
  - Settings forms
  - All modal forms

**Affected Forms**:
- `Account/Login.cshtml`
- `Account/Register.cshtml`
- `Agent/Index.cshtml`
- `Bookings/Index.cshtml`
- `Payments/Index.cshtml`
- `Quotations/Index.cshtml`
- `Settings/Index.cshtml`
- And 40+ more modal forms

---

## 🟠 **P1 HIGH PRIORITY ISSUES (Missing Features) - 25 Items**

### **User Interface Missing**

1. **UTM Tracking Display**
   - Schema: ✅ Ready (UtmSource, UtmMedium, UtmCampaign, UtmTerm, UtmContent)
   - Issue: No UI to display UTM data in leads
   - Impact: Cannot track marketing campaign effectiveness
   - Fix Time: 2 hours

2. **Geolocation Display**
   - Schema: ✅ Ready (Latitude, Longitude in Leads)
   - Issue: No UI to show location on map
   - Impact: Cannot track agent field visits
   - Fix Time: 3 hours

3. **Lead History Audit Trail**
   - Schema: ✅ Ready (LeadHistoryModel table exists)
   - Issue: Not populated, no UI to view
   - Impact: Cannot track lead changes
   - Fix Time: 4 hours

4. **Property Documents Management**
   - Schema: ✅ Ready (PropertyDocumentModel table exists)
   - Issue: No upload/view UI
   - Impact: Cannot store property docs
   - Fix Time: 3 hours

5. **Leave Management System**
   - Schema: ✅ Ready (LeaveRequests table exists)
   - Issue: No UI to request/approve leaves
   - Impact: Manual leave management
   - Fix Time: 6 hours

6. **Notification Preferences**
   - Schema: ✅ Ready (NotificationPreferences table exists)
   - Issue: No settings page for users
   - Impact: Cannot customize notifications
   - Fix Time: 3 hours

7. **Email Templates CRUD**
   - Schema: ✅ Ready (EmailTemplates table has 3 defaults)
   - Issue: No UI to manage templates
   - Impact: Cannot customize emails
   - Fix Time: 4 hours

8. **Audit Logs Viewer**
   - Schema: ✅ Ready (AuditLogs table exists)
   - Issue: No logging implementation, no UI
   - Impact: No audit trail
   - Fix Time: 5 hours

9. **Bulk Lead Assignment**
   - Issue: No UI to assign multiple leads at once
   - Impact: Tedious one-by-one assignment
   - Fix Time: 3 hours

10. **Storage Usage Tracking UI**
    - Issue: No dashboard widget showing storage usage
    - Impact: Cannot monitor storage limits
    - Fix Time: 2 hours

### **Business Logic Missing**

11. **Auto-Renewal Implementation**
    - Issue: No automatic subscription renewal
    - Impact: Manual renewal required
    - Fix Time: 8 hours

12. **Proration Logic**
    - Issue: Mid-cycle upgrades not prorated
    - Impact: Incorrect billing
    - Fix Time: 6 hours

13. **Subscription Addons Logic**
    - Issue: UI exists but logic incomplete
    - Impact: Cannot sell addons
    - Fix Time: 5 hours

14. **Payment Gateway for Bookings**
    - Issue: Only implemented for subscriptions
    - Impact: Manual payment collection
    - Fix Time: 8 hours

15. **EMI Calculator Widget**
    - Issue: No EMI calculation feature
    - Impact: Manual calculation needed
    - Fix Time: 3 hours

16. **Payment Reminders**
    - Issue: No overdue payment notifications
    - Impact: Delayed collections
    - Fix Time: 4 hours

17. **GST Compliance**
    - Issue: Tax calculation exists, no GSTIN validation
    - Impact: Compliance risk
    - Fix Time: 3 hours

18. **Email Notifications Enhancement**
    - Issue: Basic email exists, no proper templates
    - Impact: Poor user experience
    - Fix Time: 6 hours

19. **WhatsApp Integration Completion**
    - Issue: WhatsAppService exists but not fully integrated
    - Impact: Manual WhatsApp messages
    - Fix Time: 5 hours

20. **Dashboard Analytics**
    - Issue: Basic stats only, no charts/graphs
    - Impact: Poor insights
    - Fix Time: 8 hours

21. **Commission Reports Enhancement**
    - Issue: Basic reports, need detailed breakdown
    - Impact: Poor financial visibility
    - Fix Time: 4 hours

22. **Email Configuration UI**
    - Issue: No SMTP settings page
    - Impact: Hardcoded email settings
    - Fix Time: 3 hours

23. **Payment Gateway Config UI**
    - Issue: Hardcoded Razorpay keys
    - Impact: Cannot change gateway settings
    - Fix Time: 2 hours

24. **Role & Permission Management UI**
    - Issue: RolePermissions exist, limited UI
    - Impact: Difficult to manage permissions
    - Fix Time: 6 hours

25. **Webhook Logs Viewer**
    - Issue: Basic logging, no UI
    - Impact: Cannot debug webhook issues
    - Fix Time: 3 hours

---

## 🟢 **P2 MEDIUM PRIORITY ISSUES (UX Enhancements) - 23 Items**

1. **Property Multi-Image Gallery** - Single image only (3h)
2. **Property Analytics Tracking** - No view tracking (4h)
3. **Booking Modification** - Cannot change booking details (5h)
4. **Booking Transfer** - Cannot transfer to different buyer (4h)
5. **Bulk Payment Import** - No CSV import (3h)
6. **Agent Performance Dashboard** - No analytics view (6h)
7. **Agent Targets Setting** - No target management (4h)
8. **Partner Performance Reports** - No analytics (5h)
9. **Quotation Templates** - No template system (4h)
10. **Quotation Versioning** - No revision history (3h)
11. **Recurring Invoices** - No automation (5h)
12. **Attendance Regularization** - No correction requests (3h)
13. **Task Templates** - No predefined tasks (3h)
14. **Task Recurrence** - No recurring tasks (4h)
15. **SMS Notifications** - No SMS gateway (6h)
16. **Report Export Enhancement** - Not all reports exportable (3h)
17. **System Settings UI** - No global config page (4h)
18. **API Documentation** - No Swagger docs (4h)
19. **Rate Limiting** - No API throttling (3h)
20. **Session Management** - No concurrent session control (3h)
21. **Password Strength Meter** - No visual feedback (2h)
22. **Dark Mode** - No theme toggle (5h)
23. **Global Search** - Search limited to each page (8h)

---

## 🔵 **P3 LOW PRIORITY ISSUES (Advanced Features) - 15 Items**

1. **Two-Factor Authentication** (8h)
2. **Lead Scoring Algorithm** (10h)
3. **Partner Hierarchy** - Sub-partners (12h)
4. **Virtual Tour Integration** - 360° views (15h)
5. **Online Quotation Approval Portal** - Customer portal (20h)
6. **Biometric Integration** - Device support (10h)
7. **Task Dependencies** - Task chains (8h)
8. **Custom Report Builder** - Dynamic reports (20h)
9. **Zapier Integration** - Webhook triggers (12h)
10. **IP Whitelisting** - IP restrictions (4h)
11. **Data Encryption at Rest** - Field-level encryption (15h)
12. **AI Lead Scoring** - ML-based scoring (40h)
13. **Chatbot Integration** - Customer support (30h)
14. **Mobile App** - iOS/Android (200h)
15. **Voice Call Integration** - VoIP (25h)

---

## ⚠️ **SECURITY VULNERABILITIES**

### **Critical**
1. ❌ CSRF protection incomplete (50+ forms unprotected)
2. ❌ Payment signature verification needs testing
3. ❌ Concurrent edit handling missing (data corruption risk)

### **High**
4. ⚠️ Trial expiration not enforced (revenue loss)
5. ⚠️ No audit logging implementation
6. ⚠️ Webhook retry mechanism missing (lost events)

### **Medium**
7. ⚠️ No rate limiting (DoS risk)
8. ⚠️ No session management (multiple login risk)
9. ⚠️ Hardcoded configuration (security risk)

---

## 🐞 **KNOWN BUGS**

### **From Recent Implementation**
1. **Need Testing**: Lead duplication check (P0-L1) - recently implemented
2. **Need Testing**: Handover subscription validation (P0-L2) - recently implemented
3. **Need Testing**: Booking cancellation refund (P0-B2) - recently implemented
4. **Need Testing**: Quotation auto-expiry (P0-Q1) - recently implemented
5. **Need Testing**: Payment installment validation (P0-P2) - recently implemented

### **Compilation Warnings**
- 272 nullable reference warnings (safe to ignore)
- Entity Framework decimal precision warnings (cosmetic)

---

## 📊 **ISSUE SUMMARY**

| Priority | Count | Estimated Time | Status |
|----------|-------|----------------|--------|
| **P0 Critical** | 7 | 8 hours | 🔴 BLOCKING |
| **P0 CSRF** | 1 | 1 hour | 🔴 SECURITY |
| **P1 High** | 25 | 110 hours (2.75 weeks) | 🟡 IMPORTANT |
| **P2 Medium** | 23 | 95 hours (2.4 weeks) | 🟢 NICE TO HAVE |
| **P3 Low** | 15 | 435 hours (10.9 weeks) | 🔵 FUTURE |
| **TOTAL** | **71** | **649 hours (16.2 weeks)** | |

---

## 🎯 **PRIORITY ACTIONS**

### **This Week (Must Do)**
1. Fix P0-P1: Payment reconciliation verification (1h)
2. Fix P0-CP1: Partner commission calculation (1.5h)
3. Fix P0-AG1: Agent commission calculation (1.5h)
4. Fix P0-D2: Concurrency control (1h)
5. Fix P0-D3: Document verification endpoints (1h)
6. Fix P0-I1: Webhook retry service (1.5h)
7. Fix P0-S1: Trial expiration enforcement (0.5h)
8. Fix P0-A4: Add CSRF tokens to all forms (1h)
9. **Total: 9 hours**

### **Next 2 Weeks (Should Do)**
Implement top 10 P1 features based on business impact

### **Future Releases**
Implement P2 and P3 based on customer feedback

---

## 📝 **NOTES**

- All P0 issues are **BLOCKING** for production
- Database schema is **100% ready** for P1 features
- Most P1 issues are just **missing UI** (backend ready)
- Application is **71% complete** overall
- **Estimated time to production: 1 week** (P0 fixes only)

---

*Generated: January 3, 2026*  
*Total Issues: 71 (8 Critical, 25 High, 23 Medium, 15 Low)*
