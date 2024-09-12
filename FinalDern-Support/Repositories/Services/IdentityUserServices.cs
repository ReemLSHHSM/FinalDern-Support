using FinalDern_Support.Data;
using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using FinalDern_Support.Models;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinalDern_Support.Repositories.Services
{
    public class IdentityUserServices : IUser
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICustomer _customer;
        private readonly AppDbContext _context;
        //Inject IWT Service 
        private Jwt_TokenServices _jwtTokenService;
        public IdentityUserServices(UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> signInManager, Jwt_TokenServices jwtTokenService, AppDbContext context, ICustomer customer)
        {
            _userManager = manager;
            _signInManager = signInManager;
            _context = context;
            _jwtTokenService = jwtTokenService;
            _customer = customer;

        }
        public async Task<UserDto> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            bool passValidation = await _userManager.CheckPasswordAsync(user, password);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(username);
            }

            if (user == null || !passValidation)
            {
                throw new InvalidOperationException(" Invalid Password or UserName/Email ");
            }
            if (passValidation)
            {
                return new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = await _jwtTokenService.GenerateToken(user, System.TimeSpan.FromDays(60))
                };
            }

            return null;
        }

        public async Task<UserDto> Register(RegisterDto registerDto, ModelStateDictionary modelState)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser == null)
            {

                if (registerDto.Password != registerDto.ConfirmPassword)
                {

                    throw new Exception("Passwords do not match");
                }

                var user = new ApplicationUser()
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone,
                    type= "Customer",
             

                };



                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {

                    // Assign the "Customer" role to the user
                    await _userManager.AddToRoleAsync(user, "Customer");

                    var customer = new Customer
                    {
                        UserID = user.Id,
                        IsBusiness=registerDto.ISBussiness
                      

                    };

                    // Add the new customer to the database
                    await _customer.AddCustomer(customer);

                    return new UserDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Token = await _jwtTokenService.GenerateToken(user, System.TimeSpan.FromDays(7))
                    };
                }

                // Handle errors returned by the user manager
                foreach (var error in result.Errors)
                {
                    var errorCode = error.Code.Contains("Password") ? nameof(registerDto) :
                        error.Code.Contains("Email") ? nameof(registerDto) :
                        error.Code.Contains("Username") ? nameof(registerDto) : "";

                    modelState.AddModelError(errorCode, error.Description);
                }
            }
            else if (existingUser != null)
            {

                throw new Exception("Emaill already exist!");
            }

            return null;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
