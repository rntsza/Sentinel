using System.Data.Entity;

namespace Sentinel.All
{
    public class ContextDB : DbContext
    {
        public DbSet<TwitchUser> TwitchUsers { get; set; }

        public ContextDB() : base("name=ContextDB")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}