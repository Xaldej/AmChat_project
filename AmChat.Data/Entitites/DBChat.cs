using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class DBChat : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<UserInChat> UsersInChat { get; set; }
    }
}
