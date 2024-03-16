using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Managers;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SocialNetwork.BLL.Services
{
    public class UserService
    {
        MessageService messageService;
        IUserRepository userRepository;
        
        public UserService()
        {
            userRepository = new UserRepository();
            messageService = new MessageService();

        }

        public void Register(UserRegistrationData userRegistrationData)
        {
            CheckValidData(userRegistrationData);

            var userEntity = new UserEntity
            {
                firstname = userRegistrationData.FirstName,
                lastname = userRegistrationData.LastName,
                password = PwdEncryptor.GetPassword(new NetworkCredential(string.Empty, userRegistrationData.Password).Password),
                email = userRegistrationData.Email
            };

            if (userRepository.Create(userEntity) == 0)
                throw new Exception();

        }

        private void CheckValidData(UserRegistrationData userRegistrationData)
        {
            if (string.IsNullOrEmpty(userRegistrationData.FirstName))
                throw new ArgumentNullException(nameof(userRegistrationData.FirstName));

            if (string.IsNullOrEmpty(userRegistrationData.LastName))
                throw new ArgumentNullException(nameof(userRegistrationData.LastName));

            if (string.IsNullOrEmpty(new NetworkCredential(string.Empty, userRegistrationData.Password).Password))
                throw new ArgumentNullException(nameof(userRegistrationData.Password));

            if (string.IsNullOrEmpty(userRegistrationData.Email))
                throw new ArgumentNullException(nameof(userRegistrationData.Email));

            if (userRegistrationData.Password.Length < 8)
                throw new ArgumentNullException(nameof(userRegistrationData.Password));

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new ArgumentNullException(nameof(userRegistrationData.Email));

            if (userRepository.FindByEmail(userRegistrationData.Email) != null)
                throw new ArgumentNullException(nameof(userRegistrationData.Email));
        }

        private void CheckValidData(User userRegistrationData)
        {
            if (string.IsNullOrEmpty(userRegistrationData.FirstName))
                throw new ArgumentNullException(nameof(userRegistrationData.FirstName));

            if (string.IsNullOrEmpty(userRegistrationData.LastName))
                throw new ArgumentNullException(nameof(userRegistrationData.LastName));

            if (string.IsNullOrEmpty(userRegistrationData.StringPassword))
                throw new ArgumentNullException(nameof(userRegistrationData.Password));

            if (string.IsNullOrEmpty(userRegistrationData.Email))
                throw new ArgumentNullException(nameof(userRegistrationData.Email));

            if (userRegistrationData.StringPassword.Length < 8)
                throw new ArgumentNullException(nameof(userRegistrationData.Password));

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new ArgumentNullException(nameof(userRegistrationData.Email));

            var findedUser = userRepository.FindByEmail(userRegistrationData.Email);
            if (findedUser != null && findedUser.id != userRegistrationData.Id)
                throw new ArgumentNullException(nameof(userRegistrationData.Email));
        }

        public User Authenticate(UserAuthenticationData userAuthenticationData)
        {
            var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email);
            if (findUserEntity is null) throw new UserNotFoundException();

            if (findUserEntity.password != PwdEncryptor.GetPassword(new NetworkCredential(string.Empty, userAuthenticationData.Password).Password))
                throw new WrongPasswordException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindByEmail(string email)
        {
            var findUserEntity = userRepository.FindByEmail(email);
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindById(int id)
        {
            var findUserEntity = userRepository.FindById(id);
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public IEnumerable<User> GetAll()
        {
           var listUsers = new List<User>();
            var imageManager = new ImageManager();
            userRepository.FindAll().ToList().ForEach(userEntity =>
            {
                var user = ConstructUserModel(userEntity);
                if (!string.IsNullOrEmpty(user.Photo))
                {
                    user.BitmapImage = imageManager.Download(user.Photo);
                }
                listUsers.Add(user);
            });
            return listUsers;
        }
        public IEnumerable<User> GetAllExceptMe(int id)
        {
            var listUsers = new List<User>();
            var imageManager = new ImageManager();
            userRepository.FindAll().ToList().ForEach(userEntity =>
            {
                if (userEntity.id != id)
                {
                    var user = ConstructSimpleUserMode(userEntity);
                    listUsers.Add(user);
                }
            });
            return listUsers;
        }
        public IEnumerable<User> GetAllExceptFriens(int id, int[] friendsIds)
        {
            var listUsers = new List<User>();
            var imageManager = new ImageManager();
            userRepository.FindAll().ToList().ForEach(userEntity =>
            {
                if (userEntity.id != id && !friendsIds.Contains(userEntity.id))
                {
                    var user = ConstructSimpleUserMode(userEntity);
                    listUsers.Add(user);
                }
            });
            return listUsers;
        }

        public void Update(User user)
        {
            
            if(user.Password != null)
            {
                user.StringPassword = PwdEncryptor.GetPassword(new NetworkCredential(string.Empty, user.Password).Password);
            }

            CheckValidData(user);

            var updatableUserEntity = new UserEntity()
            {
                id = user.Id,
                firstname = user.FirstName,
                lastname = user.LastName,
                password = user.StringPassword,
                email = user.Email,
                photo = user.Photo,
                favorite_movie = user.FavoriteMovie,
                favorite_book = user.FavoriteBook
            };

            if (userRepository.Update(updatableUserEntity) == 0)
                throw new Exception();
        }

        private User ConstructUserModel(UserEntity userEntity)
        {
            var incomingMessages = messageService.GetIncomingMessagesByUserId(userEntity.id);

            var outgoingMessages = messageService.GetOutcomingMessagesByUserId(userEntity.id);

            return new User(userEntity.id,
                          userEntity.firstname,
                          userEntity.lastname,
                          new NetworkCredential(string.Empty, userEntity.password).SecurePassword,
                          userEntity.email,
                          userEntity.photo,
                          userEntity.favorite_movie,
                          userEntity.favorite_book,
                          incomingMessages,
                          outgoingMessages);
        }

        public User ConstructSimpleUserMode(UserEntity userEntity)
        {
            return new User(
                userEntity.id, 
                userEntity.firstname, 
                userEntity.lastname, 
                userEntity.email, 
                userEntity.photo, 
                userEntity.favorite_movie, 
                userEntity.favorite_book
                );
        }

    }
}
