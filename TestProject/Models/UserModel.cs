using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProject.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength =3,ErrorMessage ="Username must be Minimum 3 characters to 100 characters")]
        //[Remote("IsUsernameAvailable", "User", HttpMethod = "POST", ErrorMessage = "Username already exists.")]
        public string UserName { get; set; }
        
        [System.ComponentModel.DataAnnotations.Compare("GeneratedPassword", ErrorMessage = "GeneratedPassword and Password are mismatch")]
        public string Password { get; set; }
        public string GeneratedPassword { get; set; }
    }
}