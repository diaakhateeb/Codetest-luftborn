using Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            //_appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepo.GetAll();
        }

        public async Task AddUser(User newUser)
        {
            await _userRepo.Add(newUser);
        }

        public async Task AddManyUsers(ICollection<User> entities)
        {
            await _userRepo.AddMany(entities);
        }

        public void UpdateUser(User user)
        {
            _userRepo.ReplaceOneSync(user.Id, user);
        }

        public async Task DeleteUser(string id)
        {
            await _userRepo.Delete(x => x.Id == id);
        }

        public async Task<User> FindUserById(string id)
        {
            return await _userRepo.GetSingleByExpression(x => x.Id == id);
        }

        public async Task<bool> IsExisted(string userName, string email)
        {
            var user = await _userRepo.GetSingleByExpression(
                x => x.UserName == userName && x.Email == email);
            return user != null;
        }
    }
}
