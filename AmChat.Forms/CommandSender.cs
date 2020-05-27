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

            var commandJson = CommandExtentions.GetCommandJson<AddOrUpdateChat, NewChatInfo>(newChatInfo);

            messenger.SendMessage(commandJson);
        }

        public void AddUsersToChat(Chat chat, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chat.Id, userLoginsToAdd);

            var commandJson = CommandExtentions.GetCommandJson<AddOrUpdateChat, NewChatInfo>(newChatInfo);

            messenger.SendMessage(commandJson);
        }

        public void CloseConnection()
        {
            try
            {
                var commandJson = CommandExtentions.GetCommandJson<CloseConnection, string>(string.Empty, true);

                messenger.SendMessage(commandJson);
            }
            catch
            {

            }
        }

        public void GetChats()
        {
            var commandJson = CommandExtentions.GetCommandJson<GetChats, string>(string.Empty, true);

            messenger.SendMessage(commandJson);
        }

        public void GetKeyFromServer()
        {
            var commandJson = CommandExtentions.GetCommandJson<GetKey, string>(string.Empty, true);

            messenger.SendMessage(commandJson);
        }

        public void Login(LoginData loginData)
        {
            var commandJson = CommandExtentions.GetCommandJson<Login, LoginData>(loginData);

            messenger.SendMessage(commandJson);
        }

        public void SendMessageToChat(string message, Chat chat)
        {
            var messageToChat = new ChatMessage()
            {
                FromUser = messenger.User,
                ToChatId = chat.Id,
                DateAndTime = DateTime.Now,
                Text = message,
            };

            var commandJson = CommandExtentions.GetCommandJson<SendMessageToChat, ChatMessage>(messageToChat);

            messenger.SendMessage(commandJson);

            chat.ChatMessages.Add(messageToChat);
        }
    }
}

