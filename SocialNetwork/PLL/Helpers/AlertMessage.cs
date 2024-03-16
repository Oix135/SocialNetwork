using System.Windows;

namespace SocialNetwork.PLL.Helpers
{
    public class AlertMessage
    {
        public static void Show(string message)
        {
            MessageBox.Show(message, "Social Network", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
