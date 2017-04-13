using Hellion.Database.Structures;
using Microsoft.EntityFrameworkCore;

namespace Hellion.Database.Repository
{
    public class UserRepository : RepositoryBase<DbUser>
    {
        public UserRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
