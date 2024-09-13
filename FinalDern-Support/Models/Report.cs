namespace FinalDern_Support.Models
{
    
        public class Report
        {
            public int ID { get; set; }
            public int JobID { get; set; }   // Foreign key for Job
            public int TechnicianID { get; set; } // Foreign key for Technician
            public string Title { get; set; }
            public string Description { get; set; }
            public double TotalPrice { get; set; }
            public string TotalTime { get; set; }
            public int NumberOfPartsUsed { get; set; }

            // Navigation properties
            public Job Job { get; set; }
            public Technician Technician { get; set; }
        }

    }

