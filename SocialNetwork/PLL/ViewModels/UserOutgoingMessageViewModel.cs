using ReactiveUI;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;

namespace SocialNetwork.PLL.ViewModels
{
    public class UserOutgoingMessageViewModel: BaseViewModel
    {
        private MessageService messageService;
        public UserOutgoingMessageViewModel(MessageService messageService)
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

        internal void Show(IEnumerable<Message> outgoingMessages, UserService userService)
        {
            Messages.Clear();
            outgoingMessages.ToList().ForEach(message =>
            {
                Messages.Add(messageService.ConstructPrettyMessage(message, userService, false));
            });

            this.RaisePropertyChanged(nameof(Messages));
        }
    }
    
}
