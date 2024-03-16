namespace SocialNetwork.BLL.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public override string Message => "Пользователь не найден!";
    }
}
