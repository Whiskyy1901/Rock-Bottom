using System;

namespace RPG
{
    public class Enemy
    {
        static Random rand = new Random();

        public string name;
        public int power;
        public int health;
        public int maxHealth;
        public int xpReward;
        public bool isBoss;

        public Enemy(string name, int power, int health, int xpReward = 0, bool isBoss = false)
        {
            this.name = name;
            this.power = power;
            this.health = health;
            this.maxHealth = health;
            this.xpReward = xpReward;
            this.isBoss = isBoss;
        }

        public static Enemy CreateRandom(Player p)
        {
            int power = p.GetPower() + p.level / 2;
            int health = p.GetHealth() + p.level;
            string name;

            switch (rand.Next(0, 6))
            {
                case 0:
                    name = "Snake";
                    power += 1;
                    health = Math.Max(1, health - 1);
                    break;
                case 1:
                    name = "Lizard";
                    break;
                case 2:
                    name = "Sloth";
                    health *= 2;
                    power = Math.Max(1, power - 1);
                    break;
                case 3:
                    name = "Spider";
                    power += 1;
                    break;
                case 4:
                    name = "Mosquito";
                    health = Math.Max(1, health / 2);
                    break;
                default:
                    name = "Fly";
                    health = 1;
                    power = Math.Max(1, power - 1);
                    break;
            }

            int xp = 5 + 3 * p.mods;
            return new Enemy(name, power, health, xp);
        }
    }
}