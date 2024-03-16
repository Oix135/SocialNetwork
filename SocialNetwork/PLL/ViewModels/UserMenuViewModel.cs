using ReactiveUI;
using SocialNetwork.BLL.Models;
using SocialNetwork.PLL.View;
using System.Reactive;

namespace SocialNetwork.PLL.ViewModels
{
    public class UserMenuViewModel:BaseViewModel
    {
        private User user;
        public UserMenuViewModel() 
        {
            OpenProfileInfoCommand = ReactiveCommand.Create(OpenProfileInfo);
            OpenEditProfileCommand = ReactiveCommand.Create(OpenEditProfile);
            OpenBooksEditCommand = ReactiveCommand.Create(OpenBooksEdit);
            OpenMoviesEditCommand = ReactiveCommand.Create(OpenMoviesEdit);
            OpenMessagesCommand = ReactiveCommand.Create(OpenMessages);
            OpenFriendsCommand = ReactiveCommand.Create(OpenFriends);

            UnLoginCommand = ReactiveCommand.Create(UnLogin);
        }

        #region Properties

        public int IncommingMessageCount => user.IncomingMessages.Count();
        public int OutgoingMessageCount => user.OutgoingMessages.Count();

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> OpenProfileInfoCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenEditProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenBooksEditCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenMoviesEditCommand { get; }
        public ReactiveCommand<Unit,Unit> OpenMessagesCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFriendsCommand { get; }


        public ReactiveCommand<Unit, Unit> UnLoginCommand { get; }

        #endregion

        #region Methods

        public void Show(User user)
        {
            UpdateMessagesInfo(user);
            MainViewModel.Instance.ContentControl = new BasicView();
        }


        private void OpenProfileInfo()
        {
           MainViewModel.UserViewModel.ShowProfile(user);
        }

        private void OpenEditProfile()
        {
            MainViewModel.UserViewModel.ShowEditProfile(user);
        }

        private void OpenBooksEdit()
        {

            MainViewModel.BookViewModel.Show();
        }

        private void OpenMoviesEdit()
        {
            MainViewModel.MovieViewModel.Show();
        }

        private void OpenMessages()
        {
            MainViewModel.MessageViewModel.Show(user);
        }

        private void OpenFriends()
        {
            MainViewModel.FriendsViewModel.Show(user);
        }

        private void UnLogin()
        {
            MainViewModel.Instance.User = null;
            MainViewModel.Instance.BaseContentControl = null;
            MainViewModel.Instance.ContentControl = null;
            MainViewModel.AuthentificateViewModel.Show();
        }

        internal void UpdateMessagesInfo(User user)
        {
            this.user = user;
            this.RaisePropertyChanged(nameof(IncommingMessageCount));
            this.RaisePropertyChanged(nameof(OutgoingMessageCount));
        }

        #endregion
    }
}
