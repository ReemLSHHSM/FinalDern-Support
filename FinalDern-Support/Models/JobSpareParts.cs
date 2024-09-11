namespace FinalDern_Support.Models
{
    public class JobSpareParts
    {
        public int JobID { get; set; }
        public int SparePartID { get; set; }

        // Navigation properties
        public Job Job { get; set; }
        public SparePart SparePart { get; set; }

    }
}
