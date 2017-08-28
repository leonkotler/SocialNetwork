using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set;}

        public int? GroupId { get; set; }

        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [ScaffoldColumn(false)]
        public int Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}