using FinalDern_Support.Models;

namespace FinalDern_Support.Repositories.Interfaces
{
    public interface ICustomer
    {
        Task<Customer> AddCustomer(Customer customer);
    }
}
