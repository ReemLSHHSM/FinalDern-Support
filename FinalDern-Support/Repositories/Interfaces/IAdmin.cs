using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace FinalDern_Support.Repositories.Interfaces
{
    public interface IAdmin
    {
        public Task<object> AddTechnician(ClaimsPrincipal principal,TechnicianDto technicianDto, ModelStateDictionary modelState);
        public Task<object> AcceptSupportRequest(ClaimsPrincipal principal, int RequestID);
        public Task<object> GetAllPendingRequests(ClaimsPrincipal principal);
        public Task<object> GetAllAcceptedRequests(ClaimsPrincipal principal);
        public Task<object> SubmitQuote(ClaimsPrincipal principal, SubmitQuote submitQuote, int RequestID);
        public Task<object> GetAllReports(ClaimsPrincipal principal);
        public Task<object> GetReportsAnalyticsAsync(ClaimsPrincipal principal);
        public Task<object> PostArticle(ClaimsPrincipal principal, Knowledge_BaseDto knowledge_BaseDto);
        public Task<object> EditArticle(ClaimsPrincipal principal, int articleID, Knowledge_BaseDto knowledge_BaseDto);
        public Task<object> DeleteArticle(ClaimsPrincipal principal, int articleID);



        }
}
