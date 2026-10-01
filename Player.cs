using System;

namespace RPG
{
    [Serializable]
    public class Player
    {
        Random rand = new Random();

        public string name = "";
        public int id;
        public bool isDead = false;
        public bool hasWon = false;

        public int health = 10;
        public int maxHealth = 10;
        public int coins = 0;
        public int damage = 1;
        public int armour = 0;
        public int heals = 3;

        public int mods = 0;

        public int level = 1;
        public int xp = 0;
        public int kills = 0;

        public int storyStage = 0;

        public int textSpeed = 0;

        public int GetHealth()
        {
            int upper = 2 * mods + 5;
            int lower = mods + 2;
            return rand.Next(lower, upper);
        }

        public int GetPower()
        {
            int upper = 2 * mods + 2;
            int lower = mods + 1;
            return rand.Next(lower, upper);
        }

        public int GetCoins()
        {
            int upper = 20 * mods + 30;
            int lower = 10 * mods + 10;
            return rand.Next(lower, upper);
        }

        public int XpToNext()
        {
            return 15 * level;
        }

        public int AddXp(int amount)
        {
            xp += amount;
            int gained = 0;
            while (xp >= XpToNext())
            {
                xp -= XpToNext();
                level++;
                maxHealth += 3;
                health = Math.Min(maxHealth, health + 3);
                gained++;
            }
            return gained;
        }
    }
}

