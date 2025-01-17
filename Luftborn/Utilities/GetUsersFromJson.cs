using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Luftborn.Utilities
{
    public class GetUsersFromJson
    {
        readonly string _path;

        public GetUsersFromJson(string path)
        {
            _path = path;
        }

        public List<User> Execute()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directory = Path.Combine(baseDirectory, _path);
            using (var reader = new StreamReader(directory))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<User>>(json);
            }
        }
    }
}