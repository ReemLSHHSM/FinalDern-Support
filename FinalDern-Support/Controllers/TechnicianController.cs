using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalDern_Support.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Technician")]
    public class TechnicianController : ControllerBase
    {
        private readonly ITechnician _technician;

        public TechnicianController(ITechnician technician)
        {
            _technician = technician;
        }


        [HttpGet("GetAllNonTakenJobs")]
        public async Task<ActionResult<object>> GetAllNonTakenJobs()
        {
            var result = await _technician.GetAllNonTakenJobs(User);
            return Ok(result);


        }

        [HttpGet("GetAllFeedBacks")]
        public async Task<ActionResult<object>> GetAllFeedBacks()
        {
            var result = await _technician.GetAllFeedBacks(User);
            return Ok(result);
        }

        [HttpGet("GetJobsFeedBack/{id}")]
        public async Task<ActionResult<object>> GetJobsFeedBack(int id)
        {
            var result = await _technician.GetJobsFeedBack(User, id);
            return Ok(result);
        }

        [HttpPut("TakeSpareParts/{SparePartid}/{jobid}")]
        public async Task<IActionResult> TakeSpareParts(int SparePartid, int jobid)
        {
            var result = await _technician.TakeSpareParts(User, SparePartid, jobid);
            return Ok(result);
        }

        [HttpPost("ProduceJobReport/{id}")]
        public async Task<IActionResult> ProduceJobReport(int id, ReportContent content)
        {
            var result = await _technician.PostReport(User, id, content);
            return Ok(result);
        }

        [HttpPost("TakeJob/{id}")]
        public async Task<IActionResult> TakeJob(int id)
        {
            var result=await _technician.TakeJob(User, id);
            return Ok(result);
        }
        [HttpPut("FinishJob/{id}")]
        public async Task<IActionResult> FinishJob(int id)
        {
            var result=await _technician.FinishJob(User, id);
            return Ok(result);
        }
    }
}
