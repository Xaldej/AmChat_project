using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class UserChat
    {
        public int Id { get; set; }

        public DBUser User { get; set; }

        [Index]
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ChatId { get; set; }
    }
}
