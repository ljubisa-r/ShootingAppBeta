using GlossaryAPI.Models;

namespace GlossaryAPI.Interfaces
{
    public interface IUserRepository
    {
        User? GetById(int id);
        User? GetByUsername(string username);
        User? GetByEmail(string email);
        IQueryable<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        void SaveChanges();
    }
}
