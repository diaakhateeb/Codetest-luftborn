using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services.Interfaces
{
    /// <summary>
    /// User service interface.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Returns collection of exised users.</returns>
        Task<IEnumerable<User>> GetAll();
        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="newUser">New user data object.</param>
        Task AddUser(User newUser);
        /// <summary>
        /// Adds bulk of users.
        /// </summary>
        /// <param name="entities">Collection of user objects.</param>
        Task AddManyUsers(ICollection<User> entities);
        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="user">User with new data.</param>
        void UpdateUser(User user);
        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="id">User Id to be deleted.</param>
        Task DeleteUser(string id);
        /// <summary>
        /// Finds user by Id.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>Returns user object or null.</returns>
        Task<User> FindUserById(string id);
        /// <summary>
        /// Checks if user is existed.
        /// </summary>
        /// <param name="userName">User username.</param>
        /// <param name="email">User email.</param>
        /// <returns>Returns true if user existed. Otherwise, false.</returns>
        Task<bool> IsExisted(string userName, string email);

    }
}
