namespace SocialNetwork.DAL.Entities
{
    public class FriendEntity
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int friend_id { get; set; }

        public FriendEntity() { }
        public FriendEntity(int user_id, int friend_id)
        {
            this.user_id = user_id; 
            this.friend_id = friend_id;
        }
        
    }
}
