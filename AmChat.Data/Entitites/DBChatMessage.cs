using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class DBChatMessage : BaseEntity
    {
        [Index]
        public Guid ChatId { get; set; }

        public Guid FromUserId { get; set; }

        public DateTime DateAndTime { get; set; }

        public string Text { get; set; }
    }
}
