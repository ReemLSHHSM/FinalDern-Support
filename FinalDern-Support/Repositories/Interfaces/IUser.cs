using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinalDern_Support.Repositories.Interfaces
{
    public interface IUser
    {
        public Task<UserDto> Register(RegisterDto registerDto, ModelStateDictionary modelState);

        public Task<UserDto> Login(string username, string password);

        Task Logout();
    }
}
