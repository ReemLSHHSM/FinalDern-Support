namespace FinalDern_Support.Models
{
    public class Quote
    {
        
            public int ID { get; set; }
            public int? RequestID { get; set; }   // Foreign key for Request
            public double Cost { get; set; }
            public TimeSpan StartAt { get; set; }
            public DateTime EndAt { get; set; }
            public string Priority { get; set; }
            public string Status { get; set; }

        // Navigation properties
        public Request Request { get; set; }

        // One-to-one relationship with Job
        public Job Job { get; set; }


    }
}
