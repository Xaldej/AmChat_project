using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public class BaseCommand
    {
        public virtual string Name { get; set; }

        public string Data { get; set; }
    }
}
