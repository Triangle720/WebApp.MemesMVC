using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.MemesMVC.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 4)]
        [Required]
        public string Login { get; set; }

        [StringLength(256, MinimumLength = 6)]
        [Required]
        public string Password { get; set; }

        [StringLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]
        [Required]
        public string Email { get; set; }

        public DateTime AccountCreationTime { get; set; }

        public virtual RoleModel Role { get; set; }

        public virtual List<PictureModel> Pictures { get; set; }
    }
}
