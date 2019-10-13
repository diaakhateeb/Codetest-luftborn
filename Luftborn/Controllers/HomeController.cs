using AutoMapper;
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
using System.Threading.Tasks;

namespace Luftborn.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _iConfig;
        private readonly ClientProvider _clientProvider;
        private readonly IMapper _mapper;

        public HomeController(IConfiguration iConfig, ClientProvider clientProvider, IMapper mapper)
        {
            _iConfig = iConfig;
            _clientProvider = clientProvider;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var getAllUsersUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("GetAllService");
                var allUsers = await _clientProvider.Client.GetAsync(getAllUsersUrl);
                var allUsersResult = await allUsers.Content.ReadAsStringAsync();
                var usersList = JsonConvert.DeserializeObject<List<User>>(allUsersResult);
                var usersListViewModel = _mapper.Map<List<UserViewModel>>(usersList);

                if (usersList.Any())
                    return View(usersListViewModel);

                {
                    var addManyUsersUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("AddManyService");
                    var usersFromJson = new GetUsersFromJson("users.json").Execute();
                    var users = await _clientProvider.Client.PostAsync(addManyUsersUrl,
                        new StringContent(usersFromJson.ToString(), Encoding.UTF8, "application/json"));

                    usersList = JsonConvert.DeserializeObject<List<User>>(await users.Content.ReadAsStringAsync());
                    usersListViewModel = _mapper.Map<List<UserViewModel>>(usersList);

                    return View(usersListViewModel);
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
