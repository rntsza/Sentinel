using System;
using TwitchLib.Client.Models;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Api.V5.Models.Users;
using Sentinel.All;

namespace Sentinel
{
    internal class TwitchChatBot
    {
        //ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);
        ConnectionCredentials credentials = new ConnectionCredentials(Config.Default.BotUsername, Config.Default.BotToken);
        TwitchClient client;
        TwitchAPI api = new TwitchAPI();
        
        public TwitchChatBot()
        {

        }

        internal void Connect()
        {
            Console.WriteLine("Connecting");
            client = new TwitchClient();

            client.Initialize(credentials, Config.Default.ChannelName);
            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;
            // Until here, thats all ok

            // 
            client.OnMessageReceived += Client_OnMessageReceived;
            //

            client.Connect();

            api.Settings.ClientId = Config.Default.ClientID;
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message != null)
            {
                Console.WriteLine("Saved on database!");
                
                SaveDataBase(e.ChatMessage.DisplayName, e.ChatMessage.UserId, e.ChatMessage.Message);
            } 
        }

        TimeSpan? GetUpTime()
        {
            string userId = GetUserId(Config.Default.ChannelName);
            if(userId == null)
            {
                return null;
            }

            return api.V5.Streams.GetUptimeAsync(userId).Result;
        }

        string GetUserId(string username)
        {
            User[] userList = api.V5.Users.GetUserByNameAsync(username).Result.Matches;
            if(userList == null || userList.Length == 0)
            {
                return null;
            }
            return userList[0].Id;
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }
        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Erro! : {e.Error}");
        }

        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting");
        }

        private void SaveDataBase(string twitchid, string userid, string message)
        {
            ContextDB db = new ContextDB();
            TwitchUser user = new TwitchUser();

            user.Username = twitchid;
            user.Message = message;
            user.TwitchID = int.Parse(userid);

            db.TwitchUsers.Add(user);
            db.SaveChanges();
        }
    }
}