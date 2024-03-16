using Microsoft.Xaml.Behaviors;
using System.Net;
using System.Security;
using System.Windows.Controls;
using System.Windows;
using SocialNetwork.BLL.Managers;

namespace SocialNetwork.PLL.CustomControls
{
    public class PasswordBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(PasswordBehavior), new PropertyMetadata(default(SecureString)));

        private bool _skipUpdate;

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += PasswordBox_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= PasswordBox_PasswordChanged;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == PasswordProperty && AssociatedObject is not null)
            {
                if (!_skipUpdate)
                {
                    _skipUpdate = true;
                    AssociatedObject.Password = new NetworkCredential(string.Empty, e.NewValue as SecureString).Password; //e.NewValue as string;
                    _skipUpdate = false;
                }
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _skipUpdate = true;
            Password = new NetworkCredential(string.Empty, AssociatedObject.Password).SecurePassword;
            _skipUpdate = false;
        }
    }
}
