using DynamicData;
using ReactiveUI;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using SocialNetwork.PLL.View;
using System.Collections.ObjectModel;
using System.Reactive;

namespace SocialNetwork.PLL.ViewModels
{
    public class FriendsViewModel:BaseViewModel
    {
        private UserService userService;
        private FriendService friendService;

        private User user;

        public FriendsViewModel(UserService userService, FriendService friendService) 
        {
            this.userService = userService;
            this.friendService = friendService;

            AllUsers = new ObservableCollection<User>();
            AllFriends = new ObservableCollection<User>();

            var canAddToFriends = this.WhenAnyValue(a => a.CanAddToFriends);
            var canRemoveFromFriends = this.WhenAnyValue(a => a.CanRemoveFromFriends);

            AddToFriendsCommand = ReactiveCommand.Create(AddToFriends, canAddToFriends);
            RemoveFromFriendsCommand = ReactiveCommand.Create(RemoveFromFriends, canRemoveFromFriends);
        }

        #region Properties

        public ObservableCollection<User> AllUsers { get; }

        public ObservableCollection<User> AllFriends { get; }
       

        public bool CanAddToFriends => SelectedUser != null;

        public bool CanRemoveFromFriends => Friend != null;

        private User friend;
        public User Friend
        {
            get => friend;
            set
            {
                this.RaiseAndSetIfChanged(ref friend, value);
                this.RaisePropertyChanged(nameof(CanRemoveFromFriends));
            }
        }

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedUser, value);
                this.RaisePropertyChanged(nameof(CanAddToFriends));
            }
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddToFriendsCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveFromFriendsCommand { get; }

        #endregion

        #region Methods

        public void Show(User user)
        {
            this.user = user;

            RefreshFriends();

            MainViewModel.Instance.BaseContentControl = new FriendsView { DataContext = this };
        }

        private void RefreshFriends()
        {
            GetAllFriends();

            GetAllUsers();

            this.RaisePropertyChanged(nameof(AllUsers));
            this.RaisePropertyChanged(nameof(AllFriends));
        }

        private void AddToFriends()
        {
            try
            {
                friendService.AddToFriend(user, SelectedUser);
                SuccessMessage.Show($"{SelectedUser.FirstName} {SelectedUser.LastName} теперь у Вас в друзьях");

                RefreshFriends();
            }
            catch(Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }
        }

        private void RemoveFromFriends()
        {
            try
            {
                var idFriend = friendService.GetIdFriendByUserId(user.Id, Friend.Id);

                friendService.RemoveFromFriend(idFriend);
                SuccessMessage.Show($"{Friend.FirstName} {Friend.LastName} больше не в друзьях");

                RefreshFriends();
            }
            catch(Exception ex )
            {
                AlertMessage.Show(ex.Message);
            }
        }

        private void GetAllUsers()
        {
            AllUsers.Clear();
            AllUsers.AddRange(userService.GetAllExceptFriens(user.Id, AllFriends.Select(a => a.Id)
                                                                                .ToArray()));
        }

        private void GetAllFriends()
        {
            AllFriends.Clear();
            AllFriends.AddRange(friendService.FindAllByUserId(user.Id));
        }

        #endregion
    }
}
