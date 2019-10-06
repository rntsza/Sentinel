using System;


namespace Sentinel
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitchChatBot bot = new TwitchChatBot();
            bot.Connect();

            /*try
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch
            {

            }*/
            Console.ReadLine();

            bot.Disconnect();
        }
    }
}
