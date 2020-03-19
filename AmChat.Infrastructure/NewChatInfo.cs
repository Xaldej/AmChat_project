using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class NewChatInfo
    {
        public string Name { get; set; }

        public List<string> LoginsToAdd { get; set; }

        public NewChatInfo()
        {

        }

        public NewChatInfo(string name, List<string> loginsToAdd)
        {
            Name = name;
            LoginsToAdd = loginsToAdd;
        }
    }
}
