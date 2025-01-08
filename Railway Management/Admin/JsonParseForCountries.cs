using Newtonsoft.Json;
using static Railway_Management.Models.AllDataDetails;
using System.IO;
using System.Net.Http.Json;

namespace Railway_Management.Admin
{
    public class JsonParseForCountries
    {


        public static AllCountries_States ExtractCountries(string path)
        {
            List<CountriesExtract> data = new List<CountriesExtract>();
            // Deserializing the JSON string to a List of TrainSchedule objects
            try
            {
                string AllText = File.ReadAllText(path);
                List<AllStates> allStates = new List<AllStates>();
                List<AllCountries> allCountries = new List<AllCountries>();
                List<CountriesExtract> countrydata = JsonConvert.DeserializeObject<List<CountriesExtract>>(AllText);
                int n = 0;
                foreach (CountriesExtract details in countrydata)
                {
                    n++;
                    AllCountries country = new AllCountries();
                    Console.WriteLine(details.name+details.phone+details);
                    Console.WriteLine();
                    country.countryID = n;
                    country.countryphone = details.phone;
                    country.countryname = details.name ?? "Not Available";
                    country.flag = details.flag?? "Not Available";
                    country.countrycodealpha3 = details.countryCodeAlpha3 ?? "Not Available";
                    country.countrycode=details.countryCode??"Not Available";
                    country.currency=details.currency??"Not Available";
                    country.symbol = details.symbol;
                    


                    
                   foreach(var da in details.stateProvinces)
                    {
                        AllStates states = new AllStates();
                        Console.WriteLine(da.name);
                        states.countryID =n;
                        states.statename=da.name;
                        allStates.Add(states);
                    }
                   allCountries.Add(country);
                }
                AllCountries_States allCountries_States = new AllCountries_States();
                allCountries_States.countries = allCountries;
                allCountries_States.states = allStates;
                return allCountries_States;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.InnerException);
                Console.WriteLine(Ex.Message);
                return null;
            }

        }
    }
}
