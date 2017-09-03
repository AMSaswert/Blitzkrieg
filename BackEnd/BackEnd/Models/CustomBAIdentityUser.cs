using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class CustomBAIdentityUser
    {
        [StringLength(256)]
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }


        [StringLength(256)]
        public string Password { get; set; }

        [Required]
        [StringLength(256)]
        public string Role { get; set; }

        public int AppUserId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Surname { get; set; }

        [Required]
        [StringLength(256)]
        public string ContactPhone { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string NewPassword { get; set; }
    }
}