using Castle.Components.DictionaryAdapter;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.MemesMVC.Models
{
    enum ImageExtensions
    {
        JPG,
        JPEG,
        PNG,
        GIF
    }

    public class PictureModel
    {
        public int Id { get; set; }

        public int UserModelId { get; set; }

        [AllowNull]
        public string UrlAddress { get; set; }

        public DateTime UploadTime { get; set; } = DateTime.Now;

        [AllowNull]
        public string LocalPath { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string Title { get; set; }

        public bool IsAccepted { get; set; }

    }
}
