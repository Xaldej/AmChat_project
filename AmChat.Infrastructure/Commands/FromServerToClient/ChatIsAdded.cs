using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands.FromServerToClient
{
    public class ChatIsAdded : BaseCommand
    {
        public override string Name => nameof(ChatIsAdded).ToLower();
    }
}
