using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        public static  List<PvpPlayer> PvpPlayers = new List<PvpPlayer>();
        public static  List<Combat> combats = new List<Combat>();
        public static List<Enemy> enemyList = new List<Enemy>();
        public static List<PvpClass> playerClasses = new List<PvpClass>();
        public static Random rando = new Random();

        DiscordSocketClient _client;
        ComandHandler _handler;
       

        static void Main(string[] args)
       => new Program().StartAsync().GetAwaiter().GetResult();
        
        public async Task StartAsync()
        {
            DataStorarge.LoadData();
            Console.WriteLine(playerClasses.Count);
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {

                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            _handler = new ComandHandler();

            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
           
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
           

        }
    }
}
