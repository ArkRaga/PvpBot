using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace DiscordBot.Modules
{

    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("startPvp")]
        #region
        public async Task Startpvp()
        {
            string user = Context.User.Username;
            var dmChannle = await Context.User.GetOrCreateDMChannelAsync();
            if (!IsAPlayer(user))
            {


                await dmChannle.SendMessageAsync($"Welcome {user} Please pick a **class** from the list below.\n"
                                                 + string.Join("\n", Program.playerClasses.Select(x => x.name)) + "\n"
                                                 + "Use **$pickclass** then enter the class name to select a class to be. \n"
                                                 + "classname needs to be typed the same :)");

            }
            else { await Context.Channel.SendMessageAsync($"{user} your already playing the game."); }

        }
        #endregion
        [Command("Hunt")]
        #region
        public async Task Monster()
        {
            string name = Context.User.Username;
            PvpPlayer player = Program.PvpPlayers.FirstOrDefault(p => p.name == name);
            Enemy enm = Combat.GenRandomEnemy(player.level);

            if (!player.inCombat)
            {
                Program.combats.Add(new Combat(player, enm));

                await Context.Channel.SendMessageAsync($"{name}, a wild {enm.name} has appeared!\n"
                                                        + $"What would you like to do?" 
                                                        + "[Attack] [Defend]");
            }
            else
            {
                //look thur list and get the enemy this person is already fighting.
                Combat combat = Program.combats.FirstOrDefault(e => e.player.name == name);
                await Context.Channel.SendMessageAsync($"{name}, you are currently in combat with a {combat.player2.name}.");
            }


        }
        #endregion
        [Command("attack")]
        #region
        public async Task Attack()
        {
            string user = Context.User.Username;
            Combat combat = Program.combats.FirstOrDefault(c => c.player.name == user|| c.player2.name == user);
            var embed = new EmbedBuilder();
            embed.WithTitle($"**Battle Round: {combat.Round}**");
            embed.WithDescription(VaildateAttack("Attack"));
            if (combat.player.health >= 5)
            { embed.WithColor(new Color(0, 255, 0)); }
            else { embed.WithColor(new Color(105, 1, 1)); }
            await Context.Channel.SendMessageAsync("", false, embed.Build());

        }
        #endregion
        [Command("defend")]
        #region
        public async Task Defend()
        {
            string user = Context.User.Username;
            Combat combat = Program.combats.FirstOrDefault(c => c.player.name == user|| c.player2.name == user);
            var embed = new EmbedBuilder();
            embed.WithTitle($"**Battle Round: {combat.Round}**");
            combat.player.isDefending = true;
            embed.WithDescription(VaildateAttack("Defend"));
            if (combat.player.health >= 5)
            { embed.WithColor(new Color(0, 255, 0)); }
            else { embed.WithColor(new Color(105, 1, 1)); }
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        #endregion
        [Command("Getid")]
        #region
        public async Task GetID()
        {

            await Context.Guild.GetTextChannel(Utilities.TestChan).SendMessageAsync("It works");
        }
        #endregion
        [Command("Commands")]
        #region
        public async Task Commands()
        {
            await Context.Channel.SendMessageAsync(Utilities.GetAlert("Commands"));

        }
        #endregion
        [Command("Duel")]
        #region
        public async Task Duel([Remainder] string person = "")
        {
             var user = Context.User;

            var target = Context.Message.MentionedUsers.FirstOrDefault();
            if(!IsAPlayer(user.Username))
            {
                await Context.Channel.SendMessageAsync("Sorry you have to be a player in the pvp game");
                return;
            }
            else
            {
                PvpPlayer player = GetPlayer(user.Username);
                if(player.hasChallenge)
                {
                  await Context.Channel.SendMessageAsync($"Sorry {user.Mention}, you have a Challenge already");
                }
                else
                {
                    player.hasChallenge = true;
                    player.challenger = target.Username;
                    
                }
                

            }
         
            if(!IsAPlayer(target.Username))
            {
                await Context.Channel.SendMessageAsync($"Sorry {user} They have to be a player of the game.");
                return;
            }
            else
            {
                PvpPlayer enemy = GetPlayer(target.Username);
                if(enemy.hasChallenge)
                {
                    await Context.Channel.SendMessageAsync($"Sorry {user} They have a Challenge currently, please try again later");
                }
                else
                {
                    enemy.hasChallenge = true;
                    enemy.challenger = user.Username;
                    Console.WriteLine($"enemy is a user and {enemy.challenger} is target");
                }

            }
                
            var embed = new EmbedBuilder();
            embed.WithTitle("**Battle Announcement!**");
            embed.WithDescription(Utilities.GetFormattedAlert("Duel", user.Mention,target.Mention));
            embed.WithColor(new Color(105, 1, 1));
            await Context.Guild.GetTextChannel(Utilities.TestChan).SendMessageAsync("", false, embed.Build());

        }
        #endregion
        [Command("Accept")]
        #region
        public async Task Accept()
        {
            var user = Context.User;
            PvpPlayer player = GetPlayer(user.Username);
            if (player == null)
            {await Context.Channel.SendMessageAsync($"{user.Username} You need to be playing the pvp game to use that.");return;}
            if(player.hasChallenge)
            {
                PvpPlayer enemy = GetPlayer(player.challenger);
                if (enemy.challenger == user.Username)
                {
                    player.CurrentAction = BaseStatClass.Action.None;
                    Program.combats.Add(new Combat(player, enemy));

                    var target = Context.Guild.Users.FirstOrDefault(u => u.Username == player.challenger);
                    
                    var embed = new EmbedBuilder();
                    embed.WithTitle("**Battle Announcement!**");
                    embed.WithDescription(Utilities.GetFormattedAlert("DuelAccept", user.Mention,target.Mention));
                    embed.WithColor(new Color(252, 254, 0));
                    await Context.Guild.GetTextChannel(Utilities.TestChan).SendMessageAsync("", false, embed.Build());

                }
                
                
            }
            else
            {
                await Context.Channel.SendMessageAsync($"{user} You have no current challenge to accept :(");
            }



        }
        #endregion
        [Command("Deny")]
        #region
        public async Task Deny()
        {
            var user = Context.User;
            PvpPlayer player = GetPlayer(user.Username);
            if(player == null)
            { await Context.Channel.SendMessageAsync($"{user.Mention} you need to be playing the Pvp game to use this command"); }
            else
            {
                if (player.hasChallenge) 
                {
                    PvpPlayer enemy = GetPlayer(GetChallenger(player));
                    var targetPlayer = Context.Guild.Users.FirstOrDefault(e => e.Username == enemy.name);
                    await Context.Channel.SendMessageAsync($"Sorry {targetPlayer.Mention}, {user.Username} Does not wish to combat you right now.");
                    enemy.hasChallenge = false;
                    enemy.challenger = "none";
                    player.hasChallenge = false;
                    player.challenger = "none";
                }
                else 
                { await Context.Channel.SendMessageAsync($"{user.Mention} you do not have a challenge yet, go pick a fight."); }
            }
        }
        #endregion
        [Command("pick")]
        #region
        public async Task PickOne([Remainder] string message)
        {

            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];
            await Context.Channel.SendMessageAsync(selection);

        }
        #endregion
        [Command("Help")]
        #region
        public async Task Hello()
        {

            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("Welcome_&Name", Context.User.Username));
        }
        #endregion
        [Command("ClassList")]
        #region
        public async Task PVPClasses()
        {
            string names="";
            for (int i = 0; i < Program.playerClasses.Count; i++)
            {
                if(!Program.playerClasses[i].isLocked)
                {
                    names += Program.playerClasses[i].name + " \n";
                }

            }

            await Context.Channel.SendMessageAsync(names);
           
            //await Context.Channel.SendMessageAsync(mystring.Join("\n", Program.playerClasses.Select(x => x.name)));

        }
        #endregion
        [Command("Mystats")]
        #region
        public async Task MyStats()
        {

            string user = Context.User.Username;

            if (IsAPlayer(user))
            {
                var player = DataStorarge.GetStats(user);
                await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("Stats", player.name, player.pvpClass, player.battlePoints, player.level));
            }
            else
            {
                await Context.Channel.SendMessageAsync("You were not found, Sorry!");
            }
        }
        #endregion
        [Command("Secret")]
        #region
        public async Task Secret([Remainder]string arg = "")
        {
            if (!DoesUserHaveRole((SocketGuildUser)Context.User, "SecretOwner"))
            {
                await Context.Channel.SendMessageAsync(":x: Sorry " + Context.User.Mention + ", you don't have the role for that :(");
                return;
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("Secret"));

        }
        #endregion
        [Command("Data")]
        #region
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync(Program.PvpPlayers.Count.ToString());
            DataStorarge.SaveData();
        }
        #endregion
        [Command("pickClass")]
        #region
        public async Task PickCLass([Remainder] string classname)
        {
            string user = Context.User.Username;
            var pmchannel = await Context.User.GetOrCreateDMChannelAsync();
            if (IsPlayer(user, classname))
            {
                DataStorarge.SaveData();
                string message = $"Welcome {Context.User.Mention} the {classname} to the Pvp world! \n **Fresh Meat Boys!!**";
                await pmchannel.SendMessageAsync("Go forth and fight!\n" + Utilities.GetAlert("HowToPlay"));
                await Context.Channel.SendMessageAsync($"Congratz {user}, you're a {classname}");
                await Context.Client.GetGuild(Utilities.GuildChan).GetTextChannel(Utilities.GenChan).SendMessageAsync(message);
            }

        }
        #endregion
        [Command("PrintPlayers")]
        #region
        public async Task PrintPlayers([Remainder] int I = 0)
        {

            string num = Program.PvpPlayers.Count.ToString();
            await Context.Channel.SendMessageAsync(num);
            await Context.Channel.SendMessageAsync(Program.PvpPlayers[I].name);
            Console.WriteLine(Program.PvpPlayers.Count.ToString());

        }
        #endregion
        [Command("Quitpvp")]
        #region
        public async Task QuitPVP()
        {
            string name = Context.User.Username;
            PvpPlayer player = Program.PvpPlayers.FirstOrDefault(p => p.name == name);
            if (player != null)
            {
                if(player.hasChallenge)
                {
                    PvpPlayer enemy = GetPlayer(GetChallenger(player));
                    enemy.hasChallenge = false;
                    enemy.challenger = "";
                    enemy.exp += 2;
                    enemy.battlePoints += 2;
                    Combat combat = Program.combats.FirstOrDefault(p => p.player.name == player.name || p.player2.name == player.name);
                    Program.combats.Remove(combat);
                }
                Program.PvpPlayers.Remove(player);

                DataStorarge.SaveData();
                await Context.Channel.SendMessageAsync($"{name}, you have been removed from the game.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You're not playing");

            }


        }
        #endregion

        //Methonds----------------------------------------------------------------------------------------|
        //------------------------------------------------------------------------------------------------|
        private bool DoesUserHaveRole(SocketGuildUser user, string targetRoleName)
        {
            //targetRoleName = "";
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }
        public bool IsPlayer(string name, string pickedclass)
        {

            PvpPlayer player = Program.PvpPlayers.FirstOrDefault(p => p.name == name);
            if (player == null)
            {

                PvpClass playerPickedClass = Program.playerClasses.FirstOrDefault(p => p.name == pickedclass);
                if (playerPickedClass != null && playerPickedClass.isLocked == false)
                {
                    PvpPlayer newplayer = new PvpPlayer(name, playerPickedClass.name);
                    newplayer.SetStats(playerPickedClass);
                    Program.PvpPlayers.Add(newplayer);
                    DataStorarge.SaveData();
                    return true;
                }
                else
                {

                    Context.Channel.SendMessageAsync("Sorry invalid class options, Please try again");
                    return false;
                }


            }
            else
            {
                Context.Channel.SendMessageAsync($"You're already a {player.pvpClass}");
                return false;
            }

        }
        public bool IsAPlayer(string name)
        {

            PvpPlayer player = Program.PvpPlayers.FirstOrDefault(p => p.name == name);
            if (player == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public PvpPlayer GetPlayer(string name)
        {
            PvpPlayer player = Program.PvpPlayers.FirstOrDefault(p => p.name == name);
            if (player == null)
            {
                return null;
            }
            else
            {
                return player;
            }

        }
        
        public string GetChallenger(PvpPlayer player)
        {
            return player.challenger;

        }
        public string VaildateAttack(string action)
        {
            string user = Context.User.Username;
            if (IsAPlayer(user))
            {
                PvpPlayer player = Program.PvpPlayers.FirstOrDefault(p => p.name == user);
                if (player.inCombat)
                {
                    
                    Combat combat = Program.combats.FirstOrDefault(c => c.player.name == player.name || c.player2.name == player.name);
                    player.CurrentAction = combat.ConvertAction(action);
                    return combat.DoCombat(action);
                }
                else
                {
                    return $"{user}, you're not currently in a combat.";
                }
            }
            else
            {
                return $"Sorry {user}, you have to be playing pvp for that to work.";
            }
        }


    }
}
