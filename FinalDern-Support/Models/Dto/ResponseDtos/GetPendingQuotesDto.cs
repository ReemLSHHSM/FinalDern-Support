namespace FinalDern_Support.Models.Dto.ResponseDtos
{
    public class GetPendingQuotesDto
    {
        public int ID { get; set; }
        public double Cost { get; set; }
        public TimeSpan StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Status { get; set; }
    }
}
