using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces.ServerServices
{
    public interface IChatMaintenanceService
    {
        void ChangeChatListenersAmount(IMessengerService client);

        void ProcessChatChange(object sender, NotifyCollectionChangedEventArgs e);
    }
}
