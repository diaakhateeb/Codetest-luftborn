using Models;
using MongoDB.Driver;
using Repository.Interfaces;

namespace Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }
    }
}
