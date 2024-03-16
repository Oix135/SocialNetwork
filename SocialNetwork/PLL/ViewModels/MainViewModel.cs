using ReactiveUI;
using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using System.Reactive;
using System.Windows;

namespace SocialNetwork.PLL.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance;

        public static UserService UserService = new UserService();
        public static BookService BookService = new BookService();
        public static MovieService MovieService = new MovieService();
        public static MessageService MessageService = new MessageService();
        public static FriendService FriendService = new FriendService();


        public static UserViewModel UserViewModel;
        public static BookViewModel BookViewModel;
        public static MovieViewModel MovieViewModel;
        public static AuthentificateViewModel AuthentificateViewModel;
        public static MessageViewModel MessageViewModel;
        public static MessageSendingViewModel MessageSendingViewModel;
        public static UserIncomingMessageViewModel UserIncomingMessageViewModel;
        public static UserOutgoingMessageViewModel UserOutgoingMessageViewModel;
        public static FriendsViewModel FriendsViewModel;

        public MainViewModel()
        {
            if (Instance == null)
                Instance = this;

            UserMenuViewModel = new UserMenuViewModel();
            AuthentificateViewModel = new AuthentificateViewModel(UserService);
            UserViewModel = new UserViewModel(UserService, BookService, MovieService);
            BookViewModel = new BookViewModel(BookService);
            MovieViewModel = new MovieViewModel(MovieService);
            MessageViewModel = new MessageViewModel(UserService);
            MessageSendingViewModel = new MessageSendingViewModel(MessageService, UserService);
            UserIncomingMessageViewModel = new UserIncomingMessageViewModel(MessageService);
            UserOutgoingMessageViewModel = new UserOutgoingMessageViewModel(MessageService);
            FriendsViewModel = new FriendsViewModel(UserService, FriendService);

            AuthentificateViewModel.Show();

        }

        #region Properties

        public UserMenuViewModel UserMenuViewModel { get; }


        private FrameworkElement _contentControl;
        public FrameworkElement ContentControl
        {
            get => _contentControl;
            set => this.RaiseAndSetIfChanged(ref _contentControl, value);
        }

        private FrameworkElement _baseContentControl;
        public FrameworkElement BaseContentControl
        {
            get => _baseContentControl;
            set => this.RaiseAndSetIfChanged(ref _baseContentControl, value);
        }

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                this.RaiseAndSetIfChanged(ref _user, value);
                RefreshUser();
            }
        }
        public string Fio { get => $"{User.FirstName} {User.LastName}"; }

        #endregion

        #region Commands

        
       

        #endregion

        #region Methods

       

        private void UserUpdated()
        {
            if (User != null)
            {
                this.RaisePropertyChanged(nameof(Fio));
            }
        }

        internal void RefreshUser()
        {
            if (User != null && !string.IsNullOrEmpty(User.Photo))
            {
               User.BitmapImage = ImageManager.GetPhoto(User.Photo);
            }
            UserUpdated();
        }

        internal void ShowUserMenu(User user)
        {
            User = user;
            UserMenuViewModel.Show(User);
            
        }

        internal void UpdateMessagesInfo(User user)
        {
            UserMenuViewModel.UpdateMessagesInfo(user);
            MessageViewModel.UpdateMessagesInfo(user);
        }
    }

    #endregion
}
