# Facebook LeadGen Webhook API Documentation

## 📋 Overview

This document contains all the API endpoints and cURL commands for testing the Facebook LeadGen webhook integration.

---

## 🔗 Base URL

- **Local Development:** `https://localhost:44383`
- **Production:** `https://yourdomain.com`

---

## 🔐 Authentication

- **Verify Token:** `MY_SECRET_TOKEN_123`

---

## 📍 API Endpoints

### 1. Test API Connection

**Endpoint:** `GET /api/meta/test`

**Purpose:** Verify that the API controller is working

**cURL Command:**
```bash
curl --location 'https://localhost:44383/api/meta/test'
```

**PowerShell:**
```powershell
Invoke-RestMethod -Uri "https://localhost:44383/api/meta/test" -Method GET -SkipCertificateCheck
```

**Expected Response:**
```json
{
  "message": "API Controller is working!",
  "timestamp": "2025-12-02T10:30:00"
}
```

---

### 2. Webhook Verification (GET)

**Endpoint:** `GET /api/meta/webhook`

**Purpose:** Facebook uses this to verify your webhook during setup

**Parameters:**
- `hub.mode` - Should be "subscribe"
- `hub.verify_token` - Your verification token
- `hub.challenge` - Random string from Facebook to echo back

**cURL Command:**
```bash
curl --location 'https://localhost:44383/api/meta/webhook?hub.mode=subscribe&hub.verify_token=MY_SECRET_TOKEN_123&hub.challenge=test_challenge_12345'
```

**PowerShell:**
```powershell
Invoke-RestMethod -Uri "https://localhost:44383/api/meta/webhook?hub.mode=subscribe&hub.verify_token=MY_SECRET_TOKEN_123&hub.challenge=test_challenge_12345" -Method GET -SkipCertificateCheck
```

**Postman:**
- **Method:** GET
- **URL:** `https://localhost:44383/api/meta/webhook?hub.mode=subscribe&hub.verify_token=MY_SECRET_TOKEN_123&hub.challenge=test_challenge_12345`
- **Headers:** None required

**Expected Response:**
```
test_challenge_12345
```
(Plain text response with the challenge value)

---

### 3. Receive Lead Data (POST)

**Endpoint:** `POST /api/meta/webhook`

**Purpose:** Receives lead data from Facebook when someone submits a lead form

**Headers:**
- `Content-Type: application/json`

**cURL Command - Single Lead:**
```bash
curl --location 'https://localhost:44383/api/meta/webhook' \
--header 'Content-Type: application/json' \
--data '{
  "object": "page",
  "entry": [
    {
      "id": "123456789",
      "time": 1735738539,
      "changes": [
        {
          "field": "leadgen",
          "value": {
            "leadgen_id": "738292918",
            "created_time": "2025-01-02T10:10:10+0000",
            "field_data": [
              { "name": "first_name", "values": ["Mahi"] },
              { "name": "last_name", "values": ["Reddy"] },
              { "name": "phone_number", "values": ["9876543210"] },
              { "name": "bhk", "values": ["3 BHK"] },
              { "name": "location", "values": ["Hyderabad"] },
              { "name": "budget", "values": ["80 Lakhs"] }
            ]
          }
        }
      ]
    }
  ]
}'
```

**PowerShell - Single Lead:**
```powershell
$json = @'
{
  "object": "page",
  "entry": [
    {
      "id": "123456789",
      "time": 1735738539,
      "changes": [
        {
          "field": "leadgen",
          "value": {
            "leadgen_id": "738292918",
            "created_time": "2025-01-02T10:10:10+0000",
            "field_data": [
              { "name": "first_name", "values": ["Mahi"] },
              { "name": "last_name", "values": ["Reddy"] },
              { "name": "phone_number", "values": ["9876543210"] },
              { "name": "bhk", "values": ["3 BHK"] },
              { "name": "location", "values": ["Hyderabad"] },
              { "name": "budget", "values": ["80 Lakhs"] }
            ]
          }
        }
      ]
    }
  ]
}
'@

Invoke-RestMethod -Uri "https://localhost:44383/api/meta/webhook" -Method POST -Body $json -ContentType "application/json" -SkipCertificateCheck
```

**Expected Response:**
```json
{
  "status": "success",
  "leadsProcessed": 1,
  "leadIds": [19]
}
```

---

### 4. Multiple Leads (POST)

**cURL Command - Multiple Leads:**
```bash
curl --location 'https://localhost:44383/api/meta/webhook' \
--header 'Content-Type: application/json' \
--data '{
  "object": "page",
  "entry": [
    {
      "id": "123456789",
      "time": 1735738539,
      "changes": [
        {
          "field": "leadgen",
          "value": {
            "leadgen_id": "738292918",
            "created_time": "2025-01-02T10:10:10+0000",
            "field_data": [
              { "name": "first_name", "values": ["Ravi"] },
              { "name": "last_name", "values": ["Kumar"] },
              { "name": "phone_number", "values": ["9999888877"] },
              { "name": "bhk", "values": ["2 BHK"] },
              { "name": "location", "values": ["Bangalore"] },
              { "name": "budget", "values": ["60 Lakhs"] }
            ]
          }
        },
        {
          "field": "leadgen",
          "value": {
            "leadgen_id": "738292919",
            "created_time": "2025-01-02T10:15:10+0000",
            "field_data": [
              { "name": "first_name", "values": ["Priya"] },
              { "name": "last_name", "values": ["Sharma"] },
              { "name": "phone_number", "values": ["8888777766"] },
              { "name": "bhk", "values": ["4 BHK"] },
              { "name": "location", "values": ["Mumbai"] },
              { "name": "budget", "values": ["1.2 Crore"] }
            ]
          }
        }
      ]
    }
  ]
}'
```

**Expected Response:**
```json
{
  "status": "success",
  "leadsProcessed": 2,
  "leadIds": [20, 21]
}
```

---

### 5. Debug Endpoint (POST)

**Endpoint:** `POST /api/meta/debug`

**Purpose:** Debug JSON deserialization to troubleshoot issues

**cURL Command:**
```bash
curl --location 'https://localhost:44383/api/meta/debug' \
--header 'Content-Type: application/json' \
--data '{
  "object": "page",
  "entry": [
    {
      "id": "123456789",
      "time": 1735738539,
      "changes": [
        {
          "field": "leadgen",
          "value": {
            "leadgen_id": "738292918",
            "created_time": "2025-01-02T10:10:10+0000",
            "field_data": [
              { "name": "first_name", "values": ["Test"] },
              { "name": "last_name", "values": ["User"] }
            ]
          }
        }
      ]
    }
  ]
}'
```

**Expected Response:**
```json
{
  "rawReceived": "{...}",
  "parsed": {
    "hasObject": true,
    "objectValue": "page",
    "entryCount": 1,
    "firstEntry": {
      "id": "123456789",
      "changesCount": 1,
      "firstChange": {
        "field": "leadgen",
        "hasValue": true,
        "fieldDataCount": 2
      }
    }
  }
}
```

---

## 📊 Field Mapping

Facebook fields are mapped to your CRM Leads table as follows:

| Facebook Field | CRM Field | Description |
|---------------|-----------|-------------|
| `first_name` + `last_name` | `Name` | Combined full name |
| `phone_number` | `Contact` | Phone number |
| `email` | `Email` | Email address (optional) |
| `bhk` | `BHK` | Property type (2 BHK, 3 BHK, etc.) |
| `location` | `PreferredLocation` | Preferred area/city |
| `budget` | `Requirement` | Budget range |
| `leadgen_id` | `GroupName` | Facebook lead ID |
| - | `Source` | Set to "Facebook LeadGen" |
| - | `Status` | Set to "New" |
| - | `Stage` | Set to "Lead" |
| - | `ExecutiveId` | NULL (assigned later by admin) |

---

## 🎯 Testing Workflow

### Step 1: Test API Connection
```bash
curl --location 'https://localhost:44383/api/meta/test'
```
✅ Should return: `{ "message": "API Controller is working!" }`

### Step 2: Test Webhook Verification
```bash
curl --location 'https://localhost:44383/api/meta/webhook?hub.mode=subscribe&hub.verify_token=MY_SECRET_TOKEN_123&hub.challenge=test_challenge_12345'
```
✅ Should return: `test_challenge_12345`

### Step 3: Send Test Lead
```bash
curl --location 'https://localhost:44383/api/meta/webhook' \
--header 'Content-Type: application/json' \
--data '{
  "object": "page",
  "entry": [{
    "id": "123456789",
    "time": 1735738539,
    "changes": [{
      "field": "leadgen",
      "value": {
        "leadgen_id": "738292918",
        "created_time": "2025-01-02T10:10:10+0000",
        "field_data": [
          { "name": "first_name", "values": ["Mahi"] },
          { "name": "last_name", "values": ["Reddy"] },
          { "name": "phone_number", "values": ["9876543210"] },
          { "name": "bhk", "values": ["3 BHK"] },
          { "name": "location", "values": ["Hyderabad"] },
          { "name": "budget", "values": ["80 Lakhs"] }
        ]
      }
    }]
  }]
}'
```
✅ Should return: `{ "status": "success", "leadsProcessed": 1, "leadIds": [19] }`

### Step 4: Verify in Admin Panel
Navigate to: `https://localhost:44383/WebhookLeads/Index`

✅ Should see the lead: Mahi Reddy with all details

### Step 5: Assign Lead
1. Select the lead checkbox
2. Choose an executive from dropdown
3. Click "Assign Selected"
4. Lead moves to executive's Leads page

---

## 🚀 Production Setup

### Facebook Business Manager Configuration

1. Go to **Facebook Business Manager** → **Lead Ads** → **Webhooks**
2. Click **Add Subscription**
3. Enter Webhook URL: `https://yourdomain.com/api/meta/webhook`
4. Enter Verify Token: `MY_SECRET_TOKEN_123`
5. Subscribe to: `leadgen`
6. Click **Verify and Save**

### Production URLs

Replace `localhost:44383` with your production domain:

**GET Verification:**
```
https://yourdomain.com/api/meta/webhook?hub.mode=subscribe&hub.verify_token=MY_SECRET_TOKEN_123&hub.challenge=CHALLENGE_FROM_FACEBOOK
```

**POST Receive Leads:**
```
https://yourdomain.com/api/meta/webhook
```

**Admin Panel:**
```
https://yourdomain.com/WebhookLeads/Index
```

---

## 🔍 Troubleshooting

### Issue: 404 Not Found
**Solution:** Make sure `app.MapControllers();` is in Program.cs before `app.Run();`

### Issue: Connection Reset
**Solution:** Restart application after code changes. Use HTTPS, not HTTP.

### Issue: leadsProcessed: 0
**Solution:** Check that JSON field names match exactly (use debug endpoint to verify)

### Issue: Lead not appearing in admin panel
**Solution:** 
1. Check database Leads table for new entries
2. Verify `Source = "Facebook LeadGen"` and `ExecutiveId IS NULL`
3. Check application logs in Visual Studio Output window

---

## 📝 Sample Payloads

### Minimal Payload (Only Required Fields)
```json
{
  "object": "page",
  "entry": [{
    "id": "123456789",
    "time": 1735738539,
    "changes": [{
      "field": "leadgen",
      "value": {
        "leadgen_id": "738292918",
        "created_time": "2025-01-02T10:10:10+0000",
        "field_data": [
          { "name": "first_name", "values": ["John"] },
          { "name": "last_name", "values": ["Doe"] },
          { "name": "phone_number", "values": ["1234567890"] }
        ]
      }
    }]
  }]
}
```

### Full Payload (All Fields)
```json
{
  "object": "page",
  "entry": [{
    "id": "123456789",
    "time": 1735738539,
    "changes": [{
      "field": "leadgen",
      "value": {
        "leadgen_id": "738292918",
        "created_time": "2025-01-02T10:10:10+0000",
        "field_data": [
          { "name": "first_name", "values": ["Mahi"] },
          { "name": "last_name", "values": ["Reddy"] },
          { "name": "phone_number", "values": ["9876543210"] },
          { "name": "email", "values": ["mahi.reddy@example.com"] },
          { "name": "bhk", "values": ["3 BHK"] },
          { "name": "location", "values": ["Hyderabad"] },
          { "name": "budget", "values": ["80 Lakhs"] }
        ]
      }
    }]
  }]
}
```

---

## 📞 Support

For issues or questions:
- Check application logs in Visual Studio Output window
- Use the debug endpoint to verify JSON parsing
- Review field mapping in this document
- Verify webhook URL is publicly accessible for production

---

**Last Updated:** December 2, 2025
**Version:** 1.0
