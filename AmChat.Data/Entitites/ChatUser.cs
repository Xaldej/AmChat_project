using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class ChatUser : BaseEntity
    {
        public DBChat Chat { get; set; }

        [Required]
        [Index(IsUnique = false)]
        public Guid ChatId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
