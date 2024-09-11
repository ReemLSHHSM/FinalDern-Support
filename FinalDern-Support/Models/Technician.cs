namespace FinalDern_Support.Models
{
    public class Technician
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Speciality { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}
