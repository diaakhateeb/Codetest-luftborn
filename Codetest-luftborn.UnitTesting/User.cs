using Luftborn.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Text;

namespace Tests
{
    public class User
    {
        [SetUp]
        public void Setup()
        {
            Startup.Initialize();
        }

        [Test]
        public void Create_User_OK()
        {
            using (var cnt = new ClientProvider())
            {
                var userResult = cnt.Client.PostAsync("http://localhost/LuftbornWebApi/api/User/Add",
                    new StringContent(
                        JsonConvert.SerializeObject(new Models.User
                        {
                            Name = "Jack Mondi",
                            UserName = "jmondi",
                            Email = "jack@luftborn.com",
                            Address = "Denmark, CPH",
                            PhoneNumber = "21455198"
                        }),
                        Encoding.UTF8, "application/json")).Result;
                Assert.AreEqual(userResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<dynamic>(userResult.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(true, result.success.Value);

            }
        }

        [Test]
        public void Create_User_Fail()
        {
            using (var cnt = new ClientProvider())
            {
                var userResult = cnt.Client.PostAsync("http://localhost/LuftbornWebApi/api/User/Add",
                    new StringContent(
                        JsonConvert.SerializeObject(new Models.User
                        {
                            Name = "Zaki Joshwa",
                            UserName = "zjoshwa",
                            Email = "zaki",
                            Address = "Denmark, CPH",
                            PhoneNumber = "2145519869"
                        }),
                        Encoding.UTF8, "application/json")).Result;
                Assert.AreEqual(userResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<dynamic>(userResult.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(false, result.success.Value);
            }
        }

        [Test]
        public void Find_Existed_UserById()
        {
            using (var cnt = new ClientProvider())
            {
                var userResult =
                    cnt.Client.GetAsync("http://localhost/LuftbornWebApi/api/User/GetUserById?id=5d95da3b09f49b55dc425352").Result;

                Assert.AreEqual(userResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<Models.User>(userResult.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void Find_Non_Existed_UserById()
        {
            using (var cnt = new ClientProvider())
            {
                var userResult =
                    cnt.Client.GetAsync("http://localhost/LuftbornWebApi/api/User/GetUserById?id=9f95da5a09f49c52dc425344").Result;

                Assert.AreEqual(userResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<Models.User>(userResult.Content.ReadAsStringAsync().Result);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void Edit_User_OK()
        {
            using (var cnt = new ClientProvider())
            {
                var updateUserResult = cnt.Client.PatchAsync("http://localhost/LuftbornWebApi/api/User/Change",
                        new StringContent(JsonConvert.SerializeObject(new Models.User
                        {
                            Id = "5d95da3b09f49b55dc425352",
                            Name = "Morten Bondo",
                            UserName = "Bondo",
                            Email = "morten@luftborn.com",
                            Address = "provstevej 12, Denmark - CPH", // Denmark - CPH
                            PhoneNumber = "22445922"
                        }), Encoding.UTF8, "application/json"))
                    .Result;

                Assert.AreEqual(updateUserResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<dynamic>(updateUserResult.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(true, result.success.Value);
            }
        }

        [Test]
        public void Edit_User_Fail()
        {
            using (var cnt = new ClientProvider())
            {
                var updateUserResult = cnt.Client.PatchAsync("http://localhost/LuftbornWebApi/api/User/Change",
                        new StringContent(JsonConvert.SerializeObject(new Models.User
                        {
                            Id = "5d95da3b09f49b55dc425352",
                            Name = "Morten Bondo",
                            UserName = "Bondo",
                            Email = "morten@luftborn.com",
                            Address = "provstevej 12",
                            PhoneNumber = "2244592266544" // phone number is invalid.
                        }), Encoding.UTF8, "application/json"))
                    .Result;

                Assert.AreEqual(updateUserResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<dynamic>(updateUserResult.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(false, result.success.Value);
            }
        }

        [Test]
        public void Delete_User()
        {
            using (var cnt = new ClientProvider())
            {
                var deleteUserResult = cnt.Client.DeleteAsync("http://localhost/LuftbornWebApi/api/User/Delete?id=5d9ac8a4f0d9bb666ca1b033").Result;

                Assert.AreEqual(deleteUserResult.IsSuccessStatusCode, true);

                var result = JsonConvert.DeserializeObject<dynamic>(deleteUserResult.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(true, result.success.Value);
            }
        }
    }
}