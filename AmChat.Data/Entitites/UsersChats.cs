using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class UsersChats : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ChatId { get; set; }

        public ICollection<Chat> Chats { get; set; }
    }
}
