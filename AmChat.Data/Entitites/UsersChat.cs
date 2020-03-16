using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class UsersChat
    {
        public int Id { get; set; }

        public User User { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ChatId { get; set; }
    }
}
