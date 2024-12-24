using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Railway_Management.Models
{
    public class AllDataDetails
    {

        public class Station
        {
            public int StationID { get; set; }
            public string StationName { get; set; }
            public string Location { get; set; }
        }

        public class Customer
        {
            [Key]
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Train
        {
            public int TrainID { get; set; }
            public string TrainName { get; set; }
            public string Type { get; set; }
            public string Source { get; set; }
            public string Destination { get; set; }
        }
        public class Booking
        {
            [Key]
            public int BookingID { get; set; }
            public int CustomerID { get; set; }
            public string TrainName { get; set; }
            public string TrainNumber { get; set; }
            public double Distance { get; set; }
            public string BStationName { get; set; }
            public string BStationCode { get; set; }
            public string ArrivalTime { get; set; }  // You can use TimeSpan or DateTime for time if needed
            public string StationName { get; set; }
            public string StationCode { get; set; }
            public string DepartureTime { get; set; }  // You can use TimeSpan or DateTime for time if needed
            public int Fare { get; set; }
            public string BookingDate { get; set; }

            // Navigation property for Customer (if you want to use ORM like Entity Framework)
            public Customer Customer { get; set; }
        }


        public class Fare
        {
           
            public int FareID { get; set; }
            public int TrainID { get; set; }
            public string Category { get; set; } // Local/Regional, Long Distance, Cargo
            public decimal Amount { get; set; }
        }
        public class AllStations
        {
            [Key]
            public  int Id { get; set; }
            public string station_code { get; set; }
            public string station_name { get; set; }
            public string state { get; set; }
            public string zone { get; set; }
            public string address { get; set; }
            public decimal? latitude { get; set; }
            public decimal? longitude { get; set; }
        }
        public class TrainDetails
        {
            [Key]
            public int trainID { get; set; }
 
            public string trainNO {  get; set; }    
            public string trainName {  get; set; }  
            public int? third_ac {  get; set; }
           
            public TimeSpan?   arrival_time {  get; set; } 

            public string from_station_code {  get; set; }   
            public string zone { get; set; }
            public int?  chair_car {  get; set; }
            
            public int? first_class {  get; set; }  
            public int? sleeper {  get; set; }
            public int? distance_cover {  get; set; }   
            public TimeSpan? departure_time {  get; set; }
            public int  distance { get; set; }
            public string trainType {  get; set; }   
            public int? duration_min { get; set; }
            public string tostationname {  get; set; }
            public string Classes {  get; set; }
            public int? seconndac {  get; set; }
            public string tostationcode {  get; set; }
            public string returntrain {  get; set; }
            public int? duration_hrs {  get; set; }
            public string from_station_name {  get; set; }
            public int firstac { get; set; }

        }
        public class TrainsRunningCoordinates
        {
            [Key]
            public int Coordinatesid { get; set; }
            public int trainID { get; set; } 

            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }


            [ForeignKey("trainID")]
            public TrainDetails TrainDetails { get; set; }
        }
        public class LoginDetails
        {
            public String Email { get; set; }
            public String Password { get; set; }
        }

        public class TrainSchedule
        {
            [Key]
            public int Id { get; set; } // Primary Key
            public string Arrival { get; set; }
            public int Day { get; set; }
            public string Train_Name { get; set; }
            public string Station_Name { get; set; }
            public string Station_Code { get; set; }
            public string Train_Number { get; set; }
            public TimeSpan Departure { get; set; }

           
        }

        public class CustomerPersonalDetails
        {
            [Key]
            public int detailsID { get; set; }
            public int CustomerID { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string pincode { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string ImageURL { get; set; }
            public Customer customer { get; set; }
        }


    }
}
