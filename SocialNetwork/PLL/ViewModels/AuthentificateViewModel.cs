using ReactiveUI;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using SocialNetwork.PLL.View;
using System.Reactive;
using System.Windows;

namespace SocialNetwork.PLL.ViewModels
{
    public class AuthentificateViewModel:BaseViewModel
    {
        private UserService userService;
        public AuthentificateViewModel(UserService userService) 
        {
            this.userService = userService;

            var canRegister = this.WhenAnyValue(x => x.UserRegistrationData.CanRegister);

            AuthenticateCommand = ReactiveCommand.Create(Authenticate);
            OpenRegisterCommand = ReactiveCommand.Create(OpenRegister);
            RegisterCommand = ReactiveCommand.Create(Register, canRegister);
            CloseRegisterCommand = ReactiveCommand.Create(CloseRegister);
            ExitCommand = ReactiveCommand.Create(Exit);
        }

        

        #region Properties

        

        private UserAuthenticationData _userAuthenticationData;
        public UserAuthenticationData UserAuthenticationData
        {
            get => _userAuthenticationData;
            set => this.RaiseAndSetIfChanged(ref _userAuthenticationData, value);
        }

        private UserRegistrationData _userRegistrationData;
        public UserRegistrationData UserRegistrationData
        {
            get => _userRegistrationData;
            set => this.RaiseAndSetIfChanged(ref _userRegistrationData, value);
        }

        private bool _registerVisible;
        public bool RegisterVisible
        {
            get => _registerVisible;
            set => this.RaiseAndSetIfChanged(ref _registerVisible, value);
        }

        private bool _authorizeVisible;
        public bool AuthorizeVisible
        {
            get => _authorizeVisible;
            set => this.RaiseAndSetIfChanged(ref _authorizeVisible, value);
        }

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> AuthenticateCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseRegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        #endregion

        #region Methods

        public void Show()
        {
            UserAuthenticationData = new UserAuthenticationData();

            AuthorizeVisible = true;
            RegisterVisible = false;

            MainViewModel.Instance.ContentControl = new AuthorizeView { DataContext = this };
        }

        private void OpenRegister()
        {
            UserRegistrationData = new UserRegistrationData();
            RegisterVisible = true;
            AuthorizeVisible = false;
        }

        private void Authenticate()
        {
            try
            {
                var user = userService.Authenticate(UserAuthenticationData);
               
                MainViewModel.Instance.ShowUserMenu(user);
            }
            catch (WrongPasswordException ex)
            {
                AlertMessage.Show(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                AlertMessage.Show(ex.Message);
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }

        }

        private void Register()
        {
            try
            {
                userService.Register(UserRegistrationData);
                CloseRegister();
                SuccessMessage.Show($"Пользователь {UserRegistrationData.FirstName} {UserRegistrationData.LastName} зарегистрирован!");
            }
            catch (ArgumentNullException ex)
            {
                AlertMessage.Show(ex.Message);
            }
            catch (Exception ex)
            {
                AlertMessage.Show(ex.Message);
            }

        }
        private void CloseRegister()
        {
            RegisterVisible = false;
            AuthorizeVisible = true;
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
