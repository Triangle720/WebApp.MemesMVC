using System;

namespace WebApp.MemesMVC.Models
{
    public class PictureModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UrlAddress { get; set; }

        public DateTime UploadTime { get; set; }
        
        //public string LocalPath { get; set; }

        //public bool IsAccepted { get; set; }



    }
}
