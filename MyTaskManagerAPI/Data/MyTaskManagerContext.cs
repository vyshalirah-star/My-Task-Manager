using Microsoft.EntityFrameworkCore;
using MyTaskManagerAPI.Models;

namespace MyTaskManagerAPI.Data
{
    public class MyTaskManagerContext : DbContext
    {
        public MyTaskManagerContext(DbContextOptions<MyTaskManagerContext> options)
            : base(options) { }

        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
