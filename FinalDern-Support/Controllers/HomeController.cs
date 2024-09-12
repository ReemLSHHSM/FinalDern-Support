using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.ResponseDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDern_Support.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("ALLArticles")]
        public async Task<IActionResult> GetAllKnowledgeBaseArticlesAsync()
        {
            // Retrieve all articles with associated admin and user details
            var articles = await _context.KnowledgeBases
                .AsNoTracking()
                .Select(article => new Knowledge_BaseDto
                {
                    Title = article.Title,
                    Category = article.Category,
                    Content = article.Content,
                    Author = _context.Admins
                        .Where(a => a.ID == article.AdminID)
                        .Select(a => _context.Users.FirstOrDefault(u => u.Id == a.UserID).UserName)
                        .FirstOrDefault()
                }).ToListAsync();

            if (articles == null || !articles.Any())
            {
                return NotFound(new ResultDto
                {
                    Success = false,
                    Message = "No Articles Found",
                    StatusCode = 404
                });
            }

            return Ok(articles);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchByTitleAsync([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest(new ResultDto
                {
                    Success = false,
                    Message = "Title parameter cannot be null or empty.",
                    StatusCode = 400 // Bad Request
                });
            }

            var articles = await _context.KnowledgeBases
                .Where(kb => kb.Title.Contains(title))
                .Select(kb => new Knowledge_BaseDto
                {
                    Title = kb.Title,
                    Content = kb.Content,
                    Category = kb.Category,
                    Author = _context.Users.Include(a=>a.Admin)
                        .Where(u => u.Admin.ID == kb.AdminID)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!articles.Any())
            {
                return NotFound(new ResultDto
                {
                    Success = false,
                    Message = "No articles found with the specified title.",
                    StatusCode = 404 // Not Found
                });
            }

            return Ok(articles);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchByCategoryAsync([FromQuery] string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return BadRequest(new ResultDto
                {
                    Success = false,
                    Message = "category parameter cannot be null or empty.",
                    StatusCode = 400 // Bad Request
                });
            }

            var articles = await _context.KnowledgeBases
                .Where(kb => kb.Category.Contains(category))
                .Select(kb => new Knowledge_BaseDto
                {
                    Title = kb.Title,  
                    Content = kb.Content,
                    Category = kb.Category,
                    Author = _context.Users.Include(a => a.Admin)
                        .Where(u => u.Admin.ID == kb.AdminID)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!articles.Any())
            {
                return NotFound(new ResultDto
                {
                    Success = false,
                    Message = "No articles found with the specified category.",
                    StatusCode = 404 // Not Found
                });
            }

            return Ok(articles);
        }


    }
}