using System;
using TwitchLib;
using TwitchLib.Client.Models;
using TwitchLib.Api.V5.Models.UploadVideo;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Common;
using TwitchLib.Api.Events;
using TwitchLib.Communication.Events;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Events;
using TwitchLib.Client.Enums;
using TwitchLib.Api.V5.Models.Users;

namespace Sentinel
{
    internal class TwitchChatBot
    {

        ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);
        //ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.TwitchChatOAuthPasswordGenerator);
        TwitchClient client;
        TwitchAPI api = new TwitchAPI();
        
        public TwitchChatBot()
        {

        }

        internal void Connect()
        {
            Console.WriteLine("Connecting");
            client = new TwitchClient();

            client.Initialize(credentials, TwitchInfo.ChannelName);
            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;
            // Até aqui tá ótimo

            // Receber mensagem e mendar resposta.
            client.OnMessageReceived += Client_OnMessageReceived;
            //


            client.Connect();

            api.Settings.ClientId = TwitchInfo.ClientID;
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if(e.ChatMessage.Message.StartsWith("Salve", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage(TwitchInfo.ChannelName, $"Salve {e.ChatMessage.DisplayName}");
            }
            else if(e.ChatMessage.Message.StartsWith("!uptime", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage(TwitchInfo.ChannelName, GetUpTime()?.ToString() ?? "Offline");
            }
        }

        TimeSpan? GetUpTime()
        {
            string userId = GetUserId(TwitchInfo.ChannelName);
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
            //Console.WriteLine(e.Data);
        }
        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Erro! : {e.Error}");
        }



        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting");
        }
    }
}