using AmChat.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data
{
    public class AmChatContext : DbContext
    {
        public AmChatContext() : base("AmChat")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AmChatContext>());
        }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UsersChats> UsersChats { get; set; }

        public DbSet<UsersInChat> UsersInChat { get; set; }
    }
}
