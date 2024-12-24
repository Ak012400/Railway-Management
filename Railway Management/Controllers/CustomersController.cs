using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;
using Railway_Management.Validations;
using static Railway_Management.Models.AllDataDetails;
using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using NuGet.Protocol.Core.Types;
using NuGet.Common;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Office.Interop.Excel;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;

namespace Railway_Management.Controllers
{

    public class CustomersController : Controller
    {

        private readonly IDbContextFactory<ConnectionContext> _connectionContext;
        CustomerExistance customerExistance;
        public CustomersController(IDbContextFactory<ConnectionContext> context)
        {
            _connectionContext = context;
            customerExistance = new CustomerExistance(context);
        }
        [Authorize]
        public IActionResult CustomerMainPage()
        {
            ViewBag.CustomerName = HttpContext.Session.GetString("CustomerName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration([FromForm] Customer customers,string confirmPassword)
        {
            int savevalue = 0;
            if (customers != null)
            {
                bool isUserExist = await customerExistance.IsUserExist(customers);
                if (isUserExist)
                {
                    TempData["success"] = "User  Exist Already";
                    return RedirectToAction("Signup", "Home");
                }
                string ValidatePasswordCreation=Password.NewPasswordValidation(customers.Password,confirmPassword);
                if (ValidatePasswordCreation != "done")
                {
                    TempData["passwordError"] = ValidatePasswordCreation;
                    return RedirectToAction("Signup", "Home");
                }
                customers.Password=Password.GetHassedPassword(customers.Password);
                using (var dx = _connectionContext.CreateDbContext())
                {
                    await dx.Customers.AddAsync(customers);
                    savevalue = await dx.SaveChangesAsync();

                }


                if (savevalue > 0)
                {
                    TempData["success"] = "Registration Has Done";
                }
                else
                {
                    TempData["success"] = "Registration Failed";
                }

            }
            else
            {
                TempData["success"] = "Registration Failed";
                return RedirectToAction("Signup", "Home");
            }


            return RedirectToAction("Signup", "Home");
        }
        [HttpPost]
        [Route("/Customers/Login")]
        public async Task<IActionResult> Login([FromForm] LoginDetails loginDetails)
        {
            try
            {
                if (loginDetails == null)
                {
                    TempData["success"] = "Not found login details data";
                    return RedirectToAction("Login", "Home");
                }
                Customer data = null;
                string hashpassword = Password.GetHassedPassword(loginDetails.Password);
                using (var dx = _connectionContext.CreateDbContext())
                {
                   data=  dx.Customers.Where(x=>x.Email==loginDetails.Email && x.Password==hashpassword).FirstOrDefault();


                }



                if (data == null)
                {
                    TempData["success"] = "Check with you credentials";
                    return RedirectToAction("Login", "Home");
                }
                HttpContext.Session.SetString("CustomerName", data.Name);
                HttpContext.Session.SetString("Name", data.Name);
                HttpContext.Session.SetString("CustomerID", data.CustomerID.ToString());



                var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,data.Name),
                new Claim("Email",data.Email),
                new Claim("Contact",data.Contact),
                new Claim("CustomerID",data.CustomerID.ToString())

            };
                var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                return RedirectToAction("CustomerMainPage", "Customers");


            } catch (Exception ex)
            {
                TempData["success"] = "Some error has Occured\n" + ex.Message;
                return RedirectToAction("Login", "Home");
            }
        }
        public async Task<IActionResult> LogoutAsync()
        {
            TempData.Clear();
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> TicketBooking()
        {
            return View();
        }
        [Authorize]
        public IActionResult LoadTrainsPage()
        {

            if (TempData["TrainDetails"] != null)
            {
                var trainDetails = JsonConvert.DeserializeObject<List<ModelUserTrainSearchDetails>>((string)TempData["TrainDetails"]);
                return View(trainDetails);
            }
            else
            {
                TempData["success"] = "did not get the data from backend";
                return View();
            }



        }
        [Authorize]
        [HttpPost]
        [Route("Customers/TrainsCollections")]
        public async Task<IActionResult> TrainsCollection([FromForm] string source, [FromForm] string destination)
        {
            CreateDataForTrainDetailsAndFare createData = new CreateDataForTrainDetailsAndFare(_connectionContext);
            List<TrainSchedule> schedules = new List<TrainSchedule>();
            List<TrainSchedule> schedules1 = new List<TrainSchedule>();
            using (var dx = _connectionContext.CreateDbContext())
            {
                schedules = await dx.TrainSchedule.Where(x => x.Station_Code == source).ToListAsync();
                schedules1 = await dx.TrainSchedule.Where(x => x.Station_Code == destination).ToListAsync();
            }
            List<string> schedules2 = new List<string>();
            schedules2 = schedules1.Select(x => x.Train_Number).ToList();
            double distance1 = await createData.GetDistanceBetweenStations(source, destination);
            List<TrainSchedule> sourceSchedules = new List<TrainSchedule>();
            List<TrainSchedule> destinationSchedules = new List<TrainSchedule>();
            List<ModelUserTrainSearchDetails> detl = new List<ModelUserTrainSearchDetails>();
            foreach (var schedule in schedules)
            {
                if (schedules2.Contains(schedule.Train_Number))
                {
                    string BstationName = "";
                    string ArrivalTime = "";
                    try
                    {
                        BstationName = schedules1.Select(x => x.Station_Name).First();
                        ArrivalTime = schedules1
      .Where(x => x.Train_Number == schedule.Train_Number && x.Station_Code == destination)
      .Select(x => x.Arrival)
      .FirstOrDefault() ?? DateTime.MinValue.ToString(); // Yaha DefaultValue set karo

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex);

                    }

                    detl.Add(new ModelUserTrainSearchDetails { TrainName = schedule.Train_Name, TrainNumber = schedule.Train_Number, distance = distance1, StationName = schedule.Station_Name, StationCode = schedule.Station_Code, DepartureTime = schedule.Departure, BStationName = BstationName, BStationCode = destination, ArrivalTime = ArrivalTime });

                    Console.WriteLine("");

                }



            }
            TempData["TrainDetails"] = JsonConvert.SerializeObject(detl);


            return RedirectToAction("LoadTrainsPage", "Customers");
        }
        [HttpPost]
        public async Task<IActionResult> BookingTicket([FromForm] ModelUserTrainSearchDetails data)
        {
            string custid = User.FindFirst("CustomerID")?.Value;
            DateTime date = DateTime.Now;
            Booking booking = new Booking()
            {
                CustomerID = int.Parse(custid),
                StationName = data.StationName,
                StationCode = data.StationCode,
                BStationName = data.BStationName,
                BStationCode = data.BStationCode,
                TrainName = data.TrainName,
                TrainNumber = data.TrainNumber,
                DepartureTime = data.DepartureTime.ToString(),
                Distance = data.distance,
                ArrivalTime = data.ArrivalTime.ToString(),
                Fare = data.Fare,
                BookingDate = date.ToString(),


            };
            int val = 0;
            using (var dx = _connectionContext.CreateDbContext())
            {
                await dx.Bookings.AddAsync(booking);
                val = await dx.SaveChangesAsync();

                if (val > 0)
                {
                    TempData["success"] = "Booking Has Done You Can See The Booking In Your Profile";
                    return RedirectToAction("CustomerMainPage", "Customers");
                }
            }
            TempData["success"] = "Booking Has Not done";
            return RedirectToAction("CustomerMainPage", "Customer");
        }

        [Authorize]
        public async Task<IActionResult> GetBookingDetails()
        {
            string customerID = User.FindFirst("CustomerID")?.Value;
            using (var db = _connectionContext.CreateDbContext())
            {
                var data = await db.Bookings.Where(x => x.CustomerID == int.Parse(customerID)).ToListAsync<Booking>();
                if(data != null)
                {
                    return View(data);

                }
               
                return View(data);
               
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CustomerDetails()
        {
            
            string userid = User.FindFirst("CustomerID")?.Value;
            try
            {
                using (var dx = _connectionContext.CreateDbContext())
                {
                    var data = dx.CustomerPersonalDetails.Where(x => x.CustomerID == int.Parse(userid)).SingleOrDefault()??null;
                    
                    if (data == null)
                    {

                        CustomerPersonalDetails customerPersonalDetails = new CustomerPersonalDetails()
                        {
                            Address = "Please Update Your address",
                            State = "Please Update Your address",
                            Country = "Please Update Your address",
                            pincode = "Please Update Your address",
                            detailsID = 0,
                            CustomerID = int.Parse(userid),
                            City = "Please Update Your address",
                            ImageURL = "Please Update Your address"

                        };
                        return View(customerPersonalDetails);


                    }
                    return View(data);



                }
            }catch(Exception ex)
            {
                Console.Write(ex.InnerException);
                Console.Write("");
                TempData["success"] = "there is an exceptoon occured\n" + ex.Message;
                return View();
            }

            

        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserDetails([FromForm] Customer customer)
        {
            string userID = User.FindFirst("CustomerID")?.Value;
            int val = 0;
            using (var dx = _connectionContext.CreateDbContext())
            {



                int CustomerId = int.Parse(userID);
                string Name = customer.Name;
                string Contact = customer.Contact;

                val = dx.Database.ExecuteSqlRaw(
                      "EXEC UpdateCustomerDetails @CustomerId, @Name, @Contact",
                      new SqlParameter("@CustomerId", CustomerId),
                      new SqlParameter("@Name", Name),
                      new SqlParameter("@Contact", Contact)
                );

            }
            if (val > 0)
            {
                TempData["success"] = "Your Records Has been Updated----Please Relogin To Effect Updates..";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["success"] = "Your Records Has Not been Updated";
                return RedirectToAction("CustomerMainPage", "Customers");
            }



        }
       
         public async Task<IActionResult> UpdatePersonalDetails([FromForm] CustomerPersonalDetails customerPersonalDetails)
         {
            int val = 0;
            
            string UserID = User.FindFirst("CustomerID")?.Value;
            customerPersonalDetails.CustomerID=int.Parse(UserID);
            if (customerPersonalDetails.ImageURL == null)
            {
                customerPersonalDetails.ImageURL = "nourl";
            }
            if (customerPersonalDetails.detailsID != 0)
            {
              
                using (var dx = _connectionContext.CreateDbContext())
                {
                    val = dx.Database.ExecuteSqlRaw(
                              "EXEC UpdateCustomerPersonalDetails @detailsID, @CustomerID, @Address,@City,@pincode,@Country,@State,@ImageURL",
                              new SqlParameter("@detailsID", customerPersonalDetails.detailsID),
                              new SqlParameter("@CustomerID", int.Parse(UserID)),
                              new SqlParameter("@Address", customerPersonalDetails.Address),
                              new SqlParameter("@City", customerPersonalDetails.City),
                              new SqlParameter("@pincode", customerPersonalDetails.pincode),
                              new SqlParameter("@Country", customerPersonalDetails.Country),
                              new SqlParameter("@State", customerPersonalDetails.State),
                              new SqlParameter("@ImageURL", customerPersonalDetails.ImageURL)
                     );
                }
            }
            else
            {
                using (var dx = _connectionContext.CreateDbContext())
                {
                    dx.CustomerPersonalDetails.Update(customerPersonalDetails);
                    val = await dx.SaveChangesAsync();
                }
            }
           
            
           
            if (val != 0)
            {
                TempData["success"] = "Your Data Has Benn Submitted Successfully..";
                return RedirectToAction("CustomerDetails", "Customers");
            }
            TempData["success"] = "Your Data Has Not Benn Submitted Successfully..";
            return RedirectToAction("CustomerDetails", "Customers");
        }

        [HttpPost]
        [Route("/Customers/UpdateCustomerPassword")]
        public async Task<IActionResult> UpdateCustomerPassword([FromForm] string password, [FromForm] string confirmPassword, [FromForm] string currentPassword)
        {
            string userID = User.FindFirst("CustomerID")?.Value;
            string realPassword;

            using (var dx = _connectionContext.CreateDbContext())
            {
                realPassword=dx.Customers.Where(x=>x.CustomerID==int.Parse(userID)).Select(x=>x.Password).FirstOrDefault();
            }
            bool isvalidpassword = Password.PasswordValidation(currentPassword, realPassword);
            if (isvalidpassword)
            {
                string result=Password.NewPasswordValidation(password,confirmPassword);

                if (result == "done")
                {
                    //update password.......
                    password=Password.GetHassedPassword(password);
                    string val = "";
                    using(var dx = _connectionContext.CreateDbContext())
                    {
                        var userIdParam = new SqlParameter("@CustomerID", int.Parse(userID));
                        var newPasswordParam = new SqlParameter("@Password", password);
                        var statusMessageParam = new SqlParameter("@StatusMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output // Set as OUTPUT
                        };

                        var result0 = dx.Database.ExecuteSqlRaw(
                            "EXEC UpdatePassword @CustomerID, @Password, @StatusMessage  OUT",
                            userIdParam, newPasswordParam, statusMessageParam
                        );

                       val=result0.ToString();

                    }


                    TempData["success"] = "Your Password Has been successfully..Updated....Please Login Again..";

                    return RedirectToAction("Logout", "Customers");
                }
                else
                {
                    TempData["passwordError"] = result;
                    return RedirectToAction("CustomerDetails", "Customers");
                }
            }
            else
            {
                TempData["passwordError"] = "Invalid Password Input We Have Different Password From Your Input";
                return RedirectToAction("CustomerDetails", "Customers");
            }
        }
    }
}
