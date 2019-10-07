using Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    /// <summary>
    /// User authentication service class.
    /// </summary>
    /// <typeparam name="T">User generic type.</typeparam>
    public class AuthService<T> : IAuthService<T> where T : class
    {
        private readonly IUserRepository _userRepo;
        /// <summary>
        /// Creates instance of AuthService class.
        /// </summary>
        /// <param name="userRepo">User repository object.</param>
        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public T Authenticate(string username)
        {
            var user = _userRepo.GetSingleByExpression(u => u.UserName == username).Result;

            return user as T;
        }
    }
}
