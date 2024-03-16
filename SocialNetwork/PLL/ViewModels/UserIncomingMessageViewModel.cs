using ReactiveUI;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;

namespace SocialNetwork.PLL.ViewModels
{
    public class UserIncomingMessageViewModel:BaseViewModel
    {
        private MessageService messageService;
        public UserIncomingMessageViewModel(MessageService messageService)
        {
            this.messageService = messageService;

            Messages = new List<PrettyMessage>();
        }

        public List<PrettyMessage> Messages { get; }

        private PrettyMessage _message;
        public PrettyMessage Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        internal void Show(IEnumerable<Message> incomingMessages, UserService userService)
        {
            Messages.Clear();
            incomingMessages.ToList().ForEach(message =>
            {
                Messages.Add(messageService.ConstructPrettyMessage(message, userService, true));
            });

            this.RaisePropertyChanged(nameof(Messages));
        }
    }
}
