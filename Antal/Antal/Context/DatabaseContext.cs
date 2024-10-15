using Antal.Models;
using Microsoft.EntityFrameworkCore;

namespace Antal.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<TodoTask> TodoItems { get; set; }
    }
}
