using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class Chat : BaseEntity
    {
        public string Name { get; set; }

        public bool IsMultipleUsersChat { get; set; }

        public ICollection<UserInChat> UsersInChat { get; set; }
    }
}
