using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models
{
    public class Post
    {
        public int PostID { get; set; }
        // Author
        public int UserID { get; set;}
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}