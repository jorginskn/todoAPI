using Microsoft.EntityFrameworkCore;

namespace todoAPI.Model
{
    public class TodoContext : DbContext
    {

        public TodoContext(DbContextOptions<TodoContext> options)
          : base(options)
        {

        }

        public DbSet<Item> TodoItems { get; set; } = null!;
    }
}
