using Models;
using System.Collections.Generic;

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
        IEnumerable<User> GetAll();
        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="newUser">New user data object.</param>
        void AddUser(User newUser);
        /// <summary>
        /// Adds bulk of users.
        /// </summary>
        /// <param name="entities">Collection of user objects.</param>
        void AddManyUsers(ICollection<User> entities);
        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="user">User with new data.</param>
        void UpdateUser(User user);
        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="id">User Id to be deleted.</param>
        void DeleteUser(string id);
        /// <summary>
        /// Finds user by Id.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>Returns user object or null.</returns>
        User FindUserById(string id);
        /// <summary>
        /// Checks if user is existed.
        /// </summary>
        /// <param name="userName">User username.</param>
        /// <param name="email">User email.</param>
        /// <returns>Returns true if user existed. Otherwise, false.</returns>
        bool IsExisted(string userName, string email);

    }
}
