using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<User> Members { get; set; }
    }
}