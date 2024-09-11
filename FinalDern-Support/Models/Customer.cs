using Azure.Core;

namespace FinalDern_Support.Models
{
    public class Customer { 
    public int ID { get; set; }
    public string UserID { get; set; }
    public bool IsBusiness { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}
