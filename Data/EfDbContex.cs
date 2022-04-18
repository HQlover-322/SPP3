using back.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.Data
{
    public class EfDbContex : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Entities.File> Files { get; set; }

        public EfDbContex(DbContextOptions options) : base(options)
        {
        }
        public EfDbContex()
        {

        }
    }
}
