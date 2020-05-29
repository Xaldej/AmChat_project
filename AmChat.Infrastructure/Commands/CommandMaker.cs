using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public static class CommandMaker
    {
        public static string GetCommandJson<T, TData>(TData data, bool isDataReady = false) where T : BaseCommand, new()
        {
            string dataJson;

            if(isDataReady)
            {
                dataJson = data as string;
            }
            else
            {
                dataJson = JsonParser<TData>.OneObjectToJson(data);
            }
            

            var command = new T() { Data = dataJson };
            var commandJson = JsonParser<T>.OneObjectToJson(command);

            return commandJson;
        }
    }
}
