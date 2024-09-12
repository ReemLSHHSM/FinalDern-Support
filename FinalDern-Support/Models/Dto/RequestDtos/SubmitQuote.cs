namespace FinalDern_Support.Models.Dto.RequestDtos
{
    public class SubmitQuote
    {
      
        public double Cost { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Priority { get; set; }
   
    }
}
