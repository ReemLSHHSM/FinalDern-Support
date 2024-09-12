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
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("ALLArticles")]
        public async Task<ActionResult<object>> GetAllKnowledgeBaseArticlesAsync()
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

        [HttpGet("SearchByTitle")]
        public async Task<ActionResult<object>> SearchByTitleAsync([FromQuery] string title)
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

            // Normalize the search title by removing spaces and converting to lower case
            var normalizedTitle = title.Replace(" ", "").ToLower();

            // Retrieve all articles that match the normalized title
            var articles = await _context.KnowledgeBases
                .Where(x => x.Title.ToLower().Replace(" ", "").Contains(normalizedTitle))
                .Select(x => new Knowledge_BaseDto
                {
                    Title = x.Title,
                    Content = x.Content,
                    Category = x.Category,
                    Author = _context.Users.Include(a => a.Admin)
                        .Where(u => u.Admin.ID == x.AdminID)
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



        [HttpGet("SearchByCategory")]
        public async Task<ActionResult<object>> SearchByCategoryAsync([FromQuery] string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return BadRequest(new ResultDto
                {
                    Success = false,
                    Message = "Category parameter cannot be null or empty.",
                    StatusCode = 400 // Bad Request
                });
            }

            // Normalize the search category by removing spaces and converting to lower case
            var normalizedCategory = category.Replace(" ", "").ToLower();

            var articles = await _context.KnowledgeBases
                .Where(kb => kb.Category.Replace(" ", "").ToLower()==normalizedCategory)
                .Select(kb => new Knowledge_BaseDto
                {
                    Title = kb.Title,
                    Content = kb.Content,
                    Category = kb.Category,
                    Author = kb.Author
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
