namespace SocialNetwork.BLL.Exceptions
{
    public class WrongPasswordException:Exception
    {
        public override string Message => "Пароль не корректный!";
    }
}
