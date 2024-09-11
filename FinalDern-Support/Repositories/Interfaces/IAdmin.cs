using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models.Dto.ResponseDtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinalDern_Support.Repositories.Interfaces
{
    public interface IAdmin
    {
        public Task<UserDto> AddTechnician(TechnicianDto technicianDto, ModelStateDictionary modelState);
    }
}
