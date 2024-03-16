using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace SocialNetwork.BLL.Managers
{
    public class ImageManager
    {
        public string Upload()
        {
            var dlg = new OpenFileDialog { Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp" };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var result = File.ReadAllBytes(dlg.FileName);
                string imageBase64 = Convert.ToBase64String(result);

                return imageBase64;
            }
            return string.Empty;
        }
        public BitmapImage Download(string imageBase64)
        {
            byte[] bytes = Convert.FromBase64String(imageBase64);
            using (var stream = new MemoryStream(bytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
        }
        internal static BitmapImage GetPhoto(string photo)
        {
            if (string.IsNullOrEmpty(photo)) return null;

            var imageManager = new ImageManager();
            return imageManager.Download(photo);
        }
    }
}
