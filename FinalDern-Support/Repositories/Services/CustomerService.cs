using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

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

    }
}
