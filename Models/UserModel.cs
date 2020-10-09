using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.MemesMVC.Models
{
    public enum RoleTypes
    {
        USER,
        MODERATOR,
        ADMIN
    }

    public class UserModel
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 4)]
        [Required]
        public string Login { get; set; }

        [StringLength(256, MinimumLength = 6)]
        [Required]
        public string Password { get; set; }

        [StringLength(20, MinimumLength = 4)]
        [Required]
        public string Nickname { get; set; }

        [StringLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]
        [Required]
        public string Email { get; set; }

        public DateTime AccountCreationTime { get; set; } = DateTime.Now;

        public RoleTypes Role { get; set; } = RoleTypes.USER;

        public virtual List<PictureModel> Pictures { get; set; }

        public bool IsBanned { get; set; } = false;

        [AllowNull]
        public DateTime ?BanExpireIn { get; set; }
    }
}
