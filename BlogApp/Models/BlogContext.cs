using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class BlogContext:DbContext
    {
        private IConfiguration _configuration;
        public BlogContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Superuser> Superusers { get; set; }
        public DbSet<New> News { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_configuration.GetConnectionString("MsSqlConnection"));
        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        { }
    }
}
