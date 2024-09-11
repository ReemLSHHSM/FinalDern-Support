namespace FinalDern_Support.Models
{
    public class Feedback
    {
        public int ID { get; set; }
        public int JobID { get; set; }     // Foreign key for Job
        public int CustomerID { get; set; } // Foreign key for Customer
        public string Title { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        // Navigation properties
        public Job Job { get; set; }
        public Customer Customer { get; set; }
    }

}
