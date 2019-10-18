using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DiscordBot
{
    class ComandHandler
    {

        private DiscordSocketClient _client;
        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services:null);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argpos = 0;

            // a user runs a command
            // the bot checks whether the user has been blocked
            // if they have been blocked, then we should ignore the command from the user
            // if the user is not blocked, then we should not ignore the user and run the command if it is valid

            if(msg.HasStringPrefix(Config.bot.cmdPrefix, ref argpos) || msg.HasMentionPrefix(_client.CurrentUser, ref argpos) )
            {
                var result = await _service.ExecuteAsync(context, argpos, services:null);
                if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);

                }

            }
        }
    }
}
