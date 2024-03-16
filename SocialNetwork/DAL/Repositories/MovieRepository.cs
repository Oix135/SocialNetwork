using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Entities.Interfaces;
using SocialNetwork.DAL.Repositories.Interfaces;

namespace SocialNetwork.DAL.Repositories
{
    internal class MovieRepository<T> : BaseRepository, IHobbieRepository<T> where T : MovieEntity
    {
        public int Create(IHobbieEntity bookEntity)
        {
            return Execute(@"insert into movies (name,image) 
                             values(:name,:image)", bookEntity);
        }

        public int DeleteById(int id)
        {
            return Execute(@"delete from movies where id = :id_p", new { id_p = id });
        }

        public IEnumerable<IHobbieEntity> FindAll()
        {
            return Query<MovieEntity>("select * from movies");
        }

        public IHobbieEntity FindById(int id)
        {
            return QueryFirstOrDefault<MovieEntity>("select * from movies where id = :id_p", new { id_p = id });
        }

        public IHobbieEntity FindByName(string name)
        {
            return QueryFirstOrDefault<MovieEntity>("select * from movies where name = :name_p", new { name_p = name });
        }

        public int Update(IHobbieEntity bookEntity)
        {
            return Execute(@"update movies set name = :name, image = :image where id = :id", bookEntity);
        }
    }
}
