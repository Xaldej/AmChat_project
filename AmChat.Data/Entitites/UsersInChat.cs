using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class UsersInChat : BaseEntity
    {
        [Required]
        public Guid ChatId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
