using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.DAL.Repositories.Interfaces;

namespace SocialNetwork.BLL.Services
{
    public class BookService
    {
        IHobbieRepository<BookEntity> bookRepository;

        public BookService()
        {
            bookRepository = new BookRepository<BookEntity>();
        }
        public List<Book> AllBooks { get; set; } = [];

        public void Create(Book book)
        {
            var bookEntity = new BookEntity
            {
                id = book.id,
                name = book.name,
                image = book.image
            };
            if (bookRepository.Create(bookEntity) == 0)
            {
                throw new Exception();
            }
        }
        public void GetAll()
        {
            AllBooks.Clear();
            var imageManager = new ImageManager();
            var listBooks = bookRepository.FindAll();
            foreach (var bookEntity in listBooks)
            {
                var book = ConstructBookModel((BookEntity)bookEntity);
                if (!string.IsNullOrEmpty(book.image))
                {
                    book.BitmapImage = imageManager.Download(book.image);
                }
                AllBooks.Add(book);
            }
        }
        public void Update(Book book)
        {
            var bookEntity = new BookEntity
            {
                id = book.id,
                name = book.name,
                image = book.image
            };
            if (bookRepository.Update(bookEntity) == 0)
            {
                throw new Exception();
            }
        }
        public Book? FindByName(string name)
        {
            var bookEntity = bookRepository.FindByName(name);
            if (bookEntity == null) return null;

            return ConstructBookModel((BookEntity)bookEntity);

        }

        public Book FindById(int favoriteBook)
        {
            var bookEntity = bookRepository.FindById(favoriteBook);
            if (bookEntity == null) return null;

            return ConstructBookModel((BookEntity)bookEntity);
        }
        private Book ConstructBookModel(BookEntity bookEntity)
        {
            return new Book(bookEntity.id,
                          bookEntity.name,
                          bookEntity.image);
        }
        public Book ConstructNewBookModel()
        {
            return new Book(0, string.Empty, string.Empty);
        }
    }
}
