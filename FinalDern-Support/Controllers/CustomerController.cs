using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalDern_Support.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customer;
        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }

        // POST: api/SupportRequest
        [HttpPost]
        [Route("SubmitSupportRequest")]
        public async Task<IActionResult> SubmitSupportRequest([FromBody] requestSupportRequestDto requestDto)
        {
            var result = await _customer.SubmitSupportRequest(User, requestDto);
            return Ok(result);
        }

        [HttpGet("GetAllQoutes")]
        public async Task<ActionResult<object>> GetAllQuotes()
        {
            var result = await _customer.GetAllQuotes(User);

            if (result is ResultDto resultDto)
            {
                // If the result is an error, return the error details
                return StatusCode(resultDto.StatusCode, resultDto);
            }

            // If the result is not a ResultDto, assume it's a list of quotes and return it
            return Ok(result);
        }

        [HttpPut("HandleQuote/{qouteID}")]
        public async Task<IActionResult> HandleQuote([FromBody] string newStatus, int qouteID)
        {
            var result = await _customer.UpdateQuoteStatusAsync(User, newStatus, qouteID);
            return Ok(result);
        }

        [HttpGet("GetAllJobs")]
        public async Task<ActionResult<object>> GetAllJobs()
        {
            var result = await _customer.GetCompletedJobsAsync(User);
            return Ok(result);
        }

        [HttpPost("SubmitFeedBack/{JobID}")]
        public async Task<IActionResult> SubmitFeedBack(int JobID, PostFeedBackrequest postFeedBackrequest)
        {
            var result = await _customer.PostFeedBack(User, JobID, postFeedBackrequest);
            return Ok(result);
        }

        
    }
}
