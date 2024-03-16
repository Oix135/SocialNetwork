using SocialNetwork.DAL.Entities.Interfaces;

namespace SocialNetwork.DAL.Repositories.Interfaces
{
    public interface IHobbieRepository<out T>
    {
        int Create(IHobbieEntity hobbieEntity);
        IEnumerable<IHobbieEntity> FindAll();
        IHobbieEntity FindByName(string name);
        IHobbieEntity FindById(int id);
        int Update(IHobbieEntity hobbieEntity);
        int DeleteById(int id);
    }
}
