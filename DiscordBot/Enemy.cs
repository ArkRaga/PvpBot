using System;

namespace DiscordBot
{
    public class Enemy: BaseStatClass
    {
        public int id = 0;

        

        public Enemy() { }
        public Enemy(string Name = "testy", int hp = 0, int damage = 0, int def = 0, int spd = 0)
        {
            name = Name;
            health = hp;
            this.speed = spd;
            this.defense = def;
            this.damage = damage;
        }

        public Enemy(Enemy enemy, int level) : this(enemy.name, enemy.health, enemy.damage, enemy.defense, enemy.speed)
        {

            ScaleEnemy(level);

        }
       

        public void ScaleEnemy(int level)
        {
            if (level > 1)
            {
                int statmod = level + 1;
                health += statmod;
                damage += statmod;
                defense += statmod;
                speed += statmod;
            }
        }
        public override Action GetAction()        {
            
            switch(Program.rando.Next(1,6))
            {
                case 1:
                case 2:
                    return Action.Attack;
                case 3:
                    return Action.Crit;
                case 4:
                case 5:
                    return Action.Miss;
            }
            return Action.Miss;
        }



    }
}
