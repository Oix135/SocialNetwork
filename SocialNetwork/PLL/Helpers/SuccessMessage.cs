using System.Windows;

namespace SocialNetwork.PLL.Helpers
{
    internal class SuccessMessage
    {
        public static void Show(string message)
        {
            MessageBox.Show(message, "Social Network", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
