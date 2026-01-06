# Comprehensive Anti-Forgery Token Audit

## Controllers Missing [ValidateAntiForgeryToken]

Based on scan of all [HttpPost] methods:

### ❌ AttendanceController.cs
- Line 462: `RequestCorrection` - NEEDS TOKEN

### ❌ BookingsController.cs  
- Line 1030: `CancelBooking` - NEEDS TOKEN

### ❌ HomeController.cs
- Line 360: `MarkNotificationRead` - NEEDS TOKEN
- Line 376: `MarkAllNotificationsRead` - NEEDS TOKEN

### ❌ LeadsController.cs
- Line 750: `SaveNote` - NEEDS TOKEN
- Line 1153: `UploadLeadFile` - NEEDS TOKEN (View already fixed)
- Line 1530: `Delete` - NEEDS TOKEN

### ❌ ManageUsersController.cs
- Line 489: `CreatePartner` - NEEDS TOKEN
- Line 706: `ApprovePartner` - NEEDS TOKEN
- Line 777: `RejectPartner` - NEEDS TOKEN
- Line 952: `UploadPartnerDocument` - NEEDS TOKEN
- Line 999: `ApprovePartnerDocument` - NEEDS TOKEN
- Line 1032: `RejectPartnerDocument` - NEEDS TOKEN

### ❌ PropertiesController.cs
- Line 114: `SaveProperty` - NEEDS TOKEN
- Line 241: `Delete` - NEEDS TOKEN

### ❌ SettingsController.cs
- Line 46: `UpdateSettings` - NEEDS TOKEN
- Line 305: `UpdateBranding` - NEEDS TOKEN

### ❌ SubscriptionController.cs
- Line 91: `CreatePlan` - NEEDS TOKEN
- Line 123: `UpdatePlan` - NEEDS TOKEN
- Line 190: `TogglePlan` - NEEDS TOKEN
- Line 301: `AdminChangePlan` - NEEDS TOKEN
- Line 1214: `MarkRefundProcessed` - NEEDS TOKEN
- Line 1316: `AdminUpgradePlan` - NEEDS TOKEN
- Line 1513: `RazorpayWebhook` - NEEDS TOKEN (webhook - may not need token)

---

## Views Missing Anti-Forgery Tokens

### ❌ Views/Properties/Details.cshtml
- Line 709: `deleteFlat` function - NEEDS TOKEN
- Line 875: `deleteProperty` function - NEEDS TOKEN

### Need to check:
- Views/Bookings/*.cshtml - CancelBooking calls
- Views/Home/*.cshtml - Notification marking calls
- Views/Leads/*.cshtml - SaveNote, Delete calls
- Views/ManageUsers/*.cshtml - Partner approval/document functions
- Views/Subscription/*.cshtml - Plan management calls
- Views/Settings/*.cshtml - Settings update calls
- Views/Attendance/*.cshtml - RequestCorrection calls

---

## Priority Actions

### HIGH PRIORITY (User-facing CRUD operations):
1. ✅ Properties/Details.cshtml - deleteFlat, deleteProperty
2. Leads - SaveNote, Delete  
3. Bookings - CancelBooking
4. ManageUsers - Partner approvals, document uploads
5. Attendance - RequestCorrection

### MEDIUM PRIORITY (Admin operations):
6. Subscription - Plan management
7. Settings - Update settings/branding

### LOW PRIORITY (May be webhooks/API):
8. HomeController notifications
9. RazorpayWebhook (external webhook - may need different handling)
