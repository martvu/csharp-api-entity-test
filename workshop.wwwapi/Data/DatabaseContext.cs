using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //_connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Appointment Key etc.. Add Here
            modelBuilder.Entity<Appointment>()
                .HasKey(a => new { a.DoctorId, a.PatientId });

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            //TODO: Seed Data Here
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, FullName = "Martin Marson" },
                new Patient { Id = 2, FullName = "Bob Bobson" },
                new Patient { Id = 3, FullName = "Alice Alisson" }
            );

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, FullName = "Mike Mikeson" },
                new Doctor { Id = 2, FullName = "Robin Robinson" }
            );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { Booking = new DateTime(2025, 1, 27, 10, 0, 0, DateTimeKind.Utc), DoctorId = 1, PatientId = 1 },
                new Appointment { Booking = new DateTime(2025, 1, 31, 10, 0, 0, DateTimeKind.Utc), DoctorId = 2, PatientId = 3 }
            );



        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            //optionsBuilder.UseNpgsql(_connectionString);
            //optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
            
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
