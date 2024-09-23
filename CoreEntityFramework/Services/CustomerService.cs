using CoreEntityFramework.Interfaces;
using CoreEntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreEntityFramework.Services
{
    public class CustomerService
    {

        private readonly AppDbContext _context;

        // dependency injection of database context
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // implementations of customer create, read, update, delete go here


    }
}
