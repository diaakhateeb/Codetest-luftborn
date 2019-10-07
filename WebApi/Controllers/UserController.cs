using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Services.Interfaces;
using WebApi.Specifications;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(path: "log/codetest.log",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Returns all users in Json format.</returns>
        [HttpGet]
        public JsonResult GetAll()
        {
            var users = _userService.GetAll();
            return new JsonResult(users);
        }

        /// <summary>
        /// Finds user by Id.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>Returns user object in Json format.</returns>
        [HttpGet]
        public JsonResult GetUserById(string id)
        {
            var user = _userService.FindUserById(id);
            return Json(user);
        }

        /// <summary>
        /// Inserts new user.
        /// </summary>
        /// <param name="user">New user object data.</param>
        /// <returns>Returns JsonResult object with success flag.</returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPost]
        public JsonResult Add(JObject user)
        {
            try
            {
                var newUser = JsonConvert.DeserializeObject<User>(user.ToString());

                if (!new UserSpecification().IsSatisfiedBy(newUser))
                {
                    return new JsonResult(new { success = false, responseText = "User is not valid." });
                }

                if (!_userService.IsExisted(newUser.UserName, newUser.Email))
                {
                    return new JsonResult(new
                    { success = false, responseText = "User email or username already taken." });
                }

                _userService.AddUser(newUser);
                return new JsonResult(new { success = true, responseText = "User successfully added!" });
            }
            catch (JsonException jsonExp)
            {
                Log.Error(jsonExp, "UserController.Add");
                return new JsonResult(new { success = false, responseText = "Failed to add user." });
            }
            catch (Exception exp)
            {
                Log.Error(exp, "UserController.Add");
                return new JsonResult(new { success = false, responseText = "Failed to add user." });
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Inserts bulk of users.
        /// </summary>
        /// <param name="users">List of users to be inserted.</param>
        /// <returns>Returns JsonResult object with success flag.</returns>
        [HttpPost]
        public JsonResult AddMany(ICollection<User> users)
        {
            var satisfiedUsersList = new List<User>();

            users.AsParallel().ForAll(x =>
            {
                if (_userService.FindUserById(x.Id) == null)
                {
                    if (new UserSpecification().IsSatisfiedBy(x))
                        satisfiedUsersList.Add(x);
                }
            });

            _userService.AddManyUsers(satisfiedUsersList);
            return new JsonResult(new { success = true, responseText = "Users successfully added!" });
        }

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="user">User data in Json format.</param>
        /// <returns>Returns JsonResult object with success flag.</returns>
        [HttpPatch]
        public JsonResult Change(JObject user)
        {
            try
            {
                var updatedUser = JsonConvert.DeserializeObject<User>((user.ToString()));

                if (!new UserSpecification().IsSatisfiedBy(updatedUser))
                {
                    return new JsonResult(new { success = false, responseText = "User is not valid." });
                }

                _userService.UpdateUser(updatedUser);

                return new JsonResult(new { success = true, responseText = "User successfully modified!" });
            }
            catch (JsonException jsonExp)
            {
                Log.Error(jsonExp, "UserController.Change");
                return new JsonResult(new { success = false, responseText = "User failed to get modified." });
            }
            catch (Exception exp)
            {
                Log.Error(exp, "UserController.Change");
                return new JsonResult(new { success = false, responseText = "User failed to get modified." });
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>Returns JsonResult object with success flag.</returns>
        [HttpDelete]
        public JsonResult Delete(string id)
        {
            _userService.DeleteUser(id);

            return new JsonResult(new { success = true, responseText = "User successfully deleted!" });
        }
    }
}
