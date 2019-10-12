using Luftborn.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    public class Authentication
    {
        private ClientProvider _clientProvider;

        [SetUp]
        public void Setup()
        {
            Startup.Initialize();
            _clientProvider = new ClientProvider();
        }

        [Test]
        public async Task Login_OK()
        {
            var loginResult = await _clientProvider.Client
                .PostAsJsonAsync("http://localhost/LuftbornWebApi/api/Authentication/AuthenticateUser?username=Bondo", "Bondo");

            if (loginResult.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<User>(await loginResult.Content.ReadAsStringAsync());
                Assert.IsNotNull(user);
            }
        }

        [Test]
        public async Task Login_Fail()
        {
            var loginResult = await _clientProvider.Client
                .PostAsJsonAsync("http://localhost/LuftbornWebApi/api/Authentication/AuthenticateUser?username=John", "John");

            Assert.AreEqual(loginResult.StatusCode, HttpStatusCode.Unauthorized);
        }
    }
}