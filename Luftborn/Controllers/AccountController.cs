using Luftborn.Helpers;
using Luftborn.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Luftborn.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _iConfig;
        private readonly ClientProvider _clientProvider;
        public AccountController(IConfiguration iConfig, ClientProvider clientProvider)
        {
            _iConfig = iConfig;
            _clientProvider = clientProvider;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            var loginUrl = _iConfig.GetSection("Urls").GetSection("Auth").GetValue<string>("Login");
            var loginResult = await _clientProvider.Client.PostAsJsonAsync(loginUrl + "?username=" + loginVm.Username, loginVm);

            if (!loginResult.IsSuccessStatusCode)
                return View();

            var claims = new List<Claim>
                {
                    new Claim("Username", loginVm.Username)
                };

            await HttpContext.SignInAsync(
                new ClaimsPrincipal(
                    new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}