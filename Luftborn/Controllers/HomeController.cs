using Luftborn.Helpers;
using Luftborn.Models.ViewModel;
using Luftborn.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Luftborn.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _iConfig;

        public HomeController(IConfiguration iConfig)
        {
            _iConfig = iConfig;
        }

        [Authorize]
        public IActionResult Index()
        {
            try
            {
                using (var cnt = new ClientProvider())
                {
                    var usersUrl = _iConfig.GetSection("Urls").GetValue<string>("Users");
                    var uri = new Uri(usersUrl);

                    cnt.Client.BaseAddress = new Uri(uri.Scheme + "://" + uri.Host);
                    var allUserResult = cnt.Client.GetAsync(usersUrl + "/GetAll").Result.Content.ReadAsStringAsync()
                        .Result;
                    var usersList = JsonConvert.DeserializeObject<List<User>>(allUserResult);
                    if (usersList.Any())
                        return View(usersList);

                    var usersFromJson = new GetUsersFromJson("users.json").Execute();
                    var users = cnt.Client.PostAsync(usersUrl + "/AddMany",
                        new StringContent(usersFromJson.ToString(), Encoding.UTF8, "application/json")).Result;
                    usersList = JsonConvert.DeserializeObject<List<User>>(users.Content.ReadAsStringAsync().Result);

                    return View(usersList);
                }
            }
            catch (ArgumentNullException argNullExp)
            {
                Log.Error(argNullExp, "HomeController.Index");
                return View();
            }
            catch (NullReferenceException nullRefExp)
            {
                Log.Error(nullRefExp, "HomeController.Index");
                return View();
            }
            catch (FileNotFoundException fileNotFoundExp)
            {
                Log.Error(fileNotFoundExp, "HomeController.Index");
                return View();
            }
            catch (FileLoadException fileLoadExp)
            {
                Log.Error(fileLoadExp, "HomeController.Index");
                return View();
            }
            catch (JsonException jsonExp)
            {
                Log.Error(jsonExp, "HomeController.Index");
                return View();
            }
            catch (HttpRequestException httpReqExp)
            {
                Log.Error(httpReqExp, "HomeController.Index");
                return View();
            }
            catch (Exception exp)
            {
                Log.Error(exp, "HomeController.Index");
                return View();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
