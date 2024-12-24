using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Railway_Management.Models;
using static Railway_Management.Models.AllDataDetails;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using Railway_Management.Admin;
using Microsoft.Extensions.Options;

namespace Railway_Management.Controllers
{
    public class MainController : Controller
    {
        private readonly IDbContextFactory<ConnectionContext> _connectionContext;

        public MainController(IDbContextFactory<ConnectionContext> context)
        {
            _connectionContext = context;

        }
        public async Task<IActionResult> MainPage()
        {
            List<Station> stations = new List<Station>();

            using(var dx = _connectionContext.CreateDbContext())
            {
                stations =await dx.Stations.ToListAsync<Station>();
            }
           


            TempData["Error"] = "Not found any error";

           return View(stations);
            
        }
       
        public IActionResult AdminDataInsertion()
        {
            return View();  

        }
        [HttpPost]
        [RequestSizeLimit(804857600)]
        public async Task<IActionResult> JsonParser([FromForm]IFormFile file,string target)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    TempData["Message"] = "File Must Needed";
                    return RedirectToAction("AdminDataInsertion", "Main");
                }

                if (Path.GetExtension(file.FileName) == ".json" || Path.GetExtension(file.FileName) == ".xlsx" || Path.GetExtension(file.FileName) == ".xls")
                {
                    if (file == null || file.Length == 0)
                    {
                        TempData["Message"] = "File Must Needed only Json";
                        return RedirectToAction("AdminDataInsertion", "Main");
                    }



                    int updatedResult = 0;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    List<AllStations> alls = new List<AllStations>();   
                    StationJsonParsor stationJsonParsor = new StationJsonParsor();
                    GeoJsonReader geoJson=new GeoJsonReader(_connectionContext);
                    int val = 0;
                    if (target == "train")
                    {
                     ExtractionJsonResult data=   geoJson.ReadGeoJson(filePath);

                        if(data == null)
                        {
                            TempData["success"] = "Data while Train data reading got null";
                            return RedirectToAction("AdminDataInsertion", "Main");
                        }
                        
                        using(var dx= _connectionContext.CreateDbContext())
                        {
                            await dx.Train_Details.AddRangeAsync(data.trainDetails);
                          updatedResult=  await dx.SaveChangesAsync();
                        }
                        if(updatedResult > 0)
                        {
                            updatedResult = 0;
                            Console.Write("now saving logitue value------------------------------------------------\n");
                            using(var dx= _connectionContext.CreateDbContext())
                            {
                                await dx.Trains_Running_Coordinates.AddRangeAsync(data.coordinates);
                               updatedResult= await dx.SaveChangesAsync();
                            }
                        }
                       if (updatedResult > 0)
                        {
                            TempData["success"] = "Train Details data has uploaded to the data base successfully..";
                            return RedirectToAction("AdminDataInsertion", "Main");
                        }


                    }
                    if (target == "station")
                    {
                        alls = stationJsonParsor.ImportStations(filePath);
                        
                    }

                    if (target == "excel")
                    {

                        ExcelHelper excel = new ExcelHelper();
                       List<TrainSchedule> value=new List<TrainSchedule>();
                        try
                        {
                           value=  excel.ReadExcel(filePath);
                        }catch (Exception ex)
                        {
                            Console.WriteLine(ex.InnerException);
                            Console.Write("");
                        }


                        if (value != null)
                        {
                            using (var dx = _connectionContext.CreateDbContext())
                            {
                                await dx.TrainSchedule.AddRangeAsync(value);
                                updatedResult = await dx.SaveChangesAsync();

                            }

                            if (updatedResult >0)
                            {
                                
                                return Json(new { success = true, message = "Excel data uploaded successfully." });
                            }
                        }
                    }
                    if(target== "schedule")
                    {
                        List<TrainSchedule> trainSchedules = new List<TrainSchedule>();
                        JsonParsorTrainSchedule jsonParsorTrainSchedule = new JsonParsorTrainSchedule();
                        trainSchedules = jsonParsorTrainSchedule.jsonParseScheduleTain(filePath);

                        Console.WriteLine("");
                    }

                   
                   


                    if (alls.Count > 0)
                    {
                        using(var dx = _connectionContext.CreateDbContext())
                        {
                            
                           await dx.AllStations1.AddRangeAsync(alls);

                           val= await dx.SaveChangesAsync();
                         
                            
                        }
                    }
                  if(val > 0)
                    {
                        TempData["success"] = "All Json Data Has Uploaded to the Database....";
                        return RedirectToAction("AdminDataInsertion", "Main");
                    }

                }
                TempData["success"] = "Some error occured while reading file";
                return RedirectToAction("AdminDataInsertion","Main");
                   
            }
            catch (Exception ex)
            {

                TempData["Message"] = "File Must Needed";
                return RedirectToAction("AdminDataInsertion", "Main");
            }

        }

        public IActionResult AdminDataSetUpload()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> HandleDataUpload([FromForm] FileUpload fileUpload)
        {

            return Ok();
        }


    }


    
}
