using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Data.Entitites
{
    public class DBUser : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        public string Login { get; set; }

        [Required]
        public int PasswordHash { get; set; }

        public ICollection<UserChat> UsersChats { get; set; }
    }
}
