using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace FinalDern_Support.Repositories.Services
{
    public class TechnicianService : ITechnician
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TechnicianService(AppDbContext Context, UserManager<ApplicationUser> userManager)
        {
            _context = Context;
            _userManager = userManager;
        }



        //Get accepted quotes to choose a job
        public async Task<object> GetAllNonTakenJobs(ClaimsPrincipal principal)
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

            try
            {


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

                if (!await _userManager.IsInRoleAsync(user, "Technician"))
                {
                    return new ResultDto
                    {
                        Success = false,
                        Message = "Access denied.",
                        StatusCode = 403 // Forbidden
                    };
                }
            }

            catch
            {
                
            }
            var jobs = _context.Quotes
           .Where(x => x.Status.ToLower() == "accepted")
             .Select(x => new GetPendingQuotesDto
             {
                 ID = x.ID,
                 Cost = x.Cost,
                 StartAt = x.StartAt,
                 EndAt = x.EndAt,
                 Status = x.Status
             })
                 .ToList();


            if (jobs == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "No Quotes Were Accepted",
                    StatusCode = 200
                };
            }

            return jobs;


        }



        public async Task<object> TakeJob(ClaimsPrincipal principal, int quoteID)
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

            if (!await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var quote = _context.Quotes.FirstOrDefault(x => x.ID == quoteID);

            if (quote == null)
            {

                return new ResultDto
                {
                    Success = false,
                    Message = "Quote Wasn't Submitted yet",
                    StatusCode = 404
                };

            }

            if (quote.Status.ToLower() != "accepted")
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Quote Wasn't Accepted yet",
                    StatusCode = 400
                };
            }

            if (quote.IsTaken)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job Has Been Taken",
                    StatusCode = 400
                };
            }



            var tech = _context.Technicians.FirstOrDefault(x => x.UserID == user.Id);

            if (!tech.IsAvailable)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "You Are Not Available",
                    StatusCode = 400
                };
            }


            var job = new Job
            {
                QuoteID = quoteID,
                IsComplete = false,
                TechID = tech.ID,
                
            };

            _context.Jobs.Add(job);
            _context.SaveChanges();
            tech.IsAvailable = false;
            _context.SaveChanges();
            quote.IsTaken = true;
            _context.SaveChanges();

            return new ResultDto
            {
                Success = true,
                Message = "Job Taken Successfully",
                StatusCode = 200
            };
        }

        public async Task<object> GetAllFeedBacks(ClaimsPrincipal principal)
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

            if (!await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }
            var tech = _context.Technicians.FirstOrDefault(x => x.UserID == user.Id);

            var feedbacks = _context.Feedbacks
              .Where(x => x.Job.TechID == tech.ID)
             .Select(f => new GetJobFeedBackDto
             {
                 JobId = f.Job.ID,
                 Title = f.Title,
                 Comment = f.Comment,
                 Rating = f.Rating
             })
               .ToList();

            if (feedbacks != null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "You don't Have Any FeedBacks",
                    StatusCode = 400
                };
            }

            return feedbacks;
        }

        public async Task<object> GetJobsFeedBack(ClaimsPrincipal principal, int jobID)
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

            if (!await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }
            var tech = _context.Technicians.FirstOrDefault(x => x.UserID == user.Id);
            var job = _context.Jobs.FirstOrDefault(x => x.ID == jobID);

            if (job == null)
            {

                return new ResultDto
                {
                    Success = false,
                    Message = "Job Doesn't Exist",
                    StatusCode = 404 // Forbidden
                };
            }

            if (job.IsComplete == false)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job Is Not Complete Yet",
                    StatusCode = 400 // Forbidden
                };
            }

            var feedbacks = _context.Feedbacks.FirstOrDefault(x => x.JobID == job.ID);
            if(feedbacks == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "No FeedBack Was Given",
                    StatusCode = 404 // Forbidden
                };
            }

            var feedback = new PostFeedBackrequest
            {
                Title = feedbacks.Title,
                Comment = feedbacks.Comment,
                Rating = feedbacks.Rating,
            };

            return feedback;
        }

        public async Task<object> TakeSpareParts(ClaimsPrincipal principal,int partsID,int jobID)
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

            if (!await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var part = _context.SpareParts.FirstOrDefault(x => x.ID == partsID);
            if(part == null || part.Quantity<1)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Part Not In Stock.",
                    StatusCode = 404 // Forbidden
                };
            }
            var tech = _context.Technicians.FirstOrDefault(x => x.UserID == user.Id);
            var techJob = _context.Jobs.FirstOrDefault(x => x.TechID == tech.ID && !x.IsComplete);

            part.Quantity -= 1;
            _context.SaveChanges();

            var jobparts = new JobSpareParts
            {
                JobID = jobID,
                SparePartID = part.ID,
            };

            _context.JobSpareParts.Add(jobparts);
            _context.SaveChanges();

            return new ResultDto
            {
                Success = true,
                Message = "SparePart Withdrawn Successfully",
                StatusCode = 200
            };
        }

        public async Task<object> PostReport(ClaimsPrincipal principal,int jobID,ReportContent reportContent)
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

            if (!await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var job=_context.Jobs.FirstOrDefault(x=>x.ID==jobID);
            if(job == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job Not Taken Yet.",
                    StatusCode = 404 // Forbidden
                };
            }

            if (!job.IsComplete)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job Not Done Yet.",
                    StatusCode = 400 // Forbidden
                };
            }

            var report = new Report
            {
                Title = reportContent.Title,
                Description = reportContent.Content,
                TotalTime = reportContent.totalTime,
                NumberOfPartsUsed = _context.JobSpareParts.Where(x => x.JobID == jobID).Count(),
                TotalPrice = _context.JobSpareParts
                 .Where(jsp => jsp.JobID == jobID)
                 .GroupBy(jsp => jsp.SparePartID)
                 .Sum(g => g.Count() * g.FirstOrDefault().SparePart.Price+20)
            };

            _context.Reports.Add(report);
            _context.SaveChanges();

            return new ResultDto
            {
                Success = true,
                Message = "Report Posted Succesfully.",
                StatusCode = 200 
            };
        }

        public async Task<object> FinishJob(ClaimsPrincipal principal,int jobID)
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

            if (!await _userManager.IsInRoleAsync(user, "Technician"))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Access denied.",
                    StatusCode = 403 // Forbidden
                };
            }

            var job=_context.Jobs.FirstOrDefault(x=>x.ID==jobID);

            if(job == null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job Not Taken Yet.",
                    StatusCode = 404 // Bad Request
                };
            }

            if (job.IsComplete)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "Job Already Complete.",
                    StatusCode = 400 // Bad Request
                };
            }

            job.IsComplete = true;
            _context.SaveChanges();
            var tech = _context.Technicians.FirstOrDefault(x => x.UserID == user.Id);
            tech.IsAvailable = true;
            _context.SaveChanges();

        
                return new ResultDto
                {
                    Success = true,
                    Message = "Job Finished Successfully.",
                    StatusCode = 200 
               };
            
        }
    }
}
