using System;

namespace RPG
{
    public class Shop
    {
        public static void LoadShop(Player player)
        {
            RunShop(player);
        }

        static string Row(string label, int price)
        {
            return "|" + label.PadRight(21) + price.ToString().PadLeft(6) + " |";
        }

        public static void RunShop(Player player)
        {
            string message = "";
            bool messageGood = true;

            while (true)
            {
                int armourC = 100 * (player.armour + 1);
                int weaponC = 50 * player.damage;
                int healC = 30 + 10 * player.mods;
                int diffC = 500 * (player.mods + 1);

                Console.Clear();
                Program.PrintColor("             Shop             ", ConsoleColor.Cyan, 2);
                Program.Print("==============================", 2);
                Program.Print("|  Item                Price |", 2);
                Program.Print(Row("(A)rmour", armourC), 2);
                Program.Print(Row("(W)eapon", weaponC), 2);
                Program.Print(Row("(H)eals", healC), 2);
                Program.Print(Row("(D)ifficulty increase", diffC), 2);
                Program.Print("==============================", 2);
                Program.Print("            (E)xit            ", 2);
                Program.Print("         (Q)uit Game          ", 2);
                Program.Print("Difficulty: tougher beasts, bigger rewards", 2);

                Console.WriteLine();

                Program.PrintColor("         Player Stats         ", ConsoleColor.Cyan, 2);
                Program.Print("==============================", 2);
                Program.PrintColor("Coins: " + player.coins, ConsoleColor.Yellow, 2);
                Program.Print("Level: " + player.level + "   Health: " + player.health + "/" + player.maxHealth, 2);
                Program.Print("Armour: " + player.armour, 2);
                Program.Print("Weapon Damage: " + player.damage, 2);
                Program.Print("Heals: " + player.heals, 2);
                Program.Print("Difficulty modifiers: " + player.mods, 2);
                Program.Print("==============================", 2);

                if (message != "")
                    Program.PrintColor(message, messageGood ? ConsoleColor.Green : ConsoleColor.Red, 10);
                message = "";

                string input = (Console.ReadLine() ?? "").Trim().ToLower();
                if (input == "a")
                {
                    messageGood = Buy("Armour", armourC, player, out message);
                }
                else if (input == "w")
                {
                    messageGood = Buy("Weapon", weaponC, player, out message);
                }
                else if (input == "h")
                {
                    messageGood = Buy("Heals", healC, player, out message);
                }
                else if (input == "d")
                {
                    messageGood = Buy("Difficulty", diffC, player, out message);
                }
                else if (input == "q")
                {
                    Program.Quit();
                }
                else if (input == "e")
                {
                    break;
                }
            }
        }

        static bool Buy(string item, int cost, Player player, out string message)
        {
            if (player.coins < cost)
            {
                message = "You can't afford this. Can somebody get these BEGGARS out of here!";
                return false;
            }

            if (item == "Armour")
                player.armour++;
            else if (item == "Weapon")
                player.damage++;
            else if (item == "Heals")
                player.heals++;
            else if (item == "Difficulty")
                player.mods++;

            player.coins -= cost;
            message = "Bought: " + item + ".";
            return true;
        }
    }
}