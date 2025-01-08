
using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;

namespace Railway_Management.Services
{
    public class PaymentGateWay : IPaymentGetWay
    {
        private readonly IDbContextFactory<ConnectionContext> _dbContextFactory;
        public PaymentGateWay(IDbContextFactory<ConnectionContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        bool IPaymentGetWay.PaymentHasDone(int customerID)
        {
           return false;
        }
    }
}
