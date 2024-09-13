using FinalDern_Support.Models.Dto.RequestDtos;
using System.Security.Claims;

namespace FinalDern_Support.Repositories.Interfaces
{
    public interface ITechnician
    {
        public Task<object> GetAllNonTakenJobs(ClaimsPrincipal principal);
        public Task<object> TakeJob(ClaimsPrincipal principal, int quoteID);
        public Task<object> GetAllFeedBacks(ClaimsPrincipal principal);
        public Task<object> GetJobsFeedBack(ClaimsPrincipal principal, int jobID);
        public Task<object> TakeSpareParts(ClaimsPrincipal principal, int partsID, int jobID);
        public Task<object> PostReport(ClaimsPrincipal principal, int jobID, ReportContent reportContent);
        public Task<object> FinishJob(ClaimsPrincipal principal, int jobID);
        public Task<object> GetAllCompletedJobs(ClaimsPrincipal principal);
    }
}
