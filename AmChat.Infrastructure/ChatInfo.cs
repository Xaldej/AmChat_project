﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ChatInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ObservableCollection<User> UsersInChat;

        public ObservableCollection<MessageToChat> ChatMessages;

        public ChatInfo()
        {

        }

        public override bool Equals(object obj)
        {
            if (!(obj is Chat chatToCompare))
            {
                return false;
            }

            return Id == chatToCompare.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -1118474401;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<User>>.Default.GetHashCode(UsersInChat);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<MessageToChat>>.Default.GetHashCode(ChatMessages);
            return hashCode;
        }
    }
}
