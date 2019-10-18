using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    

   public class PvpClass
    {
        public string name;
        public int attack;
        public int defense;
        public int speed;
        public bool isLocked = false;
       
        public PvpClass() { }
        public PvpClass(string Name, int Attack, int Defense, int Speed, bool isLocked = false)
        {
            name = Name;
            attack = Attack;
            defense = Defense;
            speed = Speed;

        }
    }
}
