using SocialNetwork.DAL.Entities.Interfaces;

namespace SocialNetwork.DAL.Entities
{
    public abstract class HobbieEntity : IHobbieEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }
}
