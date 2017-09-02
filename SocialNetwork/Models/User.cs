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
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage ="First name is requried")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is requried")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is requried")]
        public MyGender Gender { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Image Url")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [ScaffoldColumn(false)]
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}