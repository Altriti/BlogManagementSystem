using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Author Name is required")]
        public string AuthorName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string AuthorEmail { get; set; }
        [Required(ErrorMessage = "Author Name is required")]
        public string Content { get; set; }

    }
}
