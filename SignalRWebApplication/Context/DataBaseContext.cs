using Microsoft.EntityFrameworkCore;
using SignalRWebApplication.Models.Entities;

namespace SignalRWebApplication.Context
{
    public class DataBaseContext:DbContext
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DataBaseContext(DbContextOptions options):base(options)
        {

        }
    }
}
