using Models;
using Repository.Interfaces;
using System.Collections.Generic;
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

        public IEnumerable<User> GetAll()
        {
            return _userRepo.GetAll().Result;
        }

        public void AddUser(User newUser)
        {
            _userRepo.Add(newUser).Wait();
        }

        public void AddManyUsers(ICollection<User> entities)
        {
            _userRepo.AddMany(entities as ICollection<User>).Wait();
        }

        public void UpdateUser(User user)
        {
            _userRepo.ReplaceOneSync(user.Id, user);
        }

        public void DeleteUser(string id)
        {
            _userRepo.Delete(x => x.Id == id).Wait();
        }

        public User FindUserById(string id)
        {
            return _userRepo.GetSingleByExpression(x => x.Id == id).Result;
        }

        public bool IsExisted(string userName, string email)
        {
            var user = _userRepo.GetByExpression(
                x => x.UserName == userName && x.Email == email)
                .Result;
            return user != null;
        }
    }
}
