using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands.FromClienToServer
{
    public class CloseConnection : BaseCommand
    {
        public override string Name => nameof(CloseConnection).ToLower();
    }
}
