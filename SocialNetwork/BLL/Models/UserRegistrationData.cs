using ReactiveUI;
using SocialNetwork.PLL.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;

namespace SocialNetwork.BLL.Models
{
    public class UserRegistrationData:BaseViewModel
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                this.RaiseAndSetIfChanged(ref _firstName, value);
                this.RaisePropertyChanged(nameof(CanRegister));
            }
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                this.RaiseAndSetIfChanged(ref _lastName, value);
                this.RaisePropertyChanged(nameof(CanRegister));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                this.RaiseAndSetIfChanged(ref _email, value);
                this.RaisePropertyChanged(nameof(CanRegister));
            }
        }
        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                this.RaiseAndSetIfChanged(ref _password, value);
                this.RaisePropertyChanged(nameof(CanRegister));
            }
        }
        public bool CanRegister
        {
            get => 
                !string.IsNullOrEmpty(FirstName) &&
                !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(Email) &&
                Password != null &&
                new NetworkCredential(string.Empty, Password).Password.Length >= 8 &&
                new EmailAddressAttribute().IsValid(Email);
        }

    }
}
