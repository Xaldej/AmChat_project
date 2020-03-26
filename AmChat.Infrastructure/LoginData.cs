using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class LoginData
    {
        public string Login { get; set; }

        public int PasswordHash { get; set; }

        public LoginData(string login, int passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
        }
    }
}
