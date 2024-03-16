using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Entities.Interfaces;
using SocialNetwork.DAL.Repositories.Interfaces;

namespace SocialNetwork.DAL.Repositories
{
    internal class BookRepository<T> : BaseRepository, IHobbieRepository<T> where T : BookEntity
    {
        public int Create(IHobbieEntity bookEntity)
        {
            return Execute(@"insert into books (name, image) 
                             values(:name,:image)", bookEntity);
        }

        public int DeleteById(int id)
        {
            return Execute(@"delete from books where id = :id_p", new { id_p = id });
        }

        public IEnumerable<IHobbieEntity> FindAll()
        {
            return Query<BookEntity>("select * from books");
        }

        public IHobbieEntity FindById(int id)
        {
            return QueryFirstOrDefault<BookEntity>("select * from books where id = :id_p", new { id_p = id });
        }

        public IHobbieEntity FindByName(string name)
        {
            return QueryFirstOrDefault<BookEntity>("select * from books where name = :name_p", new { name_p = name });
        }

        public int Update(IHobbieEntity bookEntity)
        {
            return Execute(@"update books set name = :name, image = :image where id = :id", bookEntity);
        }
    }
}
