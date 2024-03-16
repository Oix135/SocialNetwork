using ReactiveUI;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.View;
using System.Reactive;
using System.Windows;

namespace SocialNetwork.PLL.ViewModels
{
    public class MessageViewModel:BaseViewModel
    {
        private User user;
        private UserService userService;
        public MessageViewModel(UserService userService) 
        {
            this.userService = userService;

            OpenSendingMessageCommand = ReactiveCommand.Create(OpenSendingMessage);
            OpenIncommingMessagesCommand = ReactiveCommand.Create(OpenIncommingMessages);
            OpenOutgoingMessagesCommand = ReactiveCommand.Create(OpenOutgoingMessages);
        }


        #region Properties
        public int IncommingMessageCount => user.IncomingMessages.Count();
        public int OutgoingMessageCount => user.OutgoingMessages.Count();

        private FrameworkElement _contentControl;
        public FrameworkElement ContentControl
        {
            get => _contentControl;
            set => this.RaiseAndSetIfChanged(ref _contentControl, value);
        }
        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> OpenSendingMessageCommand { get; }
        public ReactiveCommand<Unit,Unit> OpenIncommingMessagesCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenOutgoingMessagesCommand { get; }

        #endregion

        #region Methods

        internal void Show(User user)
        {
            ContentControl = null;
            UpdateMessagesInfo(user);

            MainViewModel.Instance.BaseContentControl = new MessagesView { DataContext = this };
        }

        private void OpenSendingMessage()
        {
            MainViewModel.MessageSendingViewModel.Show(user);
            ContentControl = new SendMessageView { DataContext = MainViewModel.MessageSendingViewModel };
        }
        
        private void OpenIncommingMessages()
        {
            MainViewModel.UserIncomingMessageViewModel.Show(user.IncomingMessages, userService);
            ContentControl = new IncommingMessagesView { DataContext = MainViewModel.UserIncomingMessageViewModel };
        }
        private void OpenOutgoingMessages()
        {
            MainViewModel.UserOutgoingMessageViewModel.Show(user.OutgoingMessages, userService);
            ContentControl = new OutgoingMessageView { DataContext = MainViewModel.UserOutgoingMessageViewModel };
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
