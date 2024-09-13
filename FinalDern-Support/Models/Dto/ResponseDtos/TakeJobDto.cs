namespace FinalDern_Support.Models.Dto.ResponseDtos
{
    public class TakeJobDto
    {
        public int QuoteID { get; set; }   // Foreign key for Quote
        public int TechID { get; set; }    // Foreign key for Technician
        public bool IsComplete { get; set; }
    }
}
