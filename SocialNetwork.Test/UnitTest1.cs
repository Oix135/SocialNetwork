using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.Test
{
    public class Tests
    {

        [Test]
        public void CheckConnectionTest()
        {
            BaseRepository baseRepository = new BaseRepository();   

            var connection = baseRepository.CreateConnection();

            connection.Open();

            Assert.AreEqual(connection.State, System.Data.ConnectionState.Open);

        }
    }
}