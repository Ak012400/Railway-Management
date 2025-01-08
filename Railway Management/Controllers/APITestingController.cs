using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Controllers
{
    public class APITestingController : Controller
    {
        private readonly IDbContextFactory<ConnectionContext> _connectionfactory;
        public APITestingController(IDbContextFactory<ConnectionContext> dbContextFactory)
        {
            this._connectionfactory = dbContextFactory;
        }

        [HttpGet]
        public  IActionResult Testing()
        {
           return View();
        }

        [HttpGet]
        [Route("/APITesting/GetData")]
        public async Task<IActionResult> GetData()
        {
            using (var dx = _connectionfactory.CreateDbContext())
            {
                var users = await dx.Customers.ToListAsync<Customer>();
                return Ok(users);
            }

        }

        [HttpPost]
        [Route("/APITesting/PostData")]
        public async Task<IActionResult> PostData(string value)
        {
            return Json(new { message="Got Your Data",status=200 });
        }
    }
}
