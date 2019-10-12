using Luftborn.Helpers;
using Luftborn.Models.ViewModel;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class User
    {
        private ClientProvider _clientProvider;

        [SetUp]
        public void Setup()
        {
            Startup.Initialize();
            _clientProvider = new ClientProvider();
        }

        [Test]
        public async Task Create_User_OK()
        {
            var userResult = await _clientProvider.Client.PostAsync("http://localhost/LuftbornWebApi/api/User/Add",
                new StringContent(
                    JsonConvert.SerializeObject(new UserViewModel
                    {
                        Name = "Jack Mondi",
                        UserName = "jmondi",
                        Email = "jack@luftborn.com",
                        Address = "Denmark, CPH",
                        PhoneNumber = "21455198"
                    }),
                    Encoding.UTF8, "application/json"));
            Assert.AreEqual(userResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<dynamic>(await userResult.Content.ReadAsStringAsync());
            Assert.AreEqual(true, result.success.Value);


        }

        [Test]
        public async Task Create_User_Fail()
        {
            var userResult = await _clientProvider.Client.PostAsync("http://localhost/LuftbornWebApi/api/User/Add",
                new StringContent(
                    JsonConvert.SerializeObject(new UserViewModel
                    {
                        Name = "Zaki Joshwa",
                        UserName = "zjoshwa",
                        Email = "zaki",
                        Address = "Denmark, CPH",
                        PhoneNumber = "2145519869"
                    }),
                    Encoding.UTF8, "application/json"));
            Assert.AreEqual(userResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<dynamic>(await userResult.Content.ReadAsStringAsync());
            Assert.AreEqual(false, result.success.Value);

        }

        [Test]
        public async Task Find_Existed_UserById()
        {
            var userResult =
                await _clientProvider.Client.GetAsync(
                    "http://localhost/LuftbornWebApi/api/User/GetUserById?id=5d95da3b09f49b55dc425352");

            Assert.AreEqual(userResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<Models.User>(await userResult.Content.ReadAsStringAsync());
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task Find_Non_Existed_UserById()
        {
            var userResult =
                await _clientProvider.Client.GetAsync(
                    "http://localhost/LuftbornWebApi/api/User/GetUserById?id=9f95da5a09f49c52dc425344");

            Assert.AreEqual(userResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<Models.User>(await userResult.Content.ReadAsStringAsync());
            Assert.IsNull(result);

        }

        [Test]
        public async Task Edit_User_OK()
        {
            var updateUserResult = await _clientProvider.Client.PatchAsync("http://localhost/LuftbornWebApi/api/User/Change",
                    new StringContent(JsonConvert.SerializeObject(new UserViewModel
                    {
                        Id = "5d95da3b09f49b55dc425352",
                        Name = "Morten Bondo",
                        UserName = "Bondo",
                        Email = "morten@luftborn.com",
                        Address = "provstevej 12, Denmark - CPH", // Denmark - CPH
                        PhoneNumber = "22445922"
                    }), Encoding.UTF8, "application/json"));

            Assert.AreEqual(updateUserResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<dynamic>(await updateUserResult.Content.ReadAsStringAsync());
            Assert.AreEqual(true, result.success.Value);

        }

        [Test]
        public async Task Edit_User_Fail()
        {
            var updateUserResult = await _clientProvider.Client.PatchAsync("http://localhost/LuftbornWebApi/api/User/Change",
                    new StringContent(JsonConvert.SerializeObject(new UserViewModel
                    {
                        Id = "5d95da3b09f49b55dc425352",
                        Name = "Morten Bondo",
                        UserName = "Bondo",
                        Email = "morten@luftborn.com",
                        Address = "provstevej 12",
                        PhoneNumber = "2244592266544" // phone number is invalid.
                    }), Encoding.UTF8, "application/json"));

            Assert.AreEqual(updateUserResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<dynamic>(await updateUserResult.Content.ReadAsStringAsync());
            Assert.AreEqual(false, result.success.Value);

        }

        [Test]
        public async Task Delete_User()
        {
            var deleteUserResult =
                await _clientProvider.Client.DeleteAsync("http://localhost/LuftbornWebApi/api/User/Delete?id=5d9ac8a4f0d9bb666ca1b033");

            Assert.AreEqual(deleteUserResult.IsSuccessStatusCode, true);

            var result = JsonConvert.DeserializeObject<dynamic>(await deleteUserResult.Content.ReadAsStringAsync());
            Assert.AreEqual(true, result.success.Value);

        }
    }
}