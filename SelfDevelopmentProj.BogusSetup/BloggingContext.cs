using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SelfDevelopmentProj.BogusSetup
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=blogging.db");
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //FakeDataA.Init(10);

            //builder.Entity<Blog>().HasData(FakeDataA.blogs);
            //builder.Entity<Post>().HasData(FakeDataA.posts);
        }
    }
}
