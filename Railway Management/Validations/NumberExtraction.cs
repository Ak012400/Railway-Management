using System.Text.RegularExpressions;

namespace Railway_Management.Validations
{
    public class NumberExtraction
    {

        public static string  NumberExtractionFromString(string value) {

            
            string pattern = @"\d+";
            string returnvalue = "";
            MatchCollection matches = Regex.Matches(value, pattern);

            foreach (Match match in matches)
            {
               returnvalue=match.Value;
            }
            return returnvalue;

        }
    }
}
