using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalDern_Support.Repositories.Services
{
    public class CustomerService : ICustomer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        //Get All Pending Quotes
        public async Task<object> GetAllQuotes(ClaimsPrincipal principal)
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

            if (!await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserID == user.Id);

            if (customer == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Customer not found.",
                    StatusCode = 404 // Not Found
                };
            }

            var quotes = await _context.Quotes
                .Include(q => q.Request) // Include the related Request entity
                .Where(q => q.Request.CustomerID == customer.ID
                            && q.Request.Status.ToLower() == "accepted"
                            && q.Status.ToLower() == "pending")
                .Select(q => new GetPendingQuotesDto
                {
                    ID = q.ID,
                    Cost = q.Cost,
                    StartAt = q.StartAt,
                    EndAt = q.EndAt,
                    Status = q.Status
                })
                .ToListAsync();

            return quotes;
        }



        //Submit A Request
        public async Task<object> SubmitSupportRequest(ClaimsPrincipal principal, requestSupportRequestDto requestSupportRequestDto)
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

            if (!await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied",
                    StatusCode = 403
                };
            }

            var cus = _context.Customers.FirstOrDefault(x => x.UserID == user.Id);

            if (requestSupportRequestDto == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Requset is empty",
                    StatusCode = 400
                };
            }

            if (requestSupportRequestDto.Quantity < 1)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Number of devices must be at least 1",
                    StatusCode = 400
                };
            }
            var request = new Request
            {
                Title = requestSupportRequestDto.Title,
                Description = requestSupportRequestDto.Description,
                Quantity = requestSupportRequestDto.Quantity,
                Location = requestSupportRequestDto.Location,
                Status = "Pending",
                CustomerID=cus.ID,
                IsTaken = false


            };
            
            var respons = new responseSupportRequestDto
            {
                Title = requestSupportRequestDto.Title,
                Description = requestSupportRequestDto.Description,
                Quantity = requestSupportRequestDto.Quantity,
                Location = requestSupportRequestDto.Location,
                Status = "Pending"
            };

          await _context.Requests.AddAsync(request);
           await _context.SaveChangesAsync();

            return new ResultDto
            {
                Success = true,
                Message = "Support Request Submited successfully",
                StatusCode = 200
            }; 


        }
        //Accept or Reject
        public async Task<object> UpdateQuoteStatusAsync(ClaimsPrincipal principal, string newStatus, int quoteID)
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

            if (!await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }


            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserID == user.Id);
            if (customer == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Customer not found.",
                    StatusCode = 404 // Not Found
                };
            }

            // Find the quote
            var quote = await _context.Quotes.FirstOrDefaultAsync(x => x.ID == quoteID);
            if (quote == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Quote not found or does not belong to this customer.",
                    StatusCode = 404 // Not Found
                };
            }

            // Now fetch the associated request
            var request = await _context.Requests.FirstOrDefaultAsync(x => x.ID == quote.RequestID);
            if (request == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Associated request not found.",
                    StatusCode = 404 // Not Found
                };
            }

            // Check if the quote is pending and the request is accepted
            if (quote.Status.ToLower() != "pending" || request.Status.ToLower() != "accepted")
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Only quotes with 'Pending' status and requests with 'Accepted' status can be updated.",
                    StatusCode = 400 // Bad Request
                };
            }

            // Ensure the new status is either 'accepted' or 'rejected'
            if (newStatus.ToLower() != "accepted" && newStatus.ToLower() != "rejected")
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Invalid status. Must be either 'Accepted' or 'Rejected'.",
                    StatusCode = 400 // Bad Request
                };
            }

            // If rejected, remove both the quote and the request
            if (newStatus.ToLower() == "rejected")
            {
                request.Status = "Rejected";
                await _context.SaveChangesAsync();
                quote.Status = "Rejected";
                await _context.SaveChangesAsync();
                

                return new ResultDto
                {
                    Success = true,
                    Message = "Quote and associated request have been rejected and deleted.",
                    StatusCode = 200 // OK
                };
            }

            // Otherwise, update the quote status
            quote.Status = "Accepted";
            await _context.SaveChangesAsync();

            return new ResultDto
            {
                Success = true,
                Message = "Quote Accepted",
                StatusCode = 200 // OK
            };
        }
        //To give FeedBack
        public async Task<object> GetCompletedJobsAsync(ClaimsPrincipal claims)
        {
            if (claims == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "ClaimsPrincipal cannot be null.",
                    StatusCode = 400 // Bad Request
                };
            }

            // Get user asynchronously
            var user = await _userManager.GetUserAsync(claims);
            if (user == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404 // Not Found
                };
            }

            // Ensure the user is a "Customer"
            if (!await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserID == user.Id);
            if (customer == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Customer not found.",
                    StatusCode = 404 // Not Found
                };
            }

           
            var completedJobs = await _context.Jobs
                .Where(j => j.IsComplete
                            && j.Quote.Request.CustomerID == customer.ID)
                .Include(j => j.Quote) 
                .ThenInclude(q => q.Request) 
                .Select(j => new CompletedJobDto
                {
                    ID = j.ID,
                    Title = j.Quote.Request.Title,
                    Description = j.Quote.Request.Description,
                    IsComplete = j.IsComplete
                })
                .ToListAsync(); 

          
            if (completedJobs == null || !completedJobs.Any())
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "No completed jobs found.",
                    StatusCode = 404 // Not Found
                };
            }

            return completedJobs;
        }


        public async Task<object>PostFeedBack(ClaimsPrincipal principal, int JobID, PostFeedBackrequest postFeedBackrequest)
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

            if (!await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserID == user.Id);
            if (customer == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Customer not found.",
                    StatusCode = 404 // Not Found
                };
            }
            var job = _context.Jobs.FirstOrDefault(j => j.ID == JobID);
            if (job == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job not found.",
                    StatusCode = 404 // Not Found
                };
            }

            if (!job.IsComplete)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job is not Completed yet.",
                    StatusCode = 400 // Not Found
                };
            }

            _context.Feedbacks.AddAsync(new Feedback
            {
                CustomerID=customer.ID,
                JobID=JobID,
                Title= postFeedBackrequest.Title,
                Comment=postFeedBackrequest.Comment,
                Rating=postFeedBackrequest.Rating
            });

            _context.SaveChanges();

            return new ResultDto
            {
                Success = true,
                Message = "FeedBack Posted Successfully",
                StatusCode=200
            };
        }


        

    }
}
