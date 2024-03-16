namespace SocialNetwork.BLL.Exceptions
{
    public class BookExistException:Exception
    {
        public override string Message => "Такая книга уже есть";
    }
}
