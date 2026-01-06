# 🔧 DATABASE HOTFIX REQUIRED

## ⚠️ ERROR ENCOUNTERED:
```
SqlException: Invalid column name 'WebhookEventId'.
```

## 🎯 SOLUTION:

### **Execute the Hotfix SQL Script**

1. **Open SQL Server Management Studio (SSMS)**
2. **Connect to your database server**
3. **Open this file:**
   ```
   SQL_Scripts/EXECUTE_THIS_HOTFIX.sql
   ```
4. **Execute the script** (Press F5 or click Execute)

### **What the hotfix does:**

✅ Adds `WebhookEventId` column to `PaymentTransactions` table
✅ Adds `RowVersion` column to `Bookings` table (concurrency control)
✅ Adds verification fields to `AgentDocuments` table:
   - VerificationStatus
   - VerifiedBy
   - VerifiedOn
   - RejectionReason
✅ Adds verification fields to `ChannelPartnerDocuments` table:
   - VerificationStatus
   - VerifiedBy
   - VerifiedOn
   - RejectionReason
✅ Creates `WebhookRetryQueue` table (if not exists)
✅ Creates necessary indexes for performance

### **Safe to Run:**
- Script checks if columns exist before adding them
- Won't duplicate or break existing data
- Includes verification queries at the end

### **After Running the Script:**

1. Refresh your application (Ctrl+F5)
2. The `SqlException` error will be resolved
3. All P0 critical features will work properly

---

## 📊 **Why This Error Occurred:**

The P0 critical fixes (payment reconciliation, document verification, webhook retry) added new model properties, but the database schema wasn't updated. This hotfix synchronizes the database with the code changes.

---

## 🚀 **Next Steps After Hotfix:**

Once the hotfix is executed, your application will have:
- ✅ Payment webhook idempotency (prevents duplicate processing)
- ✅ Document verification workflow
- ✅ Booking concurrency control
- ✅ Webhook retry mechanism
- ✅ All 62 P1/P2/P3 features ready to implement

---

## ⏱️ **Estimated Time:**
- **Execution time:** < 5 seconds
- **Downtime:** None (can run on live database)

---

**Execute `SQL_Scripts/EXECUTE_THIS_HOTFIX.sql` now to fix the error!**
