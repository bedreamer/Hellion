using Hellion.Database.Structures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hellion.Database
{
    public class DatabaseContext : DbContext
    {
        private string dbhost;
        private string dbUser;
        private string dbPassword;
        private string dbName;
        
        public DbSet<DbUser> Users { get; set; }

        public DbSet<DbCharacter> Characters { get; set; }

        public DbSet<DbItem> Items { get; set; }


        /// <summary>
        /// Creates a new DatabaseContext instance.
        /// </summary>
        /// <param name="ip">Database host address</param>
        /// <param name="user">Database user</param>
        /// <param name="password">Database password</param>
        /// <param name="databaseName">Database name</param>
        public DatabaseContext(string ip, string user, string password, string databaseName)
            : base()
        {
            this.dbhost = ip;
            this.dbUser = user;
            this.dbPassword = password;
            this.dbName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = string.Format("server={0};userid={1};pwd={2};port=3306;database={3};sslmode=none;", 
                this.dbhost,
                this.dbUser,
                this.dbPassword,
                this.dbName);

            optionsBuilder.UseMySQL(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbItem>()
                .HasOne(i => i.Character)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CharacterId);

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            base.OnModelCreating(modelBuilder);
        }
    }
}
