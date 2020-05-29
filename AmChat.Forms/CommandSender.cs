using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Forms
{
    public class CommandSender
    {
        private readonly IMessengerService messenger;


        public CommandSender(IMessengerService messenger)
        {
            this.messenger = messenger;
        }


        public void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chatName, userLoginsToAdd);

            var commandJson = CommandMaker.GetCommandJson<AddOrUpdateChat, NewChatInfo>(newChatInfo);

            messenger.SendMessage(commandJson);
        }

        public void AddUsersToChat(ClientChat chat, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chat.Id, userLoginsToAdd);

            var commandJson = CommandMaker.GetCommandJson<AddOrUpdateChat, NewChatInfo>(newChatInfo);

            messenger.SendMessage(commandJson);
        }

        public void CloseConnection(int attemptAmount = 0)
        {
            try
            {
                var commandJson = CommandMaker.GetCommandJson<CloseConnection, string>(string.Empty, true);

                messenger.SendMessage(commandJson);
            }
            catch
            {
                if(attemptAmount<3)
                {
                    Task.Delay(1000);
                    CloseConnection(attemptAmount + 1);
                }
                else
                {
                    return;
                }
            }
        }

        public void GetChats()
        {
            var commandJson = CommandMaker.GetCommandJson<GetChats, string>(string.Empty, true);

            messenger.SendMessage(commandJson);
        }

        public void GetKeyFromServer()
        {
            var commandJson = CommandMaker.GetCommandJson<GetKey, string>(string.Empty, true);

            messenger.SendMessage(commandJson);
        }

        public void Login(LoginData loginData)
        {
            var commandJson = CommandMaker.GetCommandJson<Login, LoginData>(loginData);

            messenger.SendMessage(commandJson);
        }

        public void SendMessageToChat(string message, ChatInfo chat)
        {
            var messageToChat = new ChatMessage()
            {
                FromUser = messenger.User,
                ToChatId = chat.Id,
                DateAndTime = DateTime.Now,
                Text = message,
            };

            var commandJson = CommandMaker.GetCommandJson<SendMessageToChat, ChatMessage>(messageToChat);

            messenger.SendMessage(commandJson);

            chat.ChatMessages.Add(messageToChat);
        }


    }
}

