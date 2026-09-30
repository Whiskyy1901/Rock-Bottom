using System;

namespace RPG
{
    [Serializable]
    public class Player
    {
        Random rand = new Random();

        public string name;
        public int id;
        public bool isDead = false;
        public int health = 10;
        public int coins = 0;
        public int damage = 1;
        public int armour = 0;
        public int heals = 3;

        public int mods = 0;

        public int GetHealth()
        {
            int upper = 2*mods+5;
            int lower = mods +2;
            return rand.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = 2*mods+2;
            int lower = mods +1;
            return rand.Next(lower, upper);
        }
        public int GetCoins()
        {
            int upper = 20*mods+30;
            int lower = 10*mods +10;
            return rand.Next(lower, upper);
        }
    }
}