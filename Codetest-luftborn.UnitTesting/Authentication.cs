using Luftborn.Helpers;
using Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Tests
{
    public class Authentication
    {
        [SetUp]
        public void Setup()
        {
            Startup.Initialize();
        }

        [Test]
        public void Login_OK()
        {
            using (var cnt = new ClientProvider())
            {
                cnt.Client.BaseAddress = new Uri("http://localhost/");
                var loginResult = cnt.Client
                    .PostAsJsonAsync("LuftbornWebApi/api/Authentication/AuthenticateUser?username=Bondo", "Bondo").Result;

                if (loginResult.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<User>(loginResult.Content.ReadAsStringAsync().Result);
                    Assert.IsNotNull(user);
                }
            }
        }

        [Test]
        public void Login_Fail()
        {
            using (var cnt = new ClientProvider())
            {
                cnt.Client.BaseAddress = new Uri("http://localhost/");
                var loginResult = cnt.Client
                    .PostAsJsonAsync("LuftbornWebApi/api/Authentication/AuthenticateUser?username=John", "John").Result;

                Assert.AreEqual(loginResult.IsSuccessStatusCode, false);
            }
        }
    }
}