using System;

namespace Sentinel.All
{
    public class TwitchUser
    {
        public TwitchUser()
        {
        }

        public TwitchUser(int twitchID, string username, string message)
        {
            TwitchID = twitchID;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }


        #region Geral
        public int Id { get; set; }
        public int TwitchID { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        #endregion
    }
}
