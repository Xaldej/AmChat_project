using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces
{
    public interface ICommandHandlerService
    {
        ICollection<MessageToProcess> MessagesToProcess { get; set; }

        void ProcessMessage(MessageToProcess message);
    }
}
