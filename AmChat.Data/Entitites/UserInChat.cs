using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class UserInChat
    {
        public int Id { get; set; }

        public DBChat Chat { get; set; }

        [Required]
        public Guid ChatId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
