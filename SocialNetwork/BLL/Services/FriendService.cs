using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.DAL.Repositories.Interfaces;

namespace SocialNetwork.BLL.Services
{
    public class FriendService
    {
        IFriendRepository friendRepository;
        IUserRepository userRepository;
        UserService userService;
        public FriendService()
        {
            friendRepository = new FriendRepository();
            userRepository = new UserRepository();  
            userService = new UserService();
        }
        public IEnumerable<User> FindAllByUserId(int userId)
        {
            var listUsers = new List<User>();
            friendRepository.FindAllByUserId(userId).ToList().ForEach(friendEntity =>
            {
                var user = userService.ConstructSimpleUserMode(userRepository.FindById(friendEntity.friend_id));
                listUsers.Add(user);
            });
            return listUsers;
        }

        public void AddToFriend(User user, User friend)
        {
            if(friendRepository.Create(ConstructFromUser(user,friend)) == 0)
                throw new Exception();
        }

        public void RemoveFromFriend(int id)
        {
            if(friendRepository.Delete(id) == 0)
                throw new Exception();
        }

        public int GetIdFriendByUserId(int userId, int friendId)
        {
             var id = friendRepository.FindAllByUserId(userId).Where(a => a.friend_id == friendId).Select(a => a.id)
                .FirstOrDefault();
            if (id == 0)
                throw new UserNotFoundException();
            return id;
        }

        public FriendEntity ConstructFromUser(User user, User friend)
        {
            return new FriendEntity(user.Id, friend.Id);
        }
    }
}
