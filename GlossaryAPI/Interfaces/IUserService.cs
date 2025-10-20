using GlossaryAPI.Models;

namespace GlossaryAPI.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByEmail(string email);
        User? GetUserByUsername(string username);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        User Authenticate(string username, string password);
    }
}
