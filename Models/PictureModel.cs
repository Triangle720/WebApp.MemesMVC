using System;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.MemesMVC.Models
{
    public class PictureModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [AllowNull]
        public string UrlAddress { get; set; }

        public DateTime UploadTime { get; set; } = DateTime.Now;
        
        [AllowNull]
        public string LocalPath { get; set; }

        public bool IsAccepted { get; set; } = false;

    }
}
