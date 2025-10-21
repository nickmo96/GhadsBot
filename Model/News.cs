using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhadsBot.Model
{
    public class News
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string PictureUrl { get; set; }

        public News(long id, string title, string text, string summary, string url, string pictureUrl)
        {
            Id = id;
            Title = title;
            Text = text;
            Summary = summary;
            Url = url;
            PictureUrl = pictureUrl;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Summary: {Summary}, Url: {Url}, PictureUrl: {PictureUrl}";
        }


    }
}
