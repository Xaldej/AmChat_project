using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public override bool Equals(object obj)
        {
            UserInfo userToCompare = obj as UserInfo;

            if(userToCompare==null)
            {
                return false;
            }

            return Id == userToCompare.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -744452910;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Login);
            return hashCode;
        }
    }
}
