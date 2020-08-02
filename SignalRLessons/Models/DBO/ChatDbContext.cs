using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Models.DBO
{
    public class ChatDbContext:IdentityDbContext<User>
    {
        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> opt) : base(opt) { }
        //public ChatDbContext(string opt) : base() {
        //    OnConfiguring.Use
        //}
    }
}
