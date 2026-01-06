using CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Services
{
    public class SubscriptionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(AppDbContext context, ILogger<SubscriptionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PartnerSubscriptionModel?> GetActiveSubscriptionAsync(int channelPartnerId)
        {
            return await _context.PartnerSubscriptions
                .Include(s => s.Plan)
                .FirstOrDefaultAsync(s => s.ChannelPartnerId == channelPartnerId && 
                                         s.Status == "Active" && 
                                         s.EndDate > DateTime.Now);
        }

        public async Task<List<SubscriptionPlanModel>> GetAvailablePlansAsync()
        {
            return await _context.SubscriptionPlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.MonthlyPrice)
                .ToListAsync();
        }

        public async Task<(bool CanAdd, string Message)> CanAddAgentAsync(int channelPartnerId)
        {
            var subscription = await GetActiveSubscriptionAsync(channelPartnerId);
            if (subscription?.Plan == null)
                return (false, "No active subscription found");

            var currentAgentCount = await _context.Users
                .CountAsync(u => u.ChannelPartnerId == channelPartnerId && 
                               u.IsActive && 
                               (u.Role == "Sales" || u.Role == "Agent"));

            if (subscription.Plan.MaxAgents == -1)
                return (true, "");

            if (currentAgentCount >= subscription.Plan.MaxAgents)
                return (false, $"Agent limit reached. Your {subscription.Plan.PlanName} plan allows {subscription.Plan.MaxAgents} agents only.");

            return (true, "");
        }

        public async Task<(bool CanAdd, string Message)> CanAddLeadAsync(int channelPartnerId)
        {
            var subscription = await GetActiveSubscriptionAsync(channelPartnerId);
            if (subscription?.Plan == null)
                return (false, "No active subscription found");

            if (subscription.Plan.MaxLeadsPerMonth == -1)
                return (true, "");

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var currentMonthLeads = await _context.Leads
                .CountAsync(l => l.ChannelPartnerId == channelPartnerId && 
                               l.CreatedOn.Month == currentMonth && 
                               l.CreatedOn.Year == currentYear);

            if (currentMonthLeads >= subscription.Plan.MaxLeadsPerMonth)
                return (false, $"Monthly lead limit reached.");

            return (true, "");
        }

        public async Task<(bool CanUpload, string Message, long CurrentUsageGB, long LimitGB)> CanUploadFileAsync(int channelPartnerId, long fileSizeBytes)
        {
            var subscription = await GetActiveSubscriptionAsync(channelPartnerId);
            if (subscription?.Plan == null)
                return (false, "No active subscription found", 0, 0);

            if (subscription.Plan.MaxStorageGB == -1)
                return (true, "", 0, -1);

            // Calculate current storage usage from AgentDocuments and ChannelPartnerDocuments using FileSize
            var agentDocsSize = await _context.AgentDocuments
                .Where(d => d.Agent != null && d.Agent.ChannelPartnerId == channelPartnerId)
                .SumAsync(d => (long?)d.FileSize) ?? 0;

            var partnerDocsSize = await _context.ChannelPartnerDocuments
                .Where(d => d.ChannelPartnerId == channelPartnerId)
                .SumAsync(d => (long?)d.FileSize) ?? 0;

            var totalUsageBytes = agentDocsSize + partnerDocsSize;
            var totalUsageGB = totalUsageBytes / (1024.0 * 1024.0 * 1024.0);
            var fileSizeGB = fileSizeBytes / (1024.0 * 1024.0 * 1024.0);
            var newTotalGB = totalUsageGB + fileSizeGB;

            if (newTotalGB > subscription.Plan.MaxStorageGB)
            {
                return (false, 
                    $"Storage limit exceeded. Your {subscription.Plan.PlanName} plan allows {subscription.Plan.MaxStorageGB}GB. Current usage: {totalUsageGB:F2}GB",
                    (long)Math.Ceiling(totalUsageGB), 
                    subscription.Plan.MaxStorageGB);
            }

            return (true, "", (long)Math.Ceiling(totalUsageGB), subscription.Plan.MaxStorageGB);
        }

        public async Task<bool> HasFeatureAccessAsync(int channelPartnerId, string featureName)
        {
            var subscription = await GetActiveSubscriptionAsync(channelPartnerId);
            if (subscription?.Plan == null)
                return false;

            return featureName.ToLower() switch
            {
                "whatsapp" => subscription.Plan.HasWhatsAppIntegration,
                "facebook" => subscription.Plan.HasFacebookIntegration,
                "email" => subscription.Plan.HasEmailIntegration,
                "customapi" => subscription.Plan.HasCustomAPIAccess,
                "advancedreports" => subscription.Plan.HasAdvancedReports,
                "customreports" => subscription.Plan.HasCustomReports,
                "dataexport" => subscription.Plan.HasDataExport,
                "prioritysupport" => subscription.Plan.HasPrioritySupport,
                "phonesupport" => subscription.Plan.HasPhoneSupport,
                "dedicatedmanager" => subscription.Plan.HasDedicatedManager,
                _ => false
            };
        }

        public async Task<PartnerSubscriptionModel> CreateSubscriptionAsync(int channelPartnerId, int planId, string billingCycle, string paymentTransactionId)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(planId);
            if (plan == null)
                throw new ArgumentException("Invalid plan ID");

            var existingSubscription = await GetActiveSubscriptionAsync(channelPartnerId);
            if (existingSubscription != null)
            {
                existingSubscription.Status = "Cancelled";
                existingSubscription.UpdatedOn = DateTime.Now;
            }

            var amount = billingCycle.ToLower() == "annual" ? plan.YearlyPrice : plan.MonthlyPrice;
            var endDate = billingCycle.ToLower() == "annual" 
                ? DateTime.Now.AddYears(1) 
                : DateTime.Now.AddMonths(1);

            var subscription = new PartnerSubscriptionModel
            {
                ChannelPartnerId = channelPartnerId,
                PlanId = planId,
                BillingCycle = billingCycle ?? "monthly",
                Amount = amount,
                StartDate = DateTime.Now,
                EndDate = endDate,
                Status = "Active",
                PaymentTransactionId = paymentTransactionId,
                CreatedOn = DateTime.Now
            };

            _context.PartnerSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }

        public async Task<PartnerSubscriptionModel> CreateScheduledSubscriptionAsync(int channelPartnerId, int planId, string billingCycle, DateTime startDate)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(planId);
            if (plan == null)
                throw new ArgumentException("Invalid plan ID");

            var amount = billingCycle.ToLower() == "annual" ? plan.YearlyPrice : plan.MonthlyPrice;
            var endDate = billingCycle.ToLower() == "annual" 
                ? startDate.AddYears(1) 
                : startDate.AddMonths(1);

            var subscription = new PartnerSubscriptionModel
            {
                ChannelPartnerId = channelPartnerId,
                PlanId = planId,
                BillingCycle = billingCycle ?? "monthly",
                Amount = amount,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Scheduled",
                CreatedOn = DateTime.Now
            };

            _context.PartnerSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }

        public async Task UpdateUsageStatsAsync(int channelPartnerId)
        {
            var subscription = await GetActiveSubscriptionAsync(channelPartnerId);
            if (subscription == null) return;

            var agentCount = await _context.Users
                .CountAsync(u => u.ChannelPartnerId == channelPartnerId && 
                               u.IsActive && 
                               (u.Role == "Sales" || u.Role == "Agent"));

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var leadCount = await _context.Leads
                .CountAsync(l => l.ChannelPartnerId == channelPartnerId && 
                               l.CreatedOn.Month == currentMonth && 
                               l.CreatedOn.Year == currentYear);

            subscription.CurrentAgentCount = agentCount;
            subscription.CurrentMonthLeads = leadCount;
            subscription.UpdatedOn = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }
}