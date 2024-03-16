using SocialNetwork.BLL.Managers;
using System.Security;
using System.Windows.Media.Imaging;

namespace SocialNetwork.BLL.Models
{
    public class User:BaseModel
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;  
                RaisePropertyChanged();
            }
        }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int FavoriteMovie { get; set; }
        public int FavoriteBook { get; set; }

        public string StringPassword { get; set; }

        public IEnumerable<Message> IncomingMessages { get; }
        public IEnumerable<Message> OutgoingMessages { get; }

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

        private BitmapImage _bitmapBookImage;
        public BitmapImage BitmapBookImage
        {
            get => _bitmapBookImage;
            set
            {
                _bitmapBookImage = value;
                RaisePropertyChanged();
            }
        }

        private BitmapImage _bitmapMovieImage;
        public BitmapImage BitmapMovieImage
        {
            get => _bitmapMovieImage;
            set
            {
                _bitmapMovieImage = value;
                RaisePropertyChanged();
            }
        }
        public List<Book> AllBooks { get; set; }
        public List<Movie> AllMovies { get; set; }
        public User(
            int id,
            string firstName,
            string lastName,
            SecureString password,
            string email,
            string photo,
            int favoriteMovie,
            int favoriteBook,
            IEnumerable<Message> incomingMessages,
            IEnumerable<Message> outgoingMessages)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            Photo = photo;
            FavoriteMovie = favoriteMovie;
            FavoriteBook = favoriteBook;
            IncomingMessages = incomingMessages;
            OutgoingMessages = outgoingMessages;

        }
        public User(
            int id, 
            string firstName, 
            string lastName, 
            string email, 
            string photo, 
            int favoriteMovie, 
            int favoriteBook
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Photo = photo;
            FavoriteMovie = favoriteMovie;
            FavoriteBook = favoriteBook;
            BitmapImage = ImageManager.GetPhoto(photo);
        }
    }
}
