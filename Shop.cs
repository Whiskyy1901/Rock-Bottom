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
                Program.Print("             Shop             ");
                Program.Print("==============================");
                Program.Print("|  Item                Price |");
                Program.Print("|(A)rmour                 "+armourC+"|");
                Program.Print("|(W)eapon                 "+weaponC+"|");
                Program.Print("|(H)eals                  "+healC+"|");
                Program.Print("|(D)ifficulty increase    "+diffC+"|");
                Program.Print("==============================");
                Program.Print("            (E)xit            ");
                Program.Print("         (Q)uit Game          ");


                Console.WriteLine();
                Console.WriteLine();

                Program.Print("         Player Stats         ");
                Program.Print("==============================");
                Program.Print("Coins: "+player.coins);

                Program.Print("Armour: "+player.armour);
                Program.Print("Weapon Damage: "+player.damage);
                Program.Print("Heals: "+player.heals);
                Program.Print("Difficulty modifiers: "+player.mods);
                Program.Print("==============================");

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
                else if(input == "q")
                {
                    Program.Quit();
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
                Program.Print("You can't afford this.");
                Program.Print("Can somebody get these BEGGARS out of here");
                Console.ReadKey();
            }
        }
    }
}