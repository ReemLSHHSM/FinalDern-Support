namespace FinalDern_Support.Models
{
    public class Request
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }

        public bool IsTaken { get; set; }
       // public int? JobID { get; set; }

        // Navigation property
        public Customer Customer { get; set; }

        //// One-to-one relationships
        public Quote Quote { get; set; }
       // public Job Job { get; set; }
    }
}
