using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Models
{
    public class UserDBContext: DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public virtual DbSet<TodoItem> TodoItems { get; set; }
    }
}
