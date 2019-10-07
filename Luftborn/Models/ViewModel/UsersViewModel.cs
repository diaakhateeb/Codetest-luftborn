using Models;
using System.Collections.Generic;

namespace Luftborn.Models.ViewModel
{
    public class UsersViewModel
    {
        public UsersViewModel()
        {
            Users = new List<User>();
        }
        public List<User> Users { get; set; }
    }
}
