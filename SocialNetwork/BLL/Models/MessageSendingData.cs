namespace SocialNetwork.BLL.Models
{
    public class MessageSendingData:BaseModel
    {
        private int _senderId;
        public int SenderId
        {
            get => _senderId;
            set
            {
                _senderId = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanSending));
            }
        }
        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanSending));
            }
        }
        private string _recipientEmail;
        public string RecipientEmail
        {
            get=>_recipientEmail;
            set
            {
                _recipientEmail = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanSending));
            }
        }

        public bool CanSending
        {
            get => SenderId != 0 
                && !string.IsNullOrEmpty(RecipientEmail) 
                && !string.IsNullOrEmpty(Content);
        }
    }
}
