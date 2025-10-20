using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;


namespace GlossaryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User? GetUserByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public User? GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public void CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username cannot be empty");

            if (_userRepository.GetByUsername(user.Username) != null)
                throw new ArgumentException("Username already exists");

            if (_userRepository.GetByEmail(user.Email) != null)
                throw new ArgumentException("Email already exists");

            // Ovde možeš dodati hash lozinke ako treba
            _userRepository.Add(user);
            _userRepository.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existingUser = _userRepository.GetById(user.Id);
            if (existingUser == null)
                throw new ArgumentException("User not found");

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Role = user.Role;

            _userRepository.Update(existingUser);
            _userRepository.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new ArgumentException("User not found");

            _userRepository.Delete(user);
            _userRepository.SaveChanges();
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            return user != null && user.PasswordHash == password ? user : null;

        }
    }
}
