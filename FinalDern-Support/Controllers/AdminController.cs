using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinalDern_Support.Repositories.Interfaces;
using System.Security.Claims;
using FinalDern_Support.Models.Dto.ResponseDtos;

namespace FinalDern_Support.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _admin;
        public AdminController(IAdmin admin)
        {
            _admin = admin;
        }

        [HttpPost("AddTechnician")]
        public async Task<IActionResult> AddTechnician([FromBody] TechnicianDto technicianDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _admin.AddTechnician(User, technicianDto, ModelState);

            if (user == null)
            {
                return BadRequest(new { Message = "Adding Technician failed. Please check your details and try again." });
            }

            return Ok(user);
        }

        [HttpPut("AcceptSupportRequest/{requestID}")]
        public async Task<IActionResult> AcceptSupportRequest(int requestID)
        {
            var result = await _admin.AcceptSupportRequest(User, requestID);

            return Ok(result);
        }

        [HttpGet("GetAllPendingRequests")]
        public async Task<ActionResult<object>> GetAllPendingRequests()
        {
            var result = await _admin.GetAllPendingRequests(User);
            return Ok(result);
        }

        [HttpGet("GetAllAcceptedRequests")]
        public async Task<ActionResult<object>> GetAllAcceptedRequests()
        {
            var result = await _admin.GetAllAcceptedRequests(User);
            return Ok(result);
        }

        [HttpPost("SubmitQuote/{RequestID}")]
        public async Task<IActionResult> SubmitQuote([FromBody]SubmitQuote submitQuote, int RequestID)
        {
            var result = await _admin.SubmitQuote(User, submitQuote, RequestID);
            return Ok(result);
        }

        [HttpGet("GetAllReports")]
        public async Task<ActionResult<object>> GetAllReports()
        {
            var result = await _admin.GetAllReports(User);
            return Ok(result);
        }

        [HttpGet("GetAnalysis")]
        public async Task<ActionResult<object>> GetAnalysis()
        {
            var result = await _admin.GetReportsAnalyticsAsync(User);
            return Ok(result);
        }

        [HttpPost("PostArticle")]
        public async Task<IActionResult> PostArticle(Knowledge_BaseDto knowledge_BaseDto)
        {
            var result= await _admin.PostArticle(User, knowledge_BaseDto);
            return Ok(result);
        }

        [HttpPut("EditArticle/{id}")]
        public async Task<IActionResult> EditArticle(int id,[FromBody ]Knowledge_BaseDto knowledge_BaseDto)
        {
            var result=await _admin.EditArticle(User, id,knowledge_BaseDto);
            return Ok(result);
        }


        //SpareParts Corner

        //[HttpDelete("DeleteArticle/{id}")]
        //public async Task<IActionResult> DeleteArticle(int id)
        //{
        //    var result=await _admin.DeleteArticle(User, id);
        //    return Ok(result);
        //}

        //[HttpPost("AddSpareParts")]
        //public async Task<IActionResult> AddSpareParts(AddSpareParts addSpareParts)
        //{
        //    var result=await _admin.AddSpareParts(User, addSpareParts);
        //    return Ok(result);
        //}

        //[HttpPut("EditSpareParts/{id}")]
        //public async Task<IActionResult> EditSpareParts(AddSpareParts addSpareParts, int id)
        //{
        //    var result = _admin.EditSpareParts(User, addSpareParts, id);
        //    return Ok(result);
        //}
    }
}
