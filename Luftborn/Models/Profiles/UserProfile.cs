using AutoMapper;
using Luftborn.Models.ViewModel;
using Models;

namespace Luftborn.Models.Profiles
{
    /// <summary>
    /// AutoMapper profile class.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Creates UserProfile instance.
        /// </summary>
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}
