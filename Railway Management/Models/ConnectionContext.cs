using Microsoft.EntityFrameworkCore;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Models
{
    public class ConnectionContext:DbContext
    {

        public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set StationCode as the Primary Key for AllStations
            modelBuilder.Entity<AllStations>()
                .HasKey(a =>a.Id);

            // Optional: Configure additional properties
            modelBuilder.Entity<AllStations>()
                .Property(a => a.Id)
                .IsRequired(); // Make StationCode mandatory if necessary

            modelBuilder.Entity<TrainsRunningCoordinates>()
                .HasOne(t => t.TrainDetails)
                .WithMany().HasForeignKey(t => t.trainID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>().HasOne(t=>t.Customer).WithMany().HasForeignKey(t=>t.CustomerID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerPersonalDetails>().HasOne(t => t.customer).WithMany().HasForeignKey(t => t.CustomerID).OnDelete(DeleteBehavior.Cascade);
                
        }
        
        public DbSet<Station> Stations { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Train> Trains { get; set; }
            public DbSet<Booking> Bookings { get; set; }
        public DbSet<CustomerPersonalDetails> CustomerPersonalDetails { get; set; }
        public DbSet<Fare> Fares { get; set; }
           public DbSet<AllStations> AllStations1 { get; set; }
        public DbSet<TrainDetails> Train_Details { get; set; }
        public DbSet<TrainsRunningCoordinates> Trains_Running_Coordinates { get; set; }
        public DbSet<TrainSchedule> TrainSchedule { get; set; }
        

    }
}
