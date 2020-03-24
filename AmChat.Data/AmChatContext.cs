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

        public DbSet<DBChat> Chats { get; set; }

        public DbSet<DBUser> Users { get; set; }

        public DbSet<UserChat> UsersChats { get; set; }

        public DbSet<ChatUser> UsersInChat { get; set; }
    }
}
