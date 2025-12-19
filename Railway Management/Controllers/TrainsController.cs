
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Railway_Management.IServices;
using Railway_Management.Models;
using System;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Controllers
{
    public class TrainsController : Controller
    {
        private readonly IDbContextFactory<ConnectionContext> connectionContext;
        private readonly IRailwayApis _apiService;
        public TrainsController(IDbContextFactory<ConnectionContext> context,IRailwayApis railwayApis)
        {
            connectionContext = context;
            _apiService = railwayApis;
        }
        [Authorize]
        public IActionResult TrainsBooking()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainDetails(string searchParam)
        {
            TrainDetailsApiResponse? trainResult = new TrainDetailsApiResponse();
            using (var dx = connectionContext.CreateDbContext())
            {
                 trainResult= await _apiService.GetTrainsAsync(searchParam);

            }
            if (trainResult?.Status==true)
            {
               
                return Ok(trainResult);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> LoadSourceStations(string searchTerm="")
        {
            List<AllStations> stations = new List<AllStations>();
            try
            {
                using (var dx = connectionContext.CreateDbContext())
                {
                     
                    stations = await dx.AllStations1.Where(t => string.IsNullOrEmpty(searchTerm) || t.station_name.Contains(searchTerm) || t.station_code.Contains(searchTerm)).ToListAsync();
                  
                }
                Console.Write("no");
                if (stations.Any())
                {

                    return Ok(stations);
                }
                return BadRequest();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> LoadDestinationStations(string searchTerm = "")
        {
            List<AllStations> stations = new List<AllStations>();
            using (var dx = connectionContext.CreateDbContext())
            {
                stations = await dx.AllStations1.Where(t => string.IsNullOrEmpty(searchTerm) || t.station_name.Contains(searchTerm) || t.station_code.Contains(searchTerm)).ToListAsync();

            }
            if (stations.Any())
            {

                return Ok(stations);
            }
            return BadRequest();

        }


    }
}

