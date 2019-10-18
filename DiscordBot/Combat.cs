using System;

namespace DiscordBot
{
    class Combat
    {
        public int Round = 1;
        public BaseStatClass player;
        public BaseStatClass player2;
        private string message;
        public Combat(BaseStatClass player, BaseStatClass player2)
        {
            this.player = player;
            this.player2 = player2;

            player.inCombat = true;
            player2.inCombat = true;
            //Console.WriteLine($"player:{this.player.name} has entered combat with {this.enemy.enName}");
        }

        public string DoCombat(string playerAction)
        {   string message = "";
            //if (player.GetAction() == BaseStatClass.Action.None  || player2.GetAction() == BaseStatClass.Action.None) return $"Waiting on  Action";
            if (player.GetAction() == BaseStatClass.Action.None)
            {
               return message = $"Waitng on {player.name}'s action \n";

            }
            if (player2.GetAction() == BaseStatClass.Action.None)
            {
                return message = $"Waitng on {player2.name}'s action \n";

            }

            CheckSpeed(out BaseStatClass Hero, out BaseStatClass Enemy);
            
            
            
            
            Round++;
            if (!Hero.CombatAction(Enemy, ConvertAction(playerAction), ref message))
            { DoResults(Hero,ref message); DoResults(Enemy,ref message); return message; }

            if (!Enemy.CombatAction(Hero, Enemy.GetAction(), ref message))
            { DoResults(Hero,ref message); DoResults(Enemy,ref message); return message; }
           

            if (Hero.IsDead())
            {

                Hero.CombatAction(Enemy, ConvertAction(playerAction), ref message);
                DoResults(Hero,ref message); DoResults(Enemy,ref message); return message;
            }

            message += $"{Hero.name} Hp: {Hero.health} \n" +
                    $"{Enemy.name} Hp: {Enemy.health} \n " +
                    $"What is your next action ? \n "+
                    "    [$Attack] [$Defend]    ";


            Hero.CurrentAction = BaseStatClass.Action.None;
            Enemy.CurrentAction = BaseStatClass.Action.None;
            return message;
            
        }
        public static Enemy GenRandomEnemy(int level)
        {
            Random rand = new Random();
            int index = rand.Next(0, Program.enemyList.Count);
            //pulls the Enemy from the list "orginalENEMY"
            Enemy orginalEnemy = Program.enemyList[index];
            //Creates new enemy using "orginal ENEMY as param"
            Enemy en = new Enemy(orginalEnemy, level);

            return en;
        }

        public void DoResults(BaseStatClass person, ref string message)
        {
            if (person is Enemy) return;
            person.challenger = "";
            person.hasChallenge = false;
            person.CurrentAction = BaseStatClass.Action.None;
            if (!person.IsDead())
            {
                person.exp += 2; person.battlePoints +=  4;

            }else
            {
                person.exp += 1; person.battlePoints += 3;
            }
            if (person.exp >= person.maxExp)
            {
              (person as PvpPlayer).LevelUp(); 
              message += $"{person.name} has LEVELED up to level {person.level}! \n"; 
            }
            (person as PvpPlayer).Reset();
            DataStorarge.SaveData();
            Program.combats.Remove(this);

        }
        public BaseStatClass.Action ConvertAction(string Saction)
        {
            switch (Saction)
            {
                case "Attack":
                    return BaseStatClass.Action.Attack;
                case "Defend":
                    return BaseStatClass.Action.Defend;
                default:
                    return BaseStatClass.Action.Attack;
                    
            }

        }
        public void CheckSpeed(out BaseStatClass Hero, out BaseStatClass Enemy)
        {
            if(this.player.speed > this.player2.speed)
            {
                Hero = this.player;
                Enemy = this.player2;
            }
            else
            {
                Hero = this.player2;
                Enemy = this.player;
            }
        }
       
        

    }

}
