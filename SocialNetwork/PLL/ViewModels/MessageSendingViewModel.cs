using ReactiveUI;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using System.Reactive;

namespace SocialNetwork.PLL.ViewModels
{

    public class MessageSendingViewModel:BaseViewModel
    {
        UserService userService;
        MessageService messageService;

        public MessageSendingViewModel(MessageService messageService, UserService userService)
        {
            this.messageService = messageService;
            this.userService = userService;

            AllUsers = new List<User>();

            var cansend = this.WhenAnyValue(a => a.MessageSendingData.CanSending);

            SendMessageCommand = ReactiveCommand.Create(SendMessage, cansend);
        }

        #region Properies

        public MessageSendingData MessageSendingData { get; private set; }

        private User _recipient;

        private User user;
        public User Recipient
        {
            get => _recipient;
            set
            {
                this.RaiseAndSetIfChanged(ref _recipient, value);
                if(value != null)
                {
                    MessageSendingData.RecipientEmail = value.Email;
                }
            }
        }

        public List<User> AllUsers { get; }

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> SendMessageCommand { get; }
        #endregion

        #region Methods
        public void Show(User user)
        {
            this.user = user;
            userService.GetAll();

            AllUsers.Clear();
            AllUsers.AddRange(userService.GetAllExceptMe(user.Id));

            MessageSendingData = new MessageSendingData();
            this.RaisePropertyChanged(nameof(MessageSendingData));

            MessageSendingData.SenderId = user.Id;
        }

        private void SendMessage()
        {
            try
            {
                messageService.SendMessage(MessageSendingData);

                SuccessMessage.Show("Сообщение успешно отправлено!");

                user = userService.FindById(user.Id);

                MessageSendingData.Content = string.Empty;

                MainViewModel.Instance.UpdateMessagesInfo(user);
            }

            catch (UserNotFoundException)
            {
                AlertMessage.Show("Пользователь не найден!");
            }

            catch (ArgumentNullException)
            {
                AlertMessage.Show("Введите корректное значение!");
            }

            catch (Exception)
            {
                AlertMessage.Show("Произошла ошибка при отправке сообщения!");
            }

        }

        #endregion
    }
}
