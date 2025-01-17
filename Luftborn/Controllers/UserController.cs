﻿using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserController(IConfiguration iConfig, ClientProvider clientProvider, IMapper mapper)
        {
            _iConfig = iConfig;
            _clientProvider = clientProvider;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id = "0")
        {
            var editUserUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("FindUserService");
            var userResult = await _clientProvider.Client.GetAsync(editUserUrl + "?id=" + id);

            if (!userResult.IsSuccessStatusCode)
                return View();

            var user = JsonConvert.DeserializeObject<User>(await userResult.Content.ReadAsStringAsync());
            var userViewModel = _mapper.Map<UserViewModel>(user);

            return View(userViewModel);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Edit(UserViewModel userData)
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
        public async Task<IActionResult> Add(UserViewModel user)
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

        [HttpDelete, Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var deleteUserUrl = _iConfig.GetSection("Urls").GetSection("Users").GetValue<string>("DeleteService");
            var deleteResult = await _clientProvider.Client.DeleteAsync(deleteUserUrl + "?id=" + id);

            if (!deleteResult.IsSuccessStatusCode)
                return View("Error", new ErrorViewModel { ErrorMessage = await deleteResult.RequestMessage.Content.ReadAsStringAsync() });

            var result = JsonConvert.DeserializeObject<dynamic>(await deleteResult.Content.ReadAsStringAsync());
            if (result.success == true)
                return RedirectToAction("Index", "Home");

            return View("Error", new ErrorViewModel { ErrorMessage = result.responseText });

        }
    }
}
