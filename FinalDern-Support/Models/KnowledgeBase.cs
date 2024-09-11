namespace FinalDern_Support.Models
{
    public class KnowledgeBase
    {
        public int ID { get; set; }
        public int AdminID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }

        // Navigation property
       public Admin Admin { get; set; }

    }
}
