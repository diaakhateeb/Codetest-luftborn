using Luftborn.Helpers;
using Luftborn.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;

namespace Luftborn.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _iConfig;
        public AccountController(IConfiguration iConfig)
        {
            _iConfig = iConfig;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult Login(LoginViewModel loginVm)
        {
            using (var cnt = new ClientProvider())
            {
                var loginUrl = _iConfig.GetSection("Urls").GetSection("Auth").GetValue<string>("Login");
                var uri = new Uri(loginUrl);

                cnt.Client.BaseAddress = new Uri(uri.Scheme + "://" + uri.Host);
                var loginResult = cnt.Client.PostAsJsonAsync(loginUrl + "?username=" + loginVm.Username, loginVm).Result;

                if (!loginResult.IsSuccessStatusCode)
                    return View();

                var claims = new List<Claim>
                {
                    new Claim("Username", loginVm.Username)
                };

                HttpContext.SignInAsync(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme))).Wait();
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}