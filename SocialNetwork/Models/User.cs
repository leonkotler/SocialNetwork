using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static SocialNetwork.Utils.Utils;

namespace SocialNetwork.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public MyGender Gender { get; set; }
        [Required]
        [Display(Name = "Email/Username")]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Group> Groups { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

    }
}