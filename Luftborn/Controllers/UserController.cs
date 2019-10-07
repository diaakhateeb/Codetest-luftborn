using Luftborn.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Text;

namespace Luftborn.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _iConfig;

        public UserController(IConfiguration iConfig)
        {
            _iConfig = iConfig;
        }

        [Authorize]
        public IActionResult Edit(string id = "0")
        {
            using (var cnt = new ClientProvider())
            {
                var usersUrl = _iConfig.GetSection("Urls").GetValue<string>("Users");
                var uri = new Uri(usersUrl);

                cnt.Client.BaseAddress = new Uri(uri.Scheme + "://" + uri.Host);
                var userResult = cnt.Client.GetAsync(usersUrl + "/GetUserById?id=" + id).Result;

                if (!userResult.IsSuccessStatusCode)
                    return View();
                var user = JsonConvert.DeserializeObject<User>(userResult.Content.ReadAsStringAsync().Result);

                return View(user);
            }
        }

        [HttpPost, Authorize]
        public IActionResult Edit(User userData)
        {
            try
            {
                using (var cnt = new ClientProvider())
                {
                    var usersUrl = _iConfig.GetSection("Urls").GetValue<string>("Users");
                    var uri = new Uri(usersUrl);

                    cnt.Client.BaseAddress = new Uri(uri.Scheme + "://" + uri.Host);
                    var updateUserResult = cnt.Client.PatchAsync(
                            usersUrl + "/Change",
                            new StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json"))
                        .Result;

                    if (!updateUserResult.IsSuccessStatusCode)
                        return View();

                    var result = JsonConvert.DeserializeObject<dynamic>(updateUserResult.Content.ReadAsStringAsync().Result);
                    if (result.success == true)
                        return RedirectToAction("Index", "Home");

                    TempData["errorMsg"] = result.responseText;
                    return View();
                }
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
        public IActionResult Add(User user)
        {
            try
            {
                using (var cnt = new ClientProvider())
                {
                    var usersUrl = _iConfig.GetSection("Urls").GetValue<string>("Users");
                    var uri = new Uri(usersUrl);
                    cnt.Client.BaseAddress = new Uri(uri.Scheme + "://" + uri.Host);

                    var userResult = cnt.Client.PostAsync(usersUrl + "/Add",
                        new StringContent(
                            JsonConvert.SerializeObject(user),
                            Encoding.UTF8, "application/json")).Result;

                    if (!userResult.IsSuccessStatusCode)
                        return View();

                    var result = JsonConvert.DeserializeObject<dynamic>(userResult.Content.ReadAsStringAsync().Result);
                    if (result.success == true)
                        return RedirectToAction("Index", "Home");

                    TempData["errorMsg"] = result.responseText;
                    return View();
                }
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
