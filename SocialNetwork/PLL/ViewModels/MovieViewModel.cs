using ReactiveUI;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using SocialNetwork.PLL.View;
using System.Reactive;
using System.Windows;

namespace SocialNetwork.PLL.ViewModels
{
    public class MovieViewModel:BaseViewModel
    {
        private MovieService movieService;
        public MovieViewModel(MovieService movieService) 
        {
            this.movieService = movieService;

            AllMovies = new List<Movie>();

            var canAddMovie = this.WhenAnyValue(x => x.Movie.CanAddMovie);

            UploadCoverMovieCommand = ReactiveCommand.Create(UploadCoverMovie);
            AddMovieCommand = ReactiveCommand.Create(AddMovie, canAddMovie);
            EditMovieCommand = ReactiveCommand.Create(EditMovie, canAddMovie);
            OpenEditMovieCommad = ReactiveCommand.Create<bool>(OpenEditMovie);
        }

        #region Properties

        private Movie _movie;
        public Movie Movie
        {
            get => _movie;
            set
            {
                this.RaiseAndSetIfChanged(ref _movie, value);
                this.RaisePropertyChanged(nameof(AddMovieVisible));
                this.RaisePropertyChanged(nameof(EditMovieVisible));
            }
        }
        public bool AddMovieVisible
        {
            get => Movie?.id == 0;
        }
        public bool EditMovieVisible
        {
            get => Movie?.id > 0;
        }

        public List<Movie> AllMovies { get; }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> UploadCoverMovieCommand { get; }
        public ReactiveCommand<Unit, Unit> AddMovieCommand { get; }
        public ReactiveCommand<Unit, Unit> EditMovieCommand { get; }
        public ReactiveCommand<bool, Unit> OpenEditMovieCommad { get; }

        #endregion

        #region Methods

        public void Show()
        {
            GetAllMovies();
            MainViewModel.Instance.BaseContentControl = new MoviesView { DataContext = this };
        }
        public void OpenEditMovie(bool edit)
        {
            if (!edit)
            {
                Movie = movieService.ConstructNewMoovieModel();
            }
            MainViewModel.Instance.BaseContentControl = new EditMoviesView { DataContext = this };
        }


        private void UploadCoverMovie()
        {
            var imageManager = new ImageManager();
            var image = imageManager.Upload();
            if (!string.IsNullOrEmpty(image))
            {
                Movie.image = image;
                Movie.BitmapImage = ImageManager.GetPhoto(image);
            }
        }
        private void AddMovie()
        {
            try
            {
                var findedMovie = movieService.FindByName(Movie.name);
                if (findedMovie != null)
                {
                    Movie = findedMovie;
                    if (!string.IsNullOrEmpty(Movie.image))
                    {
                        Movie.BitmapImage = ImageManager.GetPhoto(Movie.image);
                    }
                    throw new MovieExistException();
                }
                else
                {
                    movieService.Create(Movie);
                    SuccessMessage.Show($"Фильм {Movie.name} добавлен!");
                    Movie = movieService.ConstructNewMoovieModel();
                }
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }
        }
        private void EditMovie()
        {
            try
            {
                movieService.Update(Movie);
                SuccessMessage.Show($"Фильм {Movie.name} отредактирован!");
                Movie = movieService.ConstructNewMoovieModel();
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }
        }

        private void GetAllMovies()
        {
            movieService.GetAll();
            AllMovies.Clear();
            AllMovies.AddRange(movieService.AllMovies);
            this.RaisePropertyChanged(nameof(AllMovies));
        }

        #endregion
    }
}
