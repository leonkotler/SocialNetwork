using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int? PostId { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Enter the post's content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [ScaffoldColumn(false)]
        public int Likes { get; set; }
    }
}