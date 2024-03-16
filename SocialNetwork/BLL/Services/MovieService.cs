using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories.Interfaces;
using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.BLL.Services
{
    public class MovieService
    {
        IHobbieRepository<MovieEntity> movieRepository;

        public MovieService()
        {
            movieRepository = new MovieRepository<MovieEntity>();
        }
        public List<Movie> AllMovies { get; set; } = [];

        public void Create(Movie movie)
        {
            var bookEntity = new MovieEntity
            {
                id = movie.id,
                name = movie.name,
                image = movie.image
            };
            if (movieRepository.Create(bookEntity) == 0)
            {
                throw new Exception();
            }
        }
        public void GetAll()
        {
            AllMovies.Clear();
            var imageManager = new ImageManager();
            var listMovies = movieRepository.FindAll();
            foreach (var movieEntity in listMovies)
            {
                var movie = ConstructMovieModel((MovieEntity)movieEntity);
                if (!string.IsNullOrEmpty(movie.image))
                {
                    movie.BitmapImage = imageManager.Download(movie.image);
                }
                AllMovies.Add(movie);
            }
        }
        public void Update(Movie movie)
        {
            var movieEntity = new BookEntity
            {
                id = movie.id,
                name = movie.name,
                image = movie.image
            };
            if (movieRepository.Update(movieEntity) == 0)
            {
                throw new Exception();
            }
        }
        public Movie? FindByName(string name)
        {
            var movieEntity = movieRepository.FindByName(name);
            if (movieEntity == null) return null;

            return ConstructMovieModel((MovieEntity)movieEntity);

        }

        public Movie FindById(int favoriteMovie)
        {
            var movieEntity = movieRepository.FindById(favoriteMovie);
            if (movieEntity == null) return null;

            return ConstructMovieModel((MovieEntity)movieEntity);
        }
        private Movie ConstructMovieModel(MovieEntity movieEntity)
        {
            return new Movie(movieEntity.id,
                          movieEntity.name,
                          movieEntity.image);
        }
        public Movie ConstructNewMoovieModel()
        {
            return new Movie(0, string.Empty, string.Empty);
        }
    }
}
