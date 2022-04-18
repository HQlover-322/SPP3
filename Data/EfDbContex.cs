using back.Data.Entities;
using back.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace back.Data
{
    public class EfDbContex : IdentityDbContext<User>
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Entities.File> Files { get; set; }

        public EfDbContex(DbContextOptions<EfDbContex> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public EfDbContex()
        {

        }
    }
}
