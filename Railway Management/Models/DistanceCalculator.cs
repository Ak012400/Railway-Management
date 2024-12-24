namespace Railway_Management.Models
{
    public class DistanceCalculator
    {
        private const double EarthRadiusKm = 6371.0;
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Convert degrees to radians
            decimal? lat1Rad = (decimal?)DegreesToRadians(lat1);
            decimal? lon1Rad = (decimal?)DegreesToRadians(lon1);
            decimal? lat2Rad = (decimal?)DegreesToRadians(lat2);
            decimal? lon2Rad = (decimal?)DegreesToRadians(lon2);

            // Haversine formula
            double dLat = (double)((decimal?)lat2Rad - lat1Rad);
            double dLon = (double)(lon2Rad - lon1Rad);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos((double)lat1Rad) * Math.Cos((double)lat2Rad) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distance in kilometers
            return EarthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
