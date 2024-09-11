using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using System.Security.Claims;

namespace FinalDern_Support.Repositories.Interfaces
{
    public interface ICustomer
    {
       public Task<Customer> AddCustomer(Customer customer);
       public Task<object> SubmitSupportRequest(ClaimsPrincipal principal,requestSupportRequestDto requestSupportRequestDto);
        public Task<object> GetAllQuotes(ClaimsPrincipal principal);
        public Task<object> UpdateQuoteStatusAsync(ClaimsPrincipal principal, string newStatus, int qouteID);
        public Task<object> GetCompletedJobsAsync(ClaimsPrincipal claims);
        public Task<object> PostFeedBack(ClaimsPrincipal principal, int JobID, PostFeedBackrequest postFeedBackrequest);
    }
}
