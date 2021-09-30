using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class BlogContext:DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Superuser> Superusers { get; set; }
        public DbSet<New> News { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=.\\SQLEXPRESS;database=blogapp; integrated security=SSPI");
        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        { }
    }
}
