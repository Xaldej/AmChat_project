﻿using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class ChatIsAddedHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {
            var chatInfo = JsonParser<ChatInfo>.JsonToOneObject(data);

            var chatToAdd = ChatInfoToChat(chatInfo);

            if (chatToAdd.ChatMessages == null)
            {
                chatToAdd.ChatMessages = new ObservableCollection<ChatMessage>();
            }

            messenger.UserChats.Add(chatToAdd);
        }

        private Chat ChatInfoToChat(ChatInfo chatInfo)
        {
            return new Chat()
            {
                Id = chatInfo.Id,
                Name = chatInfo.Name,
                UsersInChat = chatInfo.UsersInChat,
                ChatMessages = chatInfo.ChatMessages,
            };
        }
    }
}