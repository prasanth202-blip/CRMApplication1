# CRM API Endpoints - Postman Collection

Base URL: `https://localhost:44383`

---

## 🔐 Authentication APIs

### 1. Login
```
POST /Account/Login
Content-Type: application/x-www-form-urlencoded

Username=admin&Password=yourpassword
```

### 2. Register
```
POST /Account/Register
Content-Type: application/x-www-form-urlencoded

Username=testuser&Email=test@example.com&Password=Test@123&Role=Sales
```

### 3. Forgot Password
```
POST /Account/ForgotPassword
Content-Type: application/x-www-form-urlencoded

Email=user@example.com
```

---

## 👥 User Management APIs

### 4. Get All Users
```
GET /ManageUsers/Index
```

### 5. Create User
```
POST /ManageUsers/CreateUser
Content-Type: application/x-www-form-urlencoded

Username=newuser&Email=new@example.com&Password=Pass@123&Role=Agent
```

### 6. Update User
```
POST /ManageUsers/EditUser
Content-Type: application/x-www-form-urlencoded

UserId=1&Username=updateduser&Email=updated@example.com&Role=Manager
```

### 7. Delete User
```
POST /ManageUsers/DeleteUser
Content-Type: application/x-www-form-urlencoded

id=1
```

---

## 📞 Leads Management APIs

### 8. Get All Leads
```
GET /Leads/Index
```

### 9. Create Lead
```
POST /Leads/Create
Content-Type: application/x-www-form-urlencoded

FirstName=John&LastName=Doe&Email=john@example.com&Phone=9876543210&Status=New&Source=Website
```

### 10. Update Lead
```
POST /Leads/Edit
Content-Type: application/x-www-form-urlencoded

LeadId=1&FirstName=John&LastName=Doe&Status=Contacted&Priority=High
```

### 11. Delete Lead
```
POST /Leads/Delete
Content-Type: application/x-www-form-urlencoded

id=1
```

### 12. Bulk Delete Leads ⭐ NEW
```
POST /Leads/BulkDelete
Content-Type: application/json

{
    "leadIds": [1, 2, 3, 4]
}
```

### 13. Bulk Assign Leads ⭐ NEW
```
POST /Leads/BulkAssign
Content-Type: application/json

{
    "leadIds": [1, 2, 3],
    "assignedToUserId": 5
}
```

### 14. Bulk Update Status ⭐ NEW
```
POST /Leads/BulkUpdateStatus
Content-Type: application/json

{
    "leadIds": [1, 2, 3],
    "newStatus": "Qualified"
}
```

### 15. Bulk Export Leads ⭐ NEW
```
POST /Leads/BulkExport
Content-Type: application/json

{
    "leadIds": [1, 2, 3, 4]
}

Response: Excel file download
```

### 16. Convert Lead to Booking
```
POST /Leads/ConvertToBooking
Content-Type: application/x-www-form-urlencoded

leadId=1&propertyId=5&bookingAmount=50000
```

---

## 🏢 Properties APIs

### 17. Get All Properties
```
GET /Properties/Index
```

### 18. Create Property
```
POST /Properties/Create
Content-Type: application/x-www-form-urlencoded

PropertyName=Sunrise Apartments&Location=Bangalore&BuilderId=1&PropertyType=Apartment&Price=5000000
```

### 19. Update Property
```
POST /Properties/Edit
Content-Type: application/x-www-form-urlencoded

PropertyId=1&PropertyName=Updated Name&Status=Available
```

### 20. Delete Property
```
POST /Properties/Delete
Content-Type: application/x-www-form-urlencoded

id=1
```

---

## 📝 Bookings APIs

### 21. Get All Bookings
```
GET /Bookings/Index
```

### 22. Create Booking
```
POST /Bookings/Create
Content-Type: application/x-www-form-urlencoded

LeadId=1&PropertyId=5&BookingAmount=50000&BookingDate=2026-01-10
```

### 23. Update Booking
```
POST /Bookings/Edit
Content-Type: application/x-www-form-urlencoded

BookingId=1&Status=Confirmed&PaymentReceived=50000
```

---

## 🤝 Agents APIs

### 24. Get All Agents
```
GET /Agent/Index
```

### 25. Create Agent
```
POST /Agent/Create
Content-Type: application/x-www-form-urlencoded

Name=Agent Name&Email=agent@example.com&Phone=9876543210&AgentType=Field Agent&BaseSalary=30000
```

### 26. Update Agent
```
POST /Agent/Edit
Content-Type: application/x-www-form-urlencoded

AgentId=1&Name=Updated Name&Status=Active
```

### 27. Get Agent Details
```
GET /Agent/Details?id=1
```

---

## 💰 Agent Payout APIs

### 28. Get Agent Payouts
```
GET /AgentPayout/Index
```

### 29. Calculate Agent Payout
```
POST /AgentPayout/Calculate
Content-Type: application/x-www-form-urlencoded

agentId=1&month=January 2026
```

### 30. Approve Agent Payout
```
POST /AgentPayout/Approve
Content-Type: application/x-www-form-urlencoded

payoutId=1
```

---

## 🏦 Channel Partner APIs

### 31. Get All Partners
```
GET /ManageUsers/ChannelPartners
```

### 32. Create Partner
```
POST /ManageUsers/CreateChannelPartner
Content-Type: application/x-www-form-urlencoded

CompanyName=Partner Company&ContactPerson=John Doe&Email=partner@example.com&Phone=9876543210
```

### 33. Update Partner
```
POST /ManageUsers/EditChannelPartner
Content-Type: application/x-www-form-urlencoded

ChannelPartnerId=1&CompanyName=Updated Company&Status=Active
```

---

## 💳 Partner Payout APIs

### 34. Get Partner Payouts
```
GET /ChannelPartnerPayout/Index
```

### 35. Calculate Partner Payout
```
POST /ChannelPartnerPayout/Calculate
Content-Type: application/x-www-form-urlencoded

partnerId=1&month=January 2026
```

### 36. Approve Partner Payout
```
POST /ChannelPartnerPayout/Approve
Content-Type: application/x-www-form-urlencoded

payoutId=1
```

---

## 📅 Attendance APIs

### 37. Get Attendance Calendar
```
GET /Attendance/Calendar?agentId=1
```

### 38. Mark Attendance
```
POST /Attendance/MarkAttendance
Content-Type: application/x-www-form-urlencoded

agentId=1&date=2026-01-03&status=Present&checkInTime=09:00&checkOutTime=18:00
```

### 39. Request Leave
```
POST /Attendance/RequestLeave
Content-Type: application/x-www-form-urlencoded

agentId=1&fromDate=2026-01-10&toDate=2026-01-12&reason=Medical Leave
```

---

## 📊 Revenue & Profit APIs

### 40. Get Revenue Report
```
GET /Revenue/Index?fromDate=2026-01-01&toDate=2026-01-31
```

### 41. Add Revenue Entry
```
POST /Revenue/Create
Content-Type: application/x-www-form-urlencoded

Amount=100000&Type=Booking&Description=Property Sale&Date=2026-01-03
```

### 42. Get Profit Report
```
GET /Profit/Index?month=January&year=2026
```

---

## 💸 Expenses APIs

### 43. Get All Expenses
```
GET /Expenses/Index
```

### 44. Create Expense
```
POST /Expenses/Create
Content-Type: application/x-www-form-urlencoded

Amount=5000&Type=Marketing&Description=Facebook Ads&Date=2026-01-03
```

### 45. Update Expense
```
POST /Expenses/Edit
Content-Type: application/x-www-form-urlencoded

ExpenseId=1&Amount=6000&Status=Approved
```

---

## 📄 Invoice APIs

### 46. Get All Invoices
```
GET /Invoices/Index
```

### 47. Create Invoice
```
POST /Invoices/Create
Content-Type: application/x-www-form-urlencoded

LeadId=1&Amount=100000&DueDate=2026-01-15&Items=[{"Description":"Property","Amount":100000}]
```

### 48. Send Invoice Email
```
POST /Invoices/SendInvoice
Content-Type: application/x-www-form-urlencoded

invoiceId=1
```

### 49. Download Invoice PDF
```
GET /Invoices/DownloadPdf?id=1

Response: PDF file
```

---

## 💳 Payment APIs

### 50. Get All Payments
```
GET /Payments/Index
```

### 51. Record Payment
```
POST /Payments/Create
Content-Type: application/x-www-form-urlencoded

InvoiceId=1&Amount=50000&PaymentMethod=Bank Transfer&TransactionReference=TXN123456
```

---

## 📦 Subscription APIs

### 52. Get Subscription Plans
```
GET /Subscription/Index
```

### 53. Create Subscription Plan
```
POST /Subscription/CreatePlan
Content-Type: application/json

{
    "planName": "Professional",
    "monthlyPrice": 299,
    "yearlyPrice": 2999,
    "maxLeads": 1000,
    "maxAgents": 10,
    "features": "Advanced Reports, Priority Support"
}
```

### 54. Select Plan (Create Razorpay Order)
```
POST /Subscription/SelectPlan
Content-Type: application/x-www-form-urlencoded

planId=1&billingCycle=monthly

Response:
{
    "success": true,
    "orderId": "order_MfKIQPM6zXK7Pe",
    "amount": 29900,
    "planName": "Professional"
}
```

### 55. Confirm Payment
```
POST /Subscription/ConfirmPayment
Content-Type: application/x-www-form-urlencoded

razorpayPaymentId=pay_MfKJxAbc123&razorpayOrderId=order_MfKIQPM6zXK7Pe&razorpaySignature=abc123&planId=1&billingCycle=monthly
```

### 56. Get Transactions
```
GET /Subscription/Transactions?type=Subscription&fromDate=2026-01-01&toDate=2026-01-31
```

### 57. Mark Refund Processed
```
POST /Subscription/MarkRefundProcessed
Content-Type: application/x-www-form-urlencoded

subscriptionId=22&refundNotes=Customer requested cancellation

Response:
{
    "success": true,
    "message": "Refund of ₹299 processed successfully! Refund ID: rfnd_MfKIQPM6zXK7Pe"
}
```

---

## 🔔 Notification APIs

### 58. Get All Notifications
```
GET /Notification/Index?userId=1
```

### 59. Mark as Read
```
POST /Notification/MarkAsRead
Content-Type: application/x-www-form-urlencoded

notificationId=1
```

### 60. Mark All as Read
```
POST /Notification/MarkAllAsRead
```

### 61. Get Unread Count
```
GET /Notification/UnreadCount?userId=1

Response: { "count": 5 }
```

---

## 📱 FCM (Push Notifications) APIs

### 62. Save Device Token
```
POST /Fcm/SaveToken
Content-Type: application/json

{
    "userId": 1,
    "token": "fcm_device_token_here"
}
```

### 63. Send Push Notification
```
POST /Fcm/SendNotification
Content-Type: application/json

{
    "userId": 1,
    "title": "New Lead",
    "body": "You have a new lead assigned",
    "data": {
        "leadId": "123"
    }
}
```

---

## 📲 WhatsApp APIs

### 64. Send WhatsApp Message
```
POST /WhatsApp/SendMessage
Content-Type: application/json

{
    "phoneNumber": "+919876543210",
    "message": "Hello from CRM!"
}
```

### 65. Send Lead Follow-up
```
POST /WhatsApp/SendLeadFollowUp
Content-Type: application/json

{
    "leadId": 1,
    "message": "Thank you for your interest!"
}
```

---

## 🪝 Webhook APIs

### 66. Razorpay Payment Webhook
```
POST /Webhook/RazorpayPayment
Content-Type: application/json
X-Razorpay-Signature: signature_here

{
    "event": "payment.captured",
    "payload": {
        "payment": {
            "entity": {
                "id": "pay_xxx",
                "amount": 29900,
                "status": "captured"
            }
        }
    }
}
```

### 67. Facebook Leads Webhook
```
POST /WebhookLeads/FacebookLeads
Content-Type: application/json

{
    "object": "page",
    "entry": [...]
}
```

---

## 📊 Dashboard APIs

### 68. Get Dashboard Stats
```
GET /Home/Index

Response: HTML with stats or JSON if Accept: application/json
```

### 69. Get Lead Statistics
```
GET /Leads/GetStatistics?fromDate=2026-01-01&toDate=2026-01-31

Response:
{
    "totalLeads": 150,
    "newLeads": 45,
    "converted": 12,
    "conversionRate": 8.0
}
```

---

## 🎯 Sales Pipeline APIs

### 70. Get Pipeline View
```
GET /SalesPipelines/Index
```

### 71. Move Lead in Pipeline
```
POST /SalesPipelines/MoveLead
Content-Type: application/json

{
    "leadId": 1,
    "newStage": "Negotiation"
}
```

---

## ⚙️ Settings APIs

### 72. Get All Settings
```
GET /Settings/Index
```

### 73. Update Setting
```
POST /Settings/Update
Content-Type: application/x-www-form-urlencoded

SettingKey=CompanyName&SettingValue=My CRM Company
```

### 74. Upload Company Logo
```
POST /Settings/UploadLogo
Content-Type: multipart/form-data

logo=@/path/to/logo.png
```

---

## 🔍 Search & Filter APIs

### 75. Search Leads
```
GET /Leads/Search?query=john&status=New&source=Website
```

### 76. Advanced Lead Filters
```
POST /Leads/AdvancedFilter
Content-Type: application/json

{
    "status": ["New", "Contacted"],
    "priority": ["High"],
    "source": ["Website", "Referral"],
    "assignedTo": [1, 2],
    "dateFrom": "2026-01-01",
    "dateTo": "2026-01-31"
}
```

---

## 📈 Reports APIs

### 77. Lead Conversion Report
```
GET /Reports/LeadConversion?month=January&year=2026
```

### 78. Agent Performance Report
```
GET /Reports/AgentPerformance?agentId=1&month=January&year=2026
```

### 79. Revenue Report
```
GET /Reports/Revenue?fromDate=2026-01-01&toDate=2026-01-31
```

### 80. Export Report
```
GET /Reports/Export?type=leads&format=excel

Response: Excel file download
```

---

## 🔐 Security Headers

All authenticated requests should include:

```
Cookie: jwtToken=your_jwt_token_here
X-Requested-With: XMLHttpRequest
```

For POST requests with anti-forgery token:
```
Cookie: jwtToken=your_jwt_token_here; .AspNetCore.Antiforgery.xxx=token
__RequestVerificationToken: token_value
```

---

## 📝 Response Formats

### Success Response
```json
{
    "success": true,
    "message": "Operation completed successfully",
    "data": { ... }
}
```

### Error Response
```json
{
    "success": false,
    "message": "Error description",
    "errorCode": "ERROR_CODE"
}
```

---

## 🧪 Test Credentials

### Admin User
```
Username: admin
Password: Admin@123
```

### Test Razorpay Cards
```
Card Number: 4111 1111 1111 1111
CVV: Any 3 digits
Expiry: Any future date
```

### Test UPI
```
UPI ID: success@razorpay
```

---

## 🚀 Postman Collection Import

Save this as `CRM_APIs.postman_collection.json` and import into Postman.

Want me to generate the actual Postman JSON collection file?
