using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Validations
{
    public class CustomerExistance
    {
        private readonly IDbContextFactory<ConnectionContext> _context;
        public CustomerExistance(IDbContextFactory<ConnectionContext> context)
        {
            this._context = context;
        }

        public async   Task<bool> IsUserExist(Customer customer)
        {
            try
            {
                Customer val = null;
                if (customer == null)
                {
                    return false;
                }
                using(var dx=_context.CreateDbContext())
                {
                    val = await dx.Customers.Where(x => x.Email == customer.Email || x.Contact == customer.Contact).FirstOrDefaultAsync();

                }

                if (val == null)
                {
                    return false;
                }
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }

        }
    }
}
