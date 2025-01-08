using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;
using System.Data;

namespace Railway_Management.Procesures
{
    public class UpdateCustomerImage
    {
        private readonly IDbContextFactory<ConnectionContext> _connectionContextFactory;
        public UpdateCustomerImage(IDbContextFactory<ConnectionContext> contextFactory)
        {
            _connectionContextFactory = contextFactory;
            
        }

        public async Task<int> UpdateCustomerImageAsync(int id, string newValue)
        {
            try
            {
                var resultParam = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var customerIdParam = new SqlParameter("@CustomerID", SqlDbType.Int) { Value = id };
                var imageUrlParam = new SqlParameter("@ImageURL", SqlDbType.VarChar, 255) { Value = newValue };
                int returnValue = 0;
                using (var dx = _connectionContextFactory.CreateDbContext())
                {

                    var data4 = await dx.CustomerPersonalDetails.Where(x => x.CustomerID == id).Select(x => x.detailsID).SingleOrDefaultAsync();
                    if(data4!=0)
                    {
                        await dx.Database.ExecuteSqlRawAsync(
                       "EXEC UpdateCustomerImage @CustomerID, @ImageURL",
                       customerIdParam,
                       imageUrlParam);
                        returnValue = 1;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                   

                }
                return returnValue;
            }catch (Exception ex)
            {
                return -1;
            }

               
        }
    }
}
