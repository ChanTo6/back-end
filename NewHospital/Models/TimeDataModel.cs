using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NewHospital.Models
{
    public class TimeDataModel
    {
        [Key]
        public int Id { get; set; }
        public int  personalID { get; set; }
        public string? DoctorId { get; set; }
        public string? TimeRange { get; set; }

        public string? Day { get; set; }
        public string? StartTime { get; set; } 
        public string? EndTime { get; set; } 

    }
}
