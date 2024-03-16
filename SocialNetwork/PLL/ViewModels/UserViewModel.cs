using ReactiveUI;
using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Entities;
using SocialNetwork.PLL.Helpers;
using SocialNetwork.PLL.View;
using System.Net;
using System.Reactive;
using System.Security;

namespace SocialNetwork.PLL.ViewModels
{
    public class UserViewModel:BaseViewModel
    {
        private UserService userService;
        private BookService bookService;
        private MovieService movieService;

        public UserViewModel(UserService userService, BookService bookService, MovieService movieService) 
        {
            this.userService = userService;
            this.bookService = bookService;
            this.movieService = movieService;

            UploadPhotoCommand = ReactiveCommand.Create(UploadPhoto);
            UpdateUserCommand = ReactiveCommand.Create(UpdateUser);
        }

        private SecureString _userPassword;


        #region Properities
        private User _user;
        public User User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> UploadPhotoCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateUserCommand { get; }
        #endregion

        #region Methods
        public void ShowProfile(User user)
        {
            User = user;
            var book = bookService.FindById(user.FavoriteBook);
            if (book != null && !string.IsNullOrEmpty(book.image))
            {
                User.BitmapBookImage = ImageManager.GetPhoto(book.image);
            }
            var movie = movieService.FindById(User.FavoriteMovie);
            if (movie != null && !string.IsNullOrEmpty(movie.image))
            {
                User.BitmapMovieImage = ImageManager.GetPhoto(movie.image);
            }
            MainViewModel.Instance.BaseContentControl = new ProfileInfoView { DataContext = this };
        }
        public void ShowEditProfile(User user)
        {
            User = user;
            user.StringPassword = new NetworkCredential(string.Empty, User.Password).Password;

            User.Password = null;
            bookService.GetAll();
            movieService.GetAll();

            User.AllBooks = bookService.AllBooks;
            User.AllMovies = movieService.AllMovies;

            MainViewModel.Instance.BaseContentControl = new EditProfileView { DataContext = this };
        }
        private void UploadPhoto()
        {
            var imageManager = new ImageManager();
            var image = imageManager.Upload();
            if (image != null)
            {
                User.Photo = image;
                try
                {
                    if (!string.IsNullOrEmpty(User.Photo))
                    {
                        User.BitmapImage = ImageManager.GetPhoto(User.Photo);
                    }
                }
                catch (Exception ex)
                {
                    AlertMessage.Show(ex.Message);
                }
            }

        }
        private void UpdateUser()
        {
            try
            {
                userService.Update(User);
                SuccessMessage.Show($"Пользователь {User.FirstName} {User.LastName} обновлен!");
                MainViewModel.Instance.RefreshUser();
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }
        }

        #endregion
    }
}
