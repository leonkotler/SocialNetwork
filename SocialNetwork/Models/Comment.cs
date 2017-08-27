using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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

        public string Content { get; set; }
        public int Likes { get; set; }
    }
}