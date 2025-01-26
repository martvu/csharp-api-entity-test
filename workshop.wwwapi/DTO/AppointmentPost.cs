using System.ComponentModel.DataAnnotations.Schema;

namespace workshop.wwwapi.DTO
{
    public class AppointmentPost
    {
        public DateTime Booking { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}
