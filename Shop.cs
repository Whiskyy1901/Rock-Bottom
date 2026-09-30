using System;
using System.Xml;

namespace RPG
{
    public class Shop
    {
        static int armourMod;
        static int damageMod;
        static int healsMod;
        static int diffMod;
        public static void LoadShop(Player player)
        {
           RunShop(player);
        }

        public static void RunShop(Player player)
        {
            int armourC;
            int weaponC;
            int healC;
            int diffC;
            while (true)
            {
                armourC = 100 * (player.armour+1);
                weaponC = 50 * player.damage;
                healC = 30 + 10*player.mods;
                diffC = 500 * player.mods;
                Console.Clear();
                Console.WriteLine("             Shop             ");
                Console.WriteLine("==============================");
                Console.WriteLine("|  Item                Price |");
                Console.WriteLine("|(A)rmour                 "+armourC+"|");
                Console.WriteLine("|(W)eapon                 "+weaponC+"|");
                Console.WriteLine("|(H)eals                  "+healC+"|");
                Console.WriteLine("|(D)ifficulty increase    "+diffC+"|");
                Console.WriteLine("==============================");
                Console.WriteLine("            (E)xit            ");


                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("         Player Stats         ");
                Console.WriteLine("==============================");
                Console.WriteLine("Coins: "+player.coins);

                Console.WriteLine("Armour: "+player.armour);
                Console.WriteLine("Weapon Damage: "+player.damage);
                Console.WriteLine("Heals: "+player.heals);
                Console.WriteLine("Difficulty modifiers: "+player.mods);
                Console.WriteLine("==============================");

                string input = Console.ReadLine().ToLower();
                if(input == "a")
                {
                    Buy("Armour", armourC, player);
                }
                else if(input == "w")
                {
                    Buy("Weapon", weaponC, player);
                }
                else if(input == "h")
                {
                    Buy("Heals", healC, player);
                }
                else if(input == "d")
                {
                    Buy("Difficulty", diffC, player);
                }
                else if(input == "e")
                {
                    break;
                }
            }
        }

        static void Buy(string item, int cost, Player player)
        {
            if(player.coins >= cost)
            {
                if(item == "Armour")
                    player.armour++;
                else if(item == "Weapon")
                    player.damage++;
                else if(item == "Heals")
                    player.heals++;
                else if(item == "Difficulty")
                    player.mods++;
                
                player.coins -= cost;
            }
            else
            {
                Console.WriteLine("You can't afford this.");
                Console.WriteLine("Can somebody get these BEGGARS out of here");
                Console.ReadKey();
            }
        }
    }
}