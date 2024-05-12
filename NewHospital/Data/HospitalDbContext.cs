
using Microsoft.EntityFrameworkCore;
using NewHospital.Models;
using System.Collections.Generic;

namespace hospital.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions options) : base(options) { }
        public DbSet<RegisterModel> Register { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<TimeDataModel> TimeData { get; set; }
    }
}
