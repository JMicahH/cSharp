using System.ComponentModel.DataAnnotations;


namespace theWall.Models
{
    public class Message
    {
        [Required]
        [MinLength(3)]
        public string messageContent { get; set; }

        [Required]
        public int authorId { get; set; }

    }
}