using Luftborn.Helpers;
using Luftborn.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Luftborn.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _iConfig;
        private readonly ClientProvider _clientProvider;

        public UserController(IConfiguration iConfig, ClientProvider clientProvider)
        {
            _iConfig = iConfig;
            _clientProvider = clientProvider;
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id = "0")
        {
            var editUserUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("FindUserService");
            var userResult = await _clientProvider.Client.GetAsync(editUserUrl + "?id=" + id);

            if (!userResult.IsSuccessStatusCode)
                return View();

            var user = JsonConvert.DeserializeObject<User>(await userResult.Content.ReadAsStringAsync());

            return View(user);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Edit(User userData)
        {
            try
            {
                var editUserUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("EditService");
                var updateUserResult = await _clientProvider.Client.PatchAsync(editUserUrl,
                        new StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json"));

                if (!updateUserResult.IsSuccessStatusCode)
                    return View();

                var result = JsonConvert.DeserializeObject<dynamic>(await updateUserResult.Content.ReadAsStringAsync());
                if (result.success == true)
                    return RedirectToAction("Index", "Home");

                TempData["errorMsg"] = result.responseText;
                return View();
            }
            catch (HttpRequestException httpReqExp)
            {
                Log.Error(httpReqExp, "UserController.Edit");
                return View();
            }
            catch (Exception exp)
            {
                Log.Error(exp, "UserController.Edit");
                return View();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Add(RegisterViewModel user)
        {
            try
            {
                var usersUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("AddService");
                var userResult = await _clientProvider.Client.PostAsync(usersUrl,
                    new StringContent(
                        JsonConvert.SerializeObject(user),
                        Encoding.UTF8, "application/json"));

                if (!userResult.IsSuccessStatusCode)
                    return View();

                var result = JsonConvert.DeserializeObject<dynamic>(await userResult.Content.ReadAsStringAsync());
                if (result.success == true)
                    return RedirectToAction("Index", "Home");

                TempData["errorMsg"] = result.responseText;
                return View();
            }
            catch (HttpRequestException httpReqExp)
            {
                Log.Error(httpReqExp, "UserController.Add");
                return View();
            }
            catch (Exception exp)
            {
                Log.Error(exp, "UserController.Add");
                return View();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
