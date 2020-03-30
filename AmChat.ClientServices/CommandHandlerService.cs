using AmChat.ClientServices.CommandHandlers;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class CommandHandlerService
    {
        Dictionary<string, ICommandHandler> CommandHandlers { get; set; }

        ClientMessengerService Messenger { get; set; }

        public ObservableCollection<Chat> UserChats { get; set; }

        public Chat ChosenChat { get; set; }


        public Action<string> MessageToCurrentChatIsGotten;

        public Action<ChatMessage> MessageToOtherChatIsGotten;

        public Action<string> MessageCorretlySend;

        public Action<Chat> ChatAdded;

        public Action CorrectLoginData;

        public Action IncorrectLoginData;

        public Action<string, bool> ErrorIsGotten;

        public Action<Guid> NewUnreadNotification;

        public CommandHandlerService(ClientMessengerService messenger)
        {
            Messenger = messenger;

            UserChats = new ObservableCollection<Chat>();
            UserChats.CollectionChanged += AddChatToContactList;
            Messenger.UserChats = UserChats; // TEMPORARY

            InitializeCommandsHandlers();
        }

        public void ProcessMessage(string message)
        {
            var command = JsonParser<Command>.JsonToOneObject(message);

            var handler = CommandHandlers[command.Name];

            handler.Execute(Messenger, command.Data);
        }



        private void InitializeCommandsHandlers()
        {
            CommandHandlers = new Dictionary<string, ICommandHandler>();

            var correctLoginHandler = new CorrectLoginHandler();
            correctLoginHandler.UserIsLoggedIn += OnUserIsLoggedIn;

            var incorrectLoginHandler = new IncorrectLoginHandler();
            incorrectLoginHandler.IncorrectLoginData += OnIncorrectLoginData;

            var serverErrorHandler = new ServerErrorHandler();
            serverErrorHandler.SendError += ShowError;

            var unreadMessagesInChatHandler = new UnreadMessagesInChatHandler();
            unreadMessagesInChatHandler.NewUnreadNotification += OnNewUnreadNotification;


            CommandHandlers.Add(nameof(ChatIsAdded).ToLower(), new ChatIsAddedHandler());
            CommandHandlers.Add(nameof(CorrectContactList).ToLower(), new CorrectContactListHandler());
            CommandHandlers.Add(nameof(CorrectLogin).ToLower(), correctLoginHandler);
            CommandHandlers.Add(nameof(IncorrectLogin).ToLower(), incorrectLoginHandler);
            CommandHandlers.Add(nameof(MessageToCertainChat).ToLower(), new MessageToCertainChatHandler());
            CommandHandlers.Add(nameof(ServerError).ToLower(), serverErrorHandler);
            CommandHandlers.Add(nameof(UnreadMessagesInChat).ToLower(), unreadMessagesInChatHandler);
        }


        private void OnIncorrectLoginData()
        {
            IncorrectLoginData();
        }

        private void OnUserIsLoggedIn()
        {
            CorrectLoginData();
        }

        private void OnNewUnreadNotification(Guid chatId)
        {
            NewUnreadNotification(chatId);
        }

        private void ShowError(string errorText)
        {
            ErrorIsGotten(errorText, false);
        }


        public void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            var newInfoChat = new NewChatInfo(chatName, userLoginsToAdd);
            var newChatInfoJson = JsonParser<NewChatInfo>.OneObjectToJson(newInfoChat);

            var command = new AddOrUpdateChat() { Data = newChatInfoJson };
            var commandJson = JsonParser<AddOrUpdateChat>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);
        }

        public void AddUsersToChat(Chat chat, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chat.Id, userLoginsToAdd);
            var newChatInfoJson = JsonParser<NewChatInfo>.OneObjectToJson(newChatInfo);

            var command = new AddOrUpdateChat() { Data = newChatInfoJson };
            var commandJson = JsonParser<AddOrUpdateChat>.OneObjectToJson(command);
            
            Messenger.SendMessage(commandJson);
        }


        private void AddChatToContactList(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is Chat newChat))
            {
                return;
            }

            newChat.ChatMessages.CollectionChanged += ShowNewMessage;
            ChatAdded(newChat);
        }

        private void ShowNewMessage(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (!(e.NewItems[0] is ChatMessage message))
            {
                return;
            }



            if (message.FromUser.Equals(Messenger.User))
            {
                MessageCorretlySend(message.Text);
            }
            else
            {

                if (ChosenChat == null || ChosenChat.Id != message.ToChatId)
                {
                    var chatToShowMessage = UserChats.Where(c => c.Id == message.ToChatId).FirstOrDefault();

                    MessageToOtherChatIsGotten(message);
                }
                else
                {
                    var messageToShow = message.FromUser.Login + ":\n" + message.Text;
                    MessageToCurrentChatIsGotten(messageToShow);
                }
            }
        }

        public void SendMessageToChat(string message)
        {
            var messageToChat = new ChatMessage()
            {
                FromUser = Messenger.User,
                ToChatId = ChosenChat.Id,
                DateAndTime = DateTime.Now,
                Text = message,
            };

            var messageToUserJson = JsonParser<ChatMessage>.OneObjectToJson(messageToChat);

            var command = new SendMessageToChat() { Data = messageToUserJson };
            var commandJson = JsonParser<SendMessageToChat>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);

            ChosenChat.ChatMessages.Add(messageToChat);
        }

        public void CloseConnection()
        {
            try
            {
                var command = new CloseConnection() { Data = string.Empty };
                var commandJson = JsonParser<CloseConnection>.OneObjectToJson(command);
                
                Messenger.SendMessage(commandJson);
            }
            catch
            {

            }
        }

        public void GetChats()
        {
            var command = new GetChats() { Data = string.Empty };
            var commandJson = JsonParser<GetChats>.OneObjectToJson(command);
            
            Messenger.SendMessage(commandJson);
        }
    }
}
