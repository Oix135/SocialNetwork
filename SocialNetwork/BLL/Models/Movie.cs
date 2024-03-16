using System.Windows.Media.Imaging;

namespace SocialNetwork.BLL.Models
{
    public class Movie:BaseModel
    {
        public int id { get; set; }

        private string _name;
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanAddMovie));
            }
        }
        public string image { get; set; }

        private BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get => _bitmapImage;
            set
            {
                _bitmapImage = value;
                RaisePropertyChanged();
            }
        }

        public Movie(int id, string name, string image)
        {
            this.id = id;
            this.name = name;
            this.image = image;
        }
        public bool CanAddMovie
        {
            get => !string.IsNullOrEmpty(name);
        }
    }
}
