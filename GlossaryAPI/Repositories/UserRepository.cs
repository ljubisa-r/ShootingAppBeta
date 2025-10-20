using GlossaryAPI.Data;
using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;

namespace GlossaryAPI.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly GlossaryDbContext _context;    
        public UserRepository(GlossaryDbContext context)
        {
            _context = context;
        }
   
        public IQueryable<User> GetAll() => _context.Users;
        public User? GetById(int id) => _context.Users.FirstOrDefault(x => x.Id == id);
        public User? GetByEmail(string email) =>  _context.Users.FirstOrDefault(x => x.Email == email);     
        public User? GetByUsername(string username) => _context.Users.FirstOrDefault(x => x.Username == username);
       
        public void Add(User user) => _context.Users.Add(user);
        public void Update(User user) => _context.Users.Update(user);
        public void Delete(User user) => _context.Users.Remove(user);
        public void SaveChanges() => _context.SaveChanges();
    }
}
