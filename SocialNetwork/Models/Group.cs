using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public int AdminId { get; set; }

        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public int Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<User> Members { get; set; }
    }
}