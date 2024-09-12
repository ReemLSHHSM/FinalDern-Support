using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinalDern_Support.Repositories.Interfaces;

namespace FinalDern_Support.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController:ControllerBase
    {
        private readonly IAdmin _admin;
        public AdminController(IAdmin admin)
        {
            _admin = admin;
        }
       // [Authorize(Roles = "Customer")]
        [HttpPost("AddTechnician")]
        public async Task<IActionResult> AddTechnician([FromBody]TechnicianDto technicianDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _admin.AddTechnician(technicianDto, ModelState);

            if (user == null)
            {
                return BadRequest(new { Message = "Adding Technician failed. Please check your details and try again." });
            }

            return CreatedAtAction(nameof(AddTechnician), new { id = user.Id }, user);
        }
    }
}
