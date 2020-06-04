using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Forms
{
    public class ViewModel
    {
        private Model Model { get; set; }


        public Action<ChatInfo> NewChatIsAdded;

        public Action<string, bool> ErrorIsGotten;

        public Action<ChatMessage, ChatInfo> NewMessageInChat;

        public Action<string> UserLoginIsUpdated;


        public ViewModel()
        {
            Model = new Model();
            Model.ErrorIsGotten += OnErrorIsGotten;
            Model.NewMessageInChat += OnNewMessageInChat;
            Model.UserIsGottenFromServer += SetUser;
        }


        public void AddChat(string chatName, List<string> loginsToAdd)
        {
            Model.AddChat(chatName, loginsToAdd);
        }

        public void CloseConnection()
        {
            Model.CloseConnection();
        }

        public UserInfo GetUser()
        {
            return Model.Messenger.User;
        }

        public void Initialize()
        {
            Model.InitializeComponents();
            Model.Messenger.UserChats.CollectionChanged += OnNewChatInContactList;

        }

        public void OnNewChatLoginsEntered(ChatInfo chat, List<string> logins)
        {
            Model.AddUsersToChat(chat, logins);
        }

        public void SendMessageToChat(string userInput, ChatInfo chosenChat)
        {
            Model.SendMessageToChat(userInput, chosenChat);
        }


        private void OnErrorIsGotten(string error, bool closeApp)
        {
            ErrorIsGotten(error, closeApp);
        }

        private void OnNewChatInContactList(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!(e.NewItems[0] is ChatInfo chat))
                {
                    return;
                }

                NewChatIsAdded(chat);
            }
        }

        private void OnNewMessageInChat(ChatMessage message, ChatInfo chat)
        {
            NewMessageInChat(message, chat);
        }

        private void SetUser()
        {
            UserLoginIsUpdated(GetUser().Login);
        }
    }
}
