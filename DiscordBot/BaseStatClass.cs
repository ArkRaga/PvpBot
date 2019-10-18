namespace DiscordBot
{
    public abstract class BaseStatClass
    {
        public string name = "";
        public string pvpClass = "Nothing";
        public string challenger;
        public int level = 1;
        public int battlePoints = 0;
        public int munnies = 0;
        public int maxHealth = 10;
        public int health = 0;
        public int maxExp = 3;
        public int exp = 0;
        public int damage = 3;
        public int defense = 0;
        public int speed = 2;
        public bool inCombat = false;
        public bool isDefending = false;
        public bool hasChallenge = false;
        public Action CurrentAction = Action.None;

        public enum Action { Attack, Defend, Crit, Miss, Skill, None };
        public bool IsDead()
        {
            return health <= 0;

        }
        public abstract Action GetAction();

        public virtual bool CombatAction(BaseStatClass enemy, Action action, ref string message)
        {

            //message = "Sorry, that's not a valid option, please try again";
            if (!IsDead())
            {

                switch (action)
                {
                    case Action.Attack:
                        int bonus = 0;
                        message += $"{name} has";
                        if (Program.rando.Next(1, 16) == 9)
                        {
                            //criting
                            bonus = 2;
                            message += "crit ";
                        }
                        if (enemy.isDefending)
                        {

                            bonus = -(damage - 1);
                            System.Console.WriteLine(bonus);

                        }

                        enemy.health -= (damage + bonus);
                        message += $" attacked for {(damage + bonus)} \n";
                        break;
                    case Action.Defend:

                        message += $"{name} has defended reduceing the damage to 1 \n";
                        isDefending = false;
                        break;

                    case Action.Miss:
                        message += $"{name} Missed for {damage}!\n";
                        break;

                    default:

                        break;

                }
                return true;
            }
            else
            {
                message += $"{name} Has Died! \n";
                return false;
            }


        }
    }

}
