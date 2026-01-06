# Anti-Forgery Token Fix Guide - ⚠️ PARTIALLY COMPLETE

## Status Summary:

### ✅ COMPLETED:
- **Controllers**: 29/29 [HttpPost] methods have `[ValidateAntiForgeryToken]` ✓
- **Views (fetch API)**: 6/6 fetch POST calls have tokens ✓
- **Build Status**: ✓ Build succeeds with no errors

### ⚠️ REMAINING ISSUES:
- **Views (jQuery)**: 34 jQuery POST calls ($.ajax, $.post) missing tokens

---

## ✅ Controllers Fixed (ALL DONE):

### Automatically Fixed by Script:
1. **AttendanceController.cs** - RequestCorrection
2. **BookingsController.cs** - CancelBooking
3. **HomeController.cs** - MarkNotificationRead, MarkAllNotificationsRead (2 methods)
4. **LeadsController.cs** - SaveNote, UploadLeadFile, Delete (3 methods)
5. **ManageUsersController.cs** - CreatePartner, ApprovePartner, RejectPartner, UploadPartnerDocument, ApprovePartnerDocument, RejectPartnerDocument (6 methods)
6. **SettingsController.cs** - UpdateSettings, UpdateBranding (2 methods)
7. **SubscriptionController.cs** - CreatePlan, UpdatePlan, TogglePlan, AdminChangePlan, MarkRefundProcessed, AdminUpgradePlan, RazorpayWebhook (7 methods)

### Manually Fixed:
8. **PropertiesController.cs** - SaveProperty, Delete (2 methods)

**Total Controller Methods Fixed: 22**

---

## ✅ Views (fetch API) Fixed (ALL DONE):

1. Views/Leads/Index.cshtml - UploadLeadFile
2. Views/Properties/Details.cshtml - 7 functions (upload/delete images, documents, flats, properties)
3. Views/Properties/Index.cshtml - Bulk upload
4. Views/Properties/_PropertyModal.cshtml - SaveProperty
5. Views/Properties/_FlatModal.cshtml - SaveFlat
6. Views/Properties/_BulkUploadFlatsModal.cshtml - Bulk upload
7. Views/Shared/_UploadsPartial.cshtml - File upload
8. Views/ManageUsers/RolePermissions.cshtml - Save permissions
9. Views/Agent/Details.cshtml - Upload/delete documents (2 functions)
10. Views/WebhookLeads/Index.cshtml - Assign/delete leads (2 functions)
11. Views/SalesPipelines/Index.cshtml - Update lead stage

**Total fetch() calls fixed: 6 verified**

---

## ⚠️ Views (jQuery) - NEEDS FIXING:

### $.post() calls (26 missing tokens):
- **Views/Tasks/Calendar.cshtml** - 5 calls (lines 1148, 1182, 1194, 1219, 1231)
- **Views/ManageUsers/Index_Old.cshtml** - 2 calls (lines 339, 368)
- **Views/ManageUsers/Index.cshtml** - 3 calls (lines 640, 649, 656)
- **Views/ManageUsers/Roles_Old.cshtml** - 2 calls (lines 223, 252)
- **Views/Tasks/Test.cshtml** - 3 calls (lines 92, 109, 121)
- **Views/Settings/Impersonation.cshtml** - 1 call (line 235)
- **Views/Shared/_Layout.cshtml** - 6 calls (lines 2344, 2361, 2422, 2441, 2482, 2515)
- **Views/Shared/_WhatsAppModal.cshtml** - 4 calls (lines 226, 238, 250, 262)

### $.ajax() calls with type: 'POST' (8 missing tokens):
- **Views/Expenses/Create.cshtml** - 1 call (line 173)
- **Views/ManageUsers/AddUser.cshtml** - 1 call (line 12)
- **Views/ManageUsers/Create.cshtml** - 1 call (line 13)
- **Views/ManageUsers/EditUser.cshtml** - 2 calls (lines 12, 136)
- **Views/ManageUsers/PartnerApproval.cshtml** - 1 call (line 460)
- **Views/Agent/List.cshtml** - 1 call (line 574) 
- **Views/Attendance/Index.cshtml** - 1 call (line 600)

**Total jQuery calls needing fixes: 34**

---

## Standard Fix Patterns:

### For fetch() (✓ Already using this):
```javascript
const token = document.querySelector('input[name="__RequestVerificationToken"]');
if (token) {
    formData.append('__RequestVerificationToken', token.value);
    // OR in headers:
    headers: { 'RequestVerificationToken': token.value }
}
```

### For $.ajax():
```javascript
$.ajax({
    url: '/Controller/Action',
    type: 'POST',
    data: { 
        param: value,
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    },
    success: function(result) { }
});
```

### For $.post():
```javascript
$.post('/Controller/Action', {
    param: value,
    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
}, function(result) { });
```

---

## Scripts Created:

1. **Scripts/Fix-AntiForgeryTokens.ps1** - Auto-adds [ValidateAntiForgeryToken] to controllers ✓ USED
2. **Scripts/Verify-AntiForgeryTokens.ps1** - Verifies fetch() calls have tokens
3. **Scripts/Verify-Extended.ps1** - Checks all POST patterns (fetch, $.ajax, $.post)

---

## Next Steps:

1. **HIGH PRIORITY**: Fix jQuery $.ajax() and $.post() calls (34 instances)
   - Focus on: ManageUsers, Tasks/Calendar, Layout, WhatsApp modal
2. **Consider**: Some files like Index_Old.cshtml, Roles_Old.cshtml, Test.cshtml might be unused - verify if they need fixing
3. **Add @Html.AntiForgeryToken()** to pages that don't have it yet

**Current Protection Level: ~60% (Controllers: 100%, fetch API: 100%, jQuery: 0%)**

### ✅ 1. Views/Leads/Index.cshtml
**Line ~1412-1424** - UploadLeadFile function - **FIXED**

### ✅ 2. Views/Properties/Details.cshtml  
**All 7 functions FIXED:**
- Line ~726-730 - uploadImage function
- Line ~745 - deleteImage function  
- Line ~766-771 - uploadDocument function
- Line ~785 - deleteDocument function
- Line ~709 - deleteFlat function - **NEWLY FIXED**
- Line ~875 - deleteProperty function - **NEWLY FIXED**
- Line ~915-920 - bulk upload flats function

### ✅ 3. Views/Properties/Index.cshtml
**Line ~998** - Bulk upload properties - **FIXED**

### ✅ 4. Views/Properties/_PropertyModal.cshtml
**Line ~126** - SaveProperty function - **FIXED**

### ✅ 5. Views/Properties/_FlatModal.cshtml
**Line ~146** - SaveFlat function - **FIXED**

### ✅ 6. Views/Properties/_BulkUploadFlatsModal.cshtml
**Line ~53** - Bulk upload flats modal - **FIXED**

### ✅ 7. Views/Shared/_UploadsPartial.cshtml
**Line ~134** - File upload - **FIXED**

### ✅ 8. Views/ManageUsers/RolePermissions.cshtml
**Line ~239** - Save permissions - **FIXED**

### ✅ 9. Views/Agent/Details.cshtml
**Both functions FIXED:**
- Line ~437 - Upload document form
- Line ~471 - Delete document function

### ✅ 10. Views/WebhookLeads/Index.cshtml
**Both functions FIXED:**
- Line ~427 - assignLeads function (assign leads to executives)
- Line ~473 - deleteLead function (delete webhook lead)

### ✅ 11. Views/SalesPipelines/Index.cshtml
**Function FIXED:**
- Line ~392 - updateLeadStage function (drag-and-drop stage updates)

---

## Controllers Fixed (2):

### ✅ PropertiesController.cs
- Line 114: `SaveProperty` - **ADDED [ValidateAntiForgeryToken]**
- Line 241: `Delete` - **ADDED [ValidateAntiForgeryToken]**
- Line 818: `DeleteFlat` - Already had token ✓

---

## ⚠️ Controllers Still Needing [ValidateAntiForgeryToken]:

### HIGH PRIORITY (Active user features):

#### ❌ LeadsController.cs
- Line 750: `SaveNote` - NEEDS TOKEN + View fix
- Line 1153: `UploadLeadFile` - NEEDS TOKEN (View fixed)
- Line 1530: `Delete` - NEEDS TOKEN + View fix

#### ❌ BookingsController.cs  
- Line 1030: `CancelBooking` - NEEDS TOKEN + View fix

#### ❌ ManageUsersController.cs
- Line 489: `CreatePartner` - NEEDS TOKEN
- Line 706: `ApprovePartner` - NEEDS TOKEN
- Line 777: `RejectPartner` - NEEDS TOKEN
- Line 952: `UploadPartnerDocument` - NEEDS TOKEN
- Line 999: `ApprovePartnerDocument` - NEEDS TOKEN
- Line 1032: `RejectPartnerDocument` - NEEDS TOKEN

#### ❌ AttendanceController.cs
- Line 462: `RequestCorrection` - NEEDS TOKEN + View fix

### MEDIUM PRIORITY (Admin features):

#### ❌ SubscriptionController.cs
- Line 91: `CreatePlan` - NEEDS TOKEN
- Line 123: `UpdatePlan` - NEEDS TOKEN
- Line 190: `TogglePlan` - NEEDS TOKEN
- Line 301: `AdminChangePlan` - NEEDS TOKEN
- Line 1214: `MarkRefundProcessed` - NEEDS TOKEN
- Line 1316: `AdminUpgradePlan` - NEEDS TOKEN

#### ❌ SettingsController.cs
- Line 46: `UpdateSettings` - NEEDS TOKEN
- Line 305: `UpdateBranding` - NEEDS TOKEN

### LOW PRIORITY (Background/Notifications):

#### ❌ HomeController.cs
- Line 360: `MarkNotificationRead` - NEEDS TOKEN
- Line 376: `MarkAllNotificationsRead` - NEEDS TOKEN

---

## Standard Fix Pattern Applied:

```javascript
const formData = new FormData();
// ... append other data ...

// Add anti-forgery token
const token = document.querySelector('input[name="__RequestVerificationToken"]');
if (token) {
    formData.append('__RequestVerificationToken', token.value);
}

fetch(url, {
    method: 'POST',
    body: formData
})
```

## Alternative Pattern (for headers):

```javascript
fetch(url, {
    method: 'POST',
    body: formData,
    headers: {
        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
    }
})
```
