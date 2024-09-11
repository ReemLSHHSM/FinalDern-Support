using System.Composition;

namespace FinalDern_Support.Models
{
    public class Job
    {
        public int ID { get; set; }
        public int QuoteID { get; set; }   // Foreign key for Quote
        public int TechID { get; set; }    // Foreign key for Technician
        public bool IsComplete { get; set; }

        // Foreign keys for one-to-one relationships
        public int ReportID { get; set; }   // Foreign key for Report (optional)
        public int FeedbackID { get; set; } // Foreign key for Feedback (optional)

        // Navigation properties
        public Quote Quote { get; set; }
        public Technician Technician { get; set; }

        public Report Report { get; set; }
        public Feedback Feedback { get; set; }

        public ICollection<JobSpareParts> JobSpareParts { get; set; }
    }

}

