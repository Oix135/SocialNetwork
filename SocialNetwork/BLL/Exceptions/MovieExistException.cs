namespace SocialNetwork.BLL.Exceptions
{
    public class MovieExistException:Exception
    {
        public override string Message => "Такой фильм уже есть";
    }
}
