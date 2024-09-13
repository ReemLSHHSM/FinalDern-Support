using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDern_Support.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SparePartsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SparePartsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("RemoveSpareParts/{partID}")]
        //public async Task<ActionResult<object>> RemoveSpareParts(int partID)
        //{
        //    var principal = User; // Get the ClaimsPrincipal from the current HttpContext

        //    if (principal == null)
        //    {
        //        return BadRequest(new ResultDto
        //        {
        //            Success = false,
        //            Message = "ClaimsPrincipal cannot be null.",
        //            StatusCode = 400 // Bad Request
        //        });
        //    }

        //    var user = await _userManager.GetUserAsync(principal);
        //    if (user == null)
        //    {
        //        return NotFound(new ResultDto
        //        {
        //            Success = false,
        //            Message = "User not found.",
        //            StatusCode = 404 // Not Found
        //        });
        //    }

        //    if (!await _userManager.IsInRoleAsync(user, "Admin"))
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "Access denied.",
        //            StatusCode = 403 // Forbidden
        //        };
        //    }

        //    var sparePart = _context.SpareParts.FirstOrDefault(x => x.ID == partID);
        //    if (sparePart == null)
        //    {
        //        return NotFound(new ResultDto
        //        {
        //            Success = false,
        //            Message = "SparePart Doesn't Exist",
        //            StatusCode = 404 // Not Found
        //        });
        //    }

        //    _context.SpareParts.Remove(sparePart);
        //    await _context.SaveChangesAsync();

        //    return Ok(new ResultDto
        //    {
        //        Success = true,
        //        Message = "SpareParts Removed Successfully",
        //        StatusCode = 200
        //    });
        //}

        [Authorize(Roles = "Admin")]
        [HttpPut("EditSpareParts/{partID}")]
        public async Task<ActionResult<object>> EditSpareParts(int partID, [FromBody] AddSpareParts addSpareParts)
        {
            var principal = User; // Get the ClaimsPrincipal from the current HttpContext

            if (principal == null)
            {
                return BadRequest(new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                });
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return NotFound(new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                });
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin") && !await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var sparePart = await _context.SpareParts.FirstOrDefaultAsync(x => x.ID == partID);
            if (sparePart == null)
            {
                return NotFound(new ResultDto
                {
                    Success = false,
                    Message = "SparePart Doesn't Exist",
                    StatusCode = 404 // Not Found
                });
            }

            // Check if the new name already exists in the database
            if (!string.IsNullOrEmpty(addSpareParts.Name) && addSpareParts.Name.ToLower().Replace(" ","") != "string")
            {
                var nameExists = await _context.SpareParts
                    .AnyAsync(sp => sp.Name.ToLower().Replace(" ", "") == addSpareParts.Name.ToLower().Replace(" ", "") && sp.ID != partID);

                if (nameExists)
                {
                    return Conflict(new ResultDto
                    {
                        Success = false,
                        Message = "A spare part with this name already exists.",
                        StatusCode = 409 // Conflict
                    });
                }

                sparePart.Name = addSpareParts.Name;
            }

            if (!string.IsNullOrEmpty(addSpareParts.Description) && addSpareParts.Description.ToLower() != "string")
            {
                sparePart.Description = addSpareParts.Description;
            }

            // Always update these fields
            sparePart.Price = addSpareParts.Price;
            sparePart.Quantity = addSpareParts.Quantity;

            await _context.SaveChangesAsync();

            return Ok(new ResultDto
            {
                Success = true,
                Message = "SpareParts Edited Successfully",
                StatusCode = 200
            });
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("AddSpareParts")]
        public async Task<ActionResult<object>> AddSpareParts([FromBody] AddSpareParts addSpareParts)
        {
            var principal = User; // Get the ClaimsPrincipal from the current HttpContext

            if (principal == null)
            {
                return BadRequest(new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                });
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return NotFound(new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                });
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            // Check if the name already exists in the database
            if (!string.IsNullOrEmpty(addSpareParts.Name) && addSpareParts.Name.ToLower().Replace(" ", "") != "string")
            {
                var nameExists = await _context.SpareParts
                    .AnyAsync(sp => sp.Name.ToLower().Replace(" ", "") == addSpareParts.Name.ToLower().Replace(" ", ""));

                if (nameExists)
                {
                    return Conflict(new ResultDto
                    {
                        Success = false,
                        Message = "A spare part with this name already exists.",
                        StatusCode = 409 // Conflict
                    });
                }
            }

            var sparePart = new SparePart
            {
                Name = addSpareParts.Name,
                Description = addSpareParts.Description,
                Quantity = addSpareParts.Quantity,
                Price = addSpareParts.Price
            };

            await _context.SpareParts.AddAsync(sparePart);
            await _context.SaveChangesAsync();

            return Ok(new ResultDto
            {
                Success = true,
                Message = "Parts Added Successfully",
                StatusCode = 200
            });
        }



        [Authorize(Roles = "Admin,Technician")]
        [HttpGet("GetAllSpareParts")]
        public async Task<ActionResult<object>> GetAllSpareParts()
        {
            var spareParts = await _context.SpareParts
                .Select(sp => new
                {
                    sp.ID,
                    sp.Name,
                    sp.Description,
                    sp.Quantity,
                    sp.Price
                })
                .ToListAsync();

            if (!spareParts.Any())
            {
                return (new ResultDto
                {
                    Success = false,
                    Message = "No spare parts found.",
                    StatusCode = 404 // Not Found
                });
            }

            return Ok(spareParts);
        }
    }
}