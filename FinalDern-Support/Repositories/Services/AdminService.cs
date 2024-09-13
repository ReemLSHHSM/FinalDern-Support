using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalDern_Support.Repositories.Services
{
    public class AdminService: IAdmin
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        //Inject IWT Service 
        private Jwt_TokenServices _jwtTokenService;
        public AdminService(UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> signInManager, Jwt_TokenServices jwtTokenService, AppDbContext context)
        {
            _userManager = manager;
            _signInManager = signInManager;
            _context = context;
            _jwtTokenService = jwtTokenService;
           

        }

        public async Task<object> AddTechnician(ClaimsPrincipal principal,TechnicianDto technicianDto, ModelStateDictionary modelState)

        {
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var useradmin = await _userManager.GetUserAsync(principal);
            if (useradmin == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
            }

            if (!await _userManager.IsInRoleAsync(useradmin, "Admin"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }


            if (technicianDto.Password != technicianDto.ConfirmPassword)
            {
                modelState.AddModelError(nameof(technicianDto.Password), "Passwords do not match");
                return null;
            }

            var user = new ApplicationUser()
            {
                UserName = technicianDto.Username,
                Email = technicianDto.Email,
                PhoneNumber = technicianDto.Phone,
                type="Technician"
            };

            var result = await _userManager.CreateAsync(user, technicianDto.Password);

            if (!result.Succeeded)
            {
          
                foreach (var error in result.Errors)
                {
                    var errorCode = error.Code.Contains("Password") ? nameof(technicianDto.Password) :
                                    error.Code.Contains("Email") ? nameof(technicianDto.Email) :
                                    error.Code.Contains("Username") ? nameof(technicianDto.Username) : "";
                    modelState.AddModelError(errorCode, error.Description);
                }

                return new ResultDto
                {
                    Success = false,
                    Message = "Creating Technician Faild",
                    StatusCode = 400
                };
            }

           
          

            await _userManager.AddToRoleAsync(user, "Technician");

            var technician = new Technician
            {
                UserID = user.Id,  // Ensure UserId is set
                Speciality = technicianDto.Specialization,
                IsAvailable = true,
            };

            _context.Technicians.Add(technician);  // Add Technician to context
            await _context.SaveChangesAsync();      // Save changes

            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromDays(7))
            };
        }

        public async Task<object> AcceptSupportRequest(ClaimsPrincipal principal,int RequestID)
        {
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var request=_context.Requests.FirstOrDefault(x=>x.ID== RequestID);

            if(request == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Request Not Sent Yet",
                    StatusCode = 404
                };
            }

            if (request.Status.ToLower() != "pending")
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Request NAlready Accepted",
                    StatusCode = 400
                };
            }

            request.Status = "Accepted";
            _context.SaveChanges();

            return  new ResultDto
            {
                Success = true,
                Message = "Request Accepted succesfully",
                StatusCode = 200
            };


        }

        //To Accept them
        public async Task<object> GetAllPendingRequests(ClaimsPrincipal principal){

            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var pendingRequests = _context.Requests
            .Where(r => r.Status.ToLower() == "pending")
            .ToList();

            if(pendingRequests.Count == 0)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "No Request Wasn't Submitted Yet",
                    StatusCode = 404
                };
            }

            return pendingRequests;
        }

        //To Create A quote For Them
        public async Task<object> GetAllAcceptedRequests(ClaimsPrincipal principal)
        {

            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var acceptedRequests = _context.Requests
            .Where(r => r.Status.ToLower() == "accepted")
            .ToList();

            if (acceptedRequests.Count == 0)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "No Request Was Accpted Yet",
                    StatusCode = 404
                };
            }

            return acceptedRequests;
        }

        public async Task<object> SubmitQuote(ClaimsPrincipal principal,SubmitQuote submitQuote,int RequestID)
        {
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var request = _context.Requests.FirstOrDefault(x => x.ID == RequestID);

            if (request == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Request Not Sent Yet.",
                    StatusCode = 404 
                };
            }

            if (request.Status.ToLower() != "accepted")
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Request Not Accepted Yet.",
                    StatusCode = 400 
                };
            }

            var quote = new Quote
            {
                RequestID = RequestID,
                StartAt = submitQuote.StartAt,
                EndAt = submitQuote.EndAt,
                Cost = submitQuote.Cost,
                Priority = submitQuote.Priority,
                Status="Pending"
            };

            _context.Quotes.Add(quote);
            _context.SaveChanges();
            
           return new ResultDto
            {
                Success = true,
                Message = "Quote Submited Successfully.",
                StatusCode = 200 // Forbidden
            };
        }

        public async Task<object> GetAllReports(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var reports = _context.Reports.ToList();

            if (reports.Count < 0) {

                return new ResultDto
                {
                    Success = false,
                    Message = "No Reports Were Submited.",
                    StatusCode = 400 
                };

            }

            return reports;
        }

        public async Task<object> GetReportsAnalyticsAsync(ClaimsPrincipal principal)
        {

            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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



            var reports = await _context.Reports
                .Include(r => r.Job)
                .ThenInclude(j => j.Quote)
                .ThenInclude(q => q.Request)
                .ToListAsync();

            if (reports == null || !reports.Any())
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "No Reports Were Found"
                };
            }

            // Total and Average Cost
            double totalCost = reports.Sum(r => r.TotalPrice);
            double averageCost = reports.Average(r => r.TotalPrice);

            // Total and Average Time
            //TimeSpan totalTime = TimeSpan.FromTicks(reports.Sum(r => r.TotalTime.Ticks));
            //double averageTimeTicks = reports.Average(r => r.TotalTime.Ticks);
            //TimeSpan averageTime = TimeSpan.FromTicks((long)averageTimeTicks);

            // Total and Average Number of Parts Used
            int totalNumberOfPartsUsed = reports.Sum(r => r.NumberOfPartsUsed);
            double averageNumberOfPartsUsed = reports.Average(r => r.NumberOfPartsUsed);

            // Get the most requested location
            var mostRequestedLocation = reports
                .GroupBy(r => r.Job.Quote.Request.Location) // Assuming City is in the Request table
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            // Fetch and calculate the average rating from Feedbacks table
            var feedbacks = await _context.Feedbacks.ToListAsync();
            double averageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;

            // Get common issues (assuming `Description` holds issues)
            var commonIssues = reports
                .GroupBy(r => r.Description)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(5) // Top 5 most common issues
                .ToList();

            // Jobs by Location
            var jobsByLocation = reports
                .GroupBy(r => r.Job.Quote.Request.Location)
                .ToDictionary(g => g.Key, g => g.Count());

            // Populate the DTO
            var analyticsDTO = new ReportsAnalyticsDTO
            {
                TotalCost = totalCost,
                AverageCost = averageCost,
                //TotalTime = totalTime,
                //AverageTime = averageTime,
                TotalNumberOfPartsUsed = totalNumberOfPartsUsed,
                AverageNumberOfPartsUsed = averageNumberOfPartsUsed,
                MostRequestedLocation = mostRequestedLocation,
                AverageRating = averageRating,
                CommonIssuesDealtWith = commonIssues,
                JobsByLocation = jobsByLocation
            };

            return analyticsDTO;
        }

        public async Task<object> PostArticle(ClaimsPrincipal principal,Knowledge_BaseDto knowledge_BaseDto)
        {
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var article = new KnowledgeBase
            {
                Title = knowledge_BaseDto.Title,
                Category = knowledge_BaseDto.Category,
                Content = knowledge_BaseDto.Content,
                Author = knowledge_BaseDto.Author,
                AdminID = 1
            };

            _context.KnowledgeBases.AddAsync(article);
            _context.SaveChanges();

            return new ResultDto
            {
                Success = true,
                Message = "Article Posted Successfully",
                StatusCode = 200
            };

        }
        public async Task<object> EditArticle(ClaimsPrincipal principal,int articleID,Knowledge_BaseDto knowledge_BaseDto){
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var article=_context.KnowledgeBases.FirstOrDefault(x=>x.ID==articleID);
            if (article == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Article Doesn't Exist",
                    StatusCode = 404
                };
            }

                if(knowledge_BaseDto.Content=="string" || string.IsNullOrEmpty(knowledge_BaseDto.Content))
                {
                    article.Content = article.Content;
                    _context.SaveChanges();
                }
                else
                {
                    article.Content = knowledge_BaseDto.Content;
                    _context.SaveChanges();
                }

                if (knowledge_BaseDto.Title == "string" || string.IsNullOrEmpty(knowledge_BaseDto.Title))
                {
                    article.Title = article.Title;
                    _context.SaveChanges();
                }
                else
                {
                    article.Title = knowledge_BaseDto.Title;
                    _context.SaveChanges();
                }

                if (knowledge_BaseDto.Category == "string" || string.IsNullOrEmpty(knowledge_BaseDto.Category))
                {
                    article.Category = article.Category;
                    _context.SaveChanges();
                }
                else
                {
                    article.Category = knowledge_BaseDto.Category;
                    _context.SaveChanges();
                }

                if (knowledge_BaseDto.Author == "string" || string.IsNullOrEmpty(knowledge_BaseDto.Author))
                {
                    article.Author = article.Author;
                    _context.SaveChanges();
                }
                else
                {
                    article.Author = knowledge_BaseDto.Author;
                    _context.SaveChanges();
                }

                return new ResultDto
                {
                    Success = true,
                    Message = "Article Updated Successfully",
                    StatusCode = 200
                };

            }
        
        public async Task<object> DeleteArticle(ClaimsPrincipal principal,int articleID)
        {
            if (principal == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
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

            var article = _context.KnowledgeBases.FirstOrDefault(x => x.ID == articleID);
            if (article == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Article Doesn't Exist",
                    StatusCode = 404
                };
            }

            _context.KnowledgeBases.Remove(article);
            _context.SaveChanges();

            return new ResultDto
            {
                Success = true,
                Message = "Article Deleted Successfully",
                StatusCode = 200
            };
        }


        //SpareParts Corner

        //public async Task<object> AddSpareParts(ClaimsPrincipal principal,AddSpareParts addSpareParts)

        //{
        //    if (principal == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "ClaimsPrincipal cannot be null.",
        //            StatusCode = 400 // Bad Request
        //        };
        //    }

        //    var user = await _userManager.GetUserAsync(principal);
        //    if (user == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "User not found.",
        //            StatusCode = 404 // Not Found
        //        };
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

        //    var spareparts = new SparePart
        //    {
        //        Name = addSpareParts.Name,
        //        Description = addSpareParts.Description,
        //        Quantity = addSpareParts.Quantity,
        //        Price = addSpareParts.Price
        //    };

        //    _context.SpareParts.AddAsync(spareparts);
        //    _context.SaveChanges();

        //    return new ResultDto
        //    {
        //        Success=true,
        //        Message="Parts Added Successfully",
        //        StatusCode=200
        //    };
        //}

        //public async Task<object> EditSpareParts(ClaimsPrincipal principal,AddSpareParts addSpareParts,int partID)
        //{
        //    if (principal == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "ClaimsPrincipal cannot be null.",
        //            StatusCode = 400 // Bad Request
        //        };
        //    }

        //    var user = await _userManager.GetUserAsync(principal);
        //    if (user == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "User not found.",
        //            StatusCode = 404 // Not Found
        //        };
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
        //    var spareParts=_context.SpareParts.FirstOrDefault(x=>x.ID==partID);
        //    if (spareParts == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "SparePart Doesn't Exist",
        //            StatusCode=200
        //        };
        //    }

        //    if(addSpareParts.Name.ToLower()=="string" || string.IsNullOrEmpty(addSpareParts.Name))
        //    {
        //        spareParts.Name = spareParts.Name;
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        spareParts.Name=addSpareParts.Name;
        //        _context.SaveChanges();
        //    }

        //    if (addSpareParts.Description.ToLower() == "string" || string.IsNullOrEmpty(addSpareParts.Description))
        //    {
        //        spareParts.Description = spareParts.Description;
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        spareParts.Description = addSpareParts.Description;
        //        _context.SaveChanges();
        //    }

          
        //        spareParts.Price = addSpareParts.Price;
        //        _context.SaveChanges();
            
           
        //        spareParts.Quantity = addSpareParts.Quantity;
        //        _context.SaveChanges();
            

        //    return new ResultDto
        //    {
        //        Success = true,
        //        Message = "SpareParts Edited Successfully",
        //        StatusCode = 200
        //    };

        //}
        //public async Task<object> RemoveSpareParts(ClaimsPrincipal principal,int partID)
        //{
        //    if (principal == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "ClaimsPrincipal cannot be null.",
        //            StatusCode = 400 // Bad Request
        //        };
        //    }

        //    var user = await _userManager.GetUserAsync(principal);
        //    if (user == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "User not found.",
        //            StatusCode = 404 // Not Found
        //        };
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
        //    var spareParts = _context.SpareParts.FirstOrDefault(x => x.ID == partID);
        //    if (spareParts == null)
        //    {
        //        return new ResultDto
        //        {
        //            Success = false,
        //            Message = "SparePart Doesn't Exist",
        //            StatusCode = 200
        //        };
        //    }

        //    _context.SpareParts.Remove(spareParts);
        //    _context.SaveChanges();

        //    return new ResultDto
        //    {
        //        Success = true,
        //        Message = "SpareParts Removed Successfully",
        //        StatusCode = 200
        //    };
      //  }
    }
}
