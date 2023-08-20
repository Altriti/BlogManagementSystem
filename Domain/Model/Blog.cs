namespace Domain.Model
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
