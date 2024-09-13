namespace FinalDern_Support.Models.Dto.ResponseDtos
{
    public class PostReport
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double TotalPrice { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int NumberOfPartsUsed { get; set; }
    }
}
