using Hellion.Database.Exceptions;
using Hellion.Database.Repository;
using Hellion.Database.Structures;
using Microsoft.EntityFrameworkCore;

namespace Hellion.Database
{
    public static class DatabaseService
    {
        private static DatabaseContext dbContext;
        private static IRepository<DbUser> userRepository;
        private static IRepository<DbCharacter> characterRepository;
        private static IRepository<DbItem> itemRepository;

        /// <summary>
        /// Gets the users repository.
        /// </summary>
        public static IRepository<DbUser> Users
        {
            get
            {
                if (dbContext == null)
                    throw new DatabaseNotInitializedException();

                if (userRepository == null)
                    userRepository = new UserRepository(dbContext);

                return userRepository;
            }
        }

        /// <summary>
        /// Gets the characters repository.
        /// </summary>
        public static IRepository<DbCharacter> Characters
        {
            get
            {
                if (dbContext == null)
                    throw new DatabaseNotInitializedException();

                if (characterRepository == null)
                    characterRepository = new CharacterRepository(dbContext);

                return characterRepository;
            }
        }

        /// <summary>
        /// Gets the items repository.
        /// </summary>
        public static IRepository<DbItem> Items
        {
            get
            {
                if (dbContext == null)
                    throw new DatabaseNotInitializedException();

                if (itemRepository == null)
                    itemRepository = new ItemRepository(dbContext);

                return itemRepository;
            }
        }

        /// <summary>
        /// Initialize the database service context.
        /// </summary>
        /// <param name="context">Database context</param>
        public static void InitializeDatabase(DbContext context)
        {
            dbContext = context as DatabaseContext;
        }

        /// <summary>
        /// Initialize the database service context.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="databaseName"></param>
        public static void Initialize(string ip, string user, string password, string databaseName)
        {
            dbContext = new DatabaseContext(ip, user, password, databaseName);

            dbContext.Database.EnsureCreated();
        }
    }
}
