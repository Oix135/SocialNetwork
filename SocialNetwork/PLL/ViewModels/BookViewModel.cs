using ReactiveUI;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using SocialNetwork.PLL.View;
using System.Reactive;

namespace SocialNetwork.PLL.ViewModels
{
    public class BookViewModel:BaseViewModel
    {
        private BookService bookService;
        public BookViewModel(BookService bookService) 
        {
            this.bookService = bookService;
            var canAddBook = this.WhenAnyValue(x => x.Book.CanAddBook);

            AllBooks = new List<Book>();
           
            
            UploadCoverBookCommand = ReactiveCommand.Create(UploadCoverBook);
            AddBookCommand = ReactiveCommand.Create(AddBook, canAddBook);
            EditBookCommand = ReactiveCommand.Create(EditBook, canAddBook);
            OpenEditBookCommad = ReactiveCommand.Create<bool>(OpenEditBook);
        }

        

        #region Properies

        private Book _book;
        public Book Book
        {
            get => _book;
            set
            {
                this.RaiseAndSetIfChanged(ref _book, value);
                this.RaisePropertyChanged(nameof(AddBookVisible));
                this.RaisePropertyChanged(nameof(EditBookVisible));
            }
        }
        public bool AddBookVisible
        {
            get => Book?.id == 0;
        }
        public bool EditBookVisible
        {
            get => Book?.id > 0;
        }
        public List<Book> AllBooks { get; }

        #endregion

        #region Commands
       
        public ReactiveCommand<Unit, Unit> UploadCoverBookCommand { get; }
        public ReactiveCommand<Unit, Unit> AddBookCommand { get; }
        public ReactiveCommand<Unit, Unit> EditBookCommand { get; }
        public ReactiveCommand<bool, Unit> OpenEditBookCommad { get; }
        #endregion


        #region Methods
        public void Show()
        {
            GetAllBoks();
            MainViewModel.Instance.BaseContentControl = new BooksView { DataContext = this };
        }
        public void OpenEditBook(bool edit)
        {
            if (!edit)
            {
                Book = bookService.ConstructNewBookModel();
            }
            MainViewModel.Instance.BaseContentControl = new EditBooksView { DataContext = this };
        }
        
        private void UploadCoverBook()
        {
            var imageManager = new ImageManager();
            var image = imageManager.Upload();
            if (!string.IsNullOrEmpty(image))
            {
                Book.image = image;
                Book.BitmapImage = ImageManager.GetPhoto(image);
            }
        }
        private void AddBook()
        {
            try
            {
                var findedBook = bookService.FindByName(Book.name);
                if (findedBook != null)
                {
                    Book = findedBook;
                    if (!string.IsNullOrEmpty(Book.image))
                    {
                        Book.BitmapImage = ImageManager.GetPhoto(Book.image);
                    }
                    throw new BookExistException();
                }
                else
                {
                    bookService.Create(Book);
                    SuccessMessage.Show($"Книга {Book.name} добавлена!");
                    Book = bookService.ConstructNewBookModel();
                }
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }
        }
        private void EditBook()
        {
            try
            {
                bookService.Update(Book);
                SuccessMessage.Show($"Книга {Book.name} отредактирована!");
                Book = bookService.ConstructNewBookModel();
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }
        }
        private void GetAllBoks()
        {
            bookService.GetAll();
            AllBooks.Clear();
            AllBooks.AddRange(bookService.AllBooks);
            this.RaisePropertyChanged(nameof(AllBooks));
        }

        #endregion
    }
}
