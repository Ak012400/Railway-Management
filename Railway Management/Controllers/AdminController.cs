using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Railway_Management.Models;

namespace Railway_Management.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDbContextFactory<ConnectionContext> _dbContextFactory;
        public AdminController(IDbContextFactory<ConnectionContext> dbContextFactory)
        {
                _dbContextFactory = dbContextFactory;
        }
        [HttpGet]
        [Route("/Admin/AdminIndex")]
        public IActionResult AdminIndex()
        {
            bool canconnect;
            using(var dx=_dbContextFactory.CreateDbContext())
            {
                canconnect = dx.AllCountries.Any();
            }
            Console.WriteLine(canconnect);
            Console.WriteLine("");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                using (var dx = _dbContextFactory.CreateDbContext())
                {
                    var data = await dx.AllCountries.Select(x=>x.countryname).ToListAsync();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = "There is Some Error Occured- " + ex.Message, status = 401 });
            }

        }
        [HttpPost]
        public async Task<IActionResult> GetAllStates( string selectedValue)
        {
            selectedValue = selectedValue.Trim();
           
            try
            {
                using (var db = _dbContextFactory.CreateDbContext())
                {
                    int value = db.AllCountries.Where(x => x.countryname.Equals(selectedValue)).Select(x => x.countryID).SingleOrDefault();

                    var data = await db.AllStates.Where(x => x.countryID == value).Select(y => y.statename).ToListAsync();
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = "Some Error Has Occured" + ex.Message, status = 401 });
            }
        }
    }
}
