namespace FinalDern_Support.Models
{
    public class SparePart
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // Navigation property
        public ICollection<JobSpareParts> JobSpareParts { get; set; }
    }
}
