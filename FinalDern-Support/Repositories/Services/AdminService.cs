using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public async Task<UserDto> AddTechnician(TechnicianDto technicianDto, ModelStateDictionary modelState)
        {
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

            if (result.Succeeded)
            {
                // Assign the Technician role to the user
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

            // Add model state errors if creation fails
            foreach (var error in result.Errors)
            {
                var errorCode = error.Code.Contains("Password") ? nameof(technicianDto.Password) :
                                error.Code.Contains("Email") ? nameof(technicianDto.Email) :
                                error.Code.Contains("Username") ? nameof(technicianDto.Username) : "";
                modelState.AddModelError(errorCode, error.Description);
            }

            return null;
        }
    }
}
