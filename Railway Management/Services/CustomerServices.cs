using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;

namespace Railway_Management.Services
{
    public class CustomerServices : ICustomers
    {
        private readonly IDbContextFactory<ConnectionContext> _dbContextFactory;
        public CustomerServices(IDbContextFactory<ConnectionContext> db)
        {
            _dbContextFactory = db;
        }
        bool ICustomers.IsUseExist(string email)
        {
            try
            {
                using (var dx = _dbContextFactory.CreateDbContext())
                {
                    return dx.Customers.Any(x => x.Email == email);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
