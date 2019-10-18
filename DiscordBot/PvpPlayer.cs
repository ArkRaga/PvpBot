using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
   
    public class PvpPlayer: BaseStatClass
    {

        public PvpPlayer(string Pname, string Pclass = "Nothing", int BP = 0, int muns = 0)
        {
            name = Pname;
            pvpClass = Pclass ;
            battlePoints = BP;
            munnies = muns;
            health = maxHealth;

        }
        public override Action GetAction()
        {
            return CurrentAction;
        }

        public void Reset()
        {
            health = maxHealth;
            //exp = 0;
            inCombat = false;
            isDefending = false;

        }
        public void LevelUp()
        {
            int statmod = 1;
            level++;
            maxHealth += statmod;
            maxExp += statmod;
            damage += statmod;
            defense += statmod;
        }
        public void SetStats(PvpClass pclass)
        {
            this.damage = pclass.attack;
            this.defense = pclass.defense;
            this.speed = pclass.speed;

        }
        
    }
}
