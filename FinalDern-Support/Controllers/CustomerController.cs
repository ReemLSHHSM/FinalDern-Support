using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("Submit")]
        public async Task<IActionResult> SubmitSupportRequest([FromBody] requestSupportRequestDto requestDto)
        {

            var result = await _customer.SubmitSupportRequest(User, requestDto); 

            return Ok(result);
        }
        [HttpGet("GetAllQoutes")]
        public async Task<IActionResult> GetAllQuotes()
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
        [HttpPut("HandleQuote")]
        public async Task<IActionResult> HandleQuote(string newStatus, int qouteID)
        {
            var result=_customer.UpdateQuoteStatusAsync(User, newStatus, qouteID);

            return  Ok(result);
        }

        [HttpGet("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            var result= _customer.GetCompletedJobsAsync(User);
            return Ok(result);

        }

        [HttpPost("SubmitFeedBack")]
        public async Task<IActionResult> SubmitFeedBack( int JobID, PostFeedBackrequest postFeedBackrequest)
        {
            var result=_customer.PostFeedBack(User, JobID, postFeedBackrequest);
            return Ok(result);
        }

    }
}
