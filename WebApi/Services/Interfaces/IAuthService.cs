using System.Threading.Tasks;

namespace WebApi.Services.Interfaces
{
    /// <summary>
    /// User authentication service interface.
    /// </summary>
    /// <typeparam name="T">Generic user type.</typeparam>
    public interface IAuthService<T> where T : class
    {
        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="username">User username.</param>
        /// <returns>Returns user object or null.</returns>
        Task<T> Authenticate(string username);
    }
}
