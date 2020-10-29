using System;

namespace WebApp.MemesMVC.Models
{
    public class DisplayedPictureModel
    {
        public string PictureTitle { get; }
        public string Url { get; }
        public string UserModelNickname { get; }

        public DateTime UploadTime { get; }

        public DisplayedPictureModel(string pictureTitle, string url, string userModelNickname, DateTime uploadTime)
        {
            PictureTitle = pictureTitle;
            Url = url;
            UserModelNickname = userModelNickname;
            UploadTime = uploadTime;
        }
    }
}
