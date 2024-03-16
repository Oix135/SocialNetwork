using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialNetwork.BLL.Models
{
    public class BaseModel: INotifyPropertyChanged
    {
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
