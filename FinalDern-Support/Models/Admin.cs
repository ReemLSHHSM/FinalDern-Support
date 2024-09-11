namespace FinalDern_Support.Models
{
    public class Admin
    {
        public int ID { get; set; }
        public string UserID { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }

        public ICollection<KnowledgeBase> KnowledgeBases { get; set; }
    }
}
