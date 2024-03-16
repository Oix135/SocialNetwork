using System.Windows.Media.Imaging;

namespace SocialNetwork.BLL.Models
{
    public class PrettyMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public BitmapImage BitmapImage { get; set; }
        public string SenderName { get; internal set; }
    }
}
