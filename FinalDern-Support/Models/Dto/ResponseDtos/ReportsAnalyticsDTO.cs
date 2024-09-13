namespace FinalDern_Support.Models.Dto.ResponseDtos
{
    public class ReportsAnalyticsDTO
    {
        public double TotalCost { get; set; }
        public double AverageCost { get; set; }
        //public TimeSpan TotalTime { get; set; }
        //public TimeSpan AverageTime { get; set; }
        public int TotalNumberOfPartsUsed { get; set; }
        public double AverageNumberOfPartsUsed { get; set; }
        public string MostRequestedLocation { get; set; }
        public double AverageRating { get; set; }
        public List<string> CommonIssuesDealtWith { get; set; }
        public Dictionary<string, int> JobsByLocation { get; set; } // Location and the number of jobs
    }
}
