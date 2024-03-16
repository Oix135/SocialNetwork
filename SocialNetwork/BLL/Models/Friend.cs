namespace SocialNetwork.BLL.Models
{
    public class Friend
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int friend_id { get; set; }

        public Friend(int id, int user_id, int friend_id)
        {
            this.id = id;
            this.user_id = user_id;
            this.friend_id = friend_id;
        }
    }
}
