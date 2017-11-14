using System.ComponentModel.DataAnnotations;


namespace theWall.Models
{
    public class Comment
    {
        [Required]
        [MinLength(3)]
        public string commentContent { get; set; }

        [Required]
        public int messageId { get; set; }

        [Required]
        public int authorId { get; set; }

    }
}