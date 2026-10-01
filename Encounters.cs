using System;

namespace RPG
{
    public enum CombatResult { Won, Fled }

    public class Encounters
    {
        static Random rand = new Random();

        public const int BossUnlockKills = 7;
        const int TRexHealth = 25;
        const int TRexPower = 4;

        public static void FirstEncounter()
        {
            Program.Print("Outside the cave, you see a man with a wooden club.");
            Program.Print("You assume that must be the man that bruised you.");
            Program.Print("You pickup the nearest rock and charge towards him.");
            Program.Pause();

            Enemy caveman = new Enemy("Caveman", 2, 2, 5);
            if (Combat(caveman) == CombatResult.Won)
            {
                Victory(caveman);
            }
            else
            {
                Program.Print("You sprint into the trees. The caveman shouts something rude after you.");
                Program.Pause();
            }
        }

        public static void StoryBeat()
        {
            Player p = Program.player;
            if (p.hasWon) return;

            if (p.kills >= 3 && p.storyStage < 1)
            {
                p.storyStage = 1;
                Console.Clear();
                Program.PrintColor("The ground trembles beneath your feet.", ConsoleColor.Yellow);
                Program.Print("Far away, something ROARS. Birds scatter from the trees.");
                Program.Print("You have a bad feeling about this...");
                Program.Pause();
            }
            else if (p.kills >= 6 && p.storyStage < 2)
            {
                p.storyStage = 2;
                Console.Clear();
                Program.Print("You find a footprint in the mud. It is three-toed, and deeper than your cave entrance.");
                Program.Print("A wooden club could never have left the bruises you woke up with...");
                Program.Print("Whatever did this is still out there.");
                Program.Pause();
            }
            else if (p.kills >= BossUnlockKills && p.storyStage < 3)
            {
                p.storyStage = 3;
                Console.Clear();
                Program.Print("You follow the tracks up into the rocky hills and find a huge lair.");
                Program.Print("Bones litter the ground. The air smells of old blood.");
                Program.PrintColor("A new location is now available: the Lair of the T-Rex.", ConsoleColor.Magenta);
                Program.Print("Better buy some armour and a stronger weapon before you go in.");
                Program.Pause();
            }
        }

        public static void RandomFight()
        {
            Console.Clear();
            Enemy enemy = Enemy.CreateRandom(Program.player);
            Program.Print("You walk around a tree and are faced with a BEAST...");
            Program.PrintColor("It's a " + enemy.name + "!", ConsoleColor.Red);
            Program.Pause();

            if (Combat(enemy) == CombatResult.Won)
                Victory(enemy);
        }

        public static void RandomEncounter()
        {
            int roll = rand.Next(0, 10);
            if (roll < 7)
                RandomFight();
            else if (roll < 9)
                FindCache();
            else
                FindBottle();
        }

        static void FindCache()
        {
            Console.Clear();
            int coins = Program.player.GetCoins() / 2;
            Program.Print("Half-buried under some leaves, you spot a stash left by some other caveman.");
            Program.PrintColor("You find " + coins + " coins.", ConsoleColor.Yellow);
            Program.player.coins += coins;
            Program.Pause();
        }

        static void FindBottle()
        {
            Console.Clear();
            Program.Print("You stumble upon a strange bubbling puddle. You scoop some into a bottle.");
            Program.PrintColor("You gain 1 heal.", ConsoleColor.Green);
            Program.player.heals += 1;
            Program.Pause();
        }

        public static void Rest()
        {
            Player p = Program.player;
            Console.Clear();

            if (p.health >= p.maxHealth)
            {
                Program.Print("You're already in fighting shape.");
                Program.Pause();
                return;
            }

            Program.Print("You curl up by a small fire and close your eyes...");

            if (rand.Next(0, 100) < 25)
            {
                p.health = Math.Min(p.maxHealth, p.health + p.maxHealth / 3);
                Program.PrintColor("Something rustles in the dark! Your rest is cut short!", ConsoleColor.Red);
                Program.Pause();

                Enemy enemy = Enemy.CreateRandom(p);
                Program.PrintColor("A " + enemy.name + " attacks!", ConsoleColor.Red);
                Program.Pause();
                if (Combat(enemy) == CombatResult.Won)
                    Victory(enemy);
                return;
            }

            p.health = p.maxHealth;
            Program.PrintColor("You wake up fully rested. Health: " + p.health + "/" + p.maxHealth, ConsoleColor.Green);
            Program.Pause();
        }

        static string Bar(int value, int max)
        {
            if (max <= 0) max = 1;
            int filled = (int)Math.Ceiling(10.0 * Math.Max(0, value) / max);
            if (filled > 10) filled = 10;
            return "[" + new string('#', filled) + new string('-', 10 - filled) + "]";
        }

        static void DrawCombat(Enemy enemy, bool windingUp)
        {
            Player player = Program.player;
            Console.Clear();
            Program.PrintColor(enemy.name.ToUpper(), enemy.isBoss ? ConsoleColor.Magenta : ConsoleColor.Red, 10);
            Program.Print("ATK " + enemy.power + "   HP " + Bar(enemy.health, enemy.maxHealth) + " " + enemy.health + "/" + enemy.maxHealth, 10);
            if (windingUp)
                Program.PrintColor(">> The " + enemy.name + " is winding up a huge attack! Defend! <<", ConsoleColor.Yellow, 10);
            Program.Print("---------------------------", 10);
            Program.Print("|  (A)ttack     (D)efend  |", 10);
            Program.Print("|  (R)un        (H)eal    |", 10);
            Program.Print("---------------------------", 10);
            Program.Print("  You  HP " + Bar(player.health, player.maxHealth) + " " + player.health + "/" + player.maxHealth + "   Heals: " + player.heals + "   Lv " + player.level, 10);
        }

        static int RollPlayerDamage(Player player, out string note)
        {
            note = "";
            int roll = rand.Next(100);
            if (roll < 10)
            {
                note = "You miss!";
                return 0;
            }
            int dmg = rand.Next(1, player.damage + 1);
            if (roll >= 90)
            {
                note = "CRITICAL HIT!";
                dmg *= 2;
            }
            return dmg;
        }

        public static CombatResult Combat(Enemy enemy)
        {
            Player player = Program.player;
            int turn = 0;
            bool windingUp = false;
            bool enraged = false;

            while (enemy.health > 0)
            {
                DrawCombat(enemy, windingUp);
                string input = (Console.ReadLine() ?? "").Trim().ToLower();

                bool defending = false;
                bool enemyTurn = true;
                int dealt = 0;

                if (input == "a")
                {
                    // Attack
                    Program.Print("You rush forwards with your rock and attack.");
                    dealt = RollPlayerDamage(player, out string note);
                    if (note != "")
                        Program.PrintColor(note, ConsoleColor.Yellow);
                }
                else if (input == "d")
                {
                    defending = true;
                    Program.Print("As the " + enemy.name + " prepares to attack, you take a defensive stance with your rock.");
                    dealt = RollPlayerDamage(player, out string note) / 2;
                    if (rand.Next(100) < 30)
                    {
                        int counter = rand.Next(1, player.damage + 1);
                        Program.PrintColor("COUNTER-STRIKE! You hit back for " + counter + " extra damage.", ConsoleColor.Yellow);
                        dealt += counter;
                    }
                }
                else if (input == "r")
                {
                    if (enemy.isBoss)
                    {
                        Program.PrintColor("The " + enemy.name + " roars so loudly your legs freeze. There is no escaping this fight!", ConsoleColor.Red);
                        Program.Pause();
                        continue;
                    }

                    if (rand.Next(0, 3) == 0)
                    {
                        Program.Print("You try to run away but trip and fall.");
                        player.health -= 1;
                        Program.PrintColor("You take 1 damage.", ConsoleColor.Red);
                        enemyTurn = false;
                    }
                    else
                    {
                        Program.Print("You awaken your inner Usain Bolt and successfully run away.");
                        Console.Write("Coawadice increased.");
                        Program.Pause();
                        return CombatResult.Fled;
                    }
                }
                else if (input == "h")
                {
                    if (player.heals == 0)
                    {
                        Program.Print("You reach into your pocket but to your horror, you do not have heals.");
                    }
                    else
                    {
                        int healValue = 3 + player.level;
                        int before = player.health;
                        player.health = Math.Min(player.maxHealth, player.health + healValue);
                        player.heals -= 1;
                        Program.Print("You reach into your pocket and drink a bottle of unknown substance.");
                        Program.PrintColor("You gain " + (player.health - before) + " health.", ConsoleColor.Green);
                        enemyTurn = false;
                    }
                }
                else
                {
                    continue;
                }

                if (input == "a" || input == "d")
                {
                    enemy.health -= dealt;
                    Program.PrintColor("You deal " + dealt + " damage.", ConsoleColor.Green);
                }

                if (enemy.health <= 0)
                {
                    Program.Pause();
                    break;
                }

                if (enemy.isBoss && !enraged && enemy.health <= enemy.maxHealth / 2)
                {
                    enraged = true;
                    Program.PrintColor("The " + enemy.name + "'s eyes blaze with fury! It is ENRAGED!", ConsoleColor.Magenta);
                }

                // Enemy's turn
                if (enemyTurn)
                {
                    turn++;
                    bool wasWindup = windingUp;
                    windingUp = false;
                    bool dodged = false;
                    int enemyPower = enemy.power + (enraged ? 2 : 0);
                    int incoming;

                    if (wasWindup)
                    {
                        incoming = defending ? enemyPower / 2 : enemyPower * 2;
                        Program.PrintColor(defending
                            ? "You brace behind your rock as the TAIL SWIPE crashes into you!"
                            : "The " + enemy.name + "'s TAIL SWIPE slams into you!", ConsoleColor.Red);
                    }
                    else if (!defending && rand.Next(100) < 10)
                    {
                        Program.Print("You nimbly dodge the " + enemy.name + "'s strike.");
                        incoming = 0;
                        dodged = true;
                    }
                    else
                    {
                        incoming = defending ? enemyPower / 3 : enemyPower;
                        if (!defending)
                            Program.Print("The " + enemy.name + " strikes you back.");
                    }

                    if (!dodged)
                    {
                        incoming -= player.armour;
                        int minDamage = enemy.isBoss ? 1 : 0;
                        if (incoming < minDamage)
                            incoming = minDamage;
                        Program.PrintColor("You lose " + incoming + " health.", ConsoleColor.Red);
                        player.health -= incoming;
                    }

                    // The boss telegraphs a big attack every third turn
                    if (enemy.isBoss && !wasWindup && turn % 3 == 0)
                    {
                        windingUp = true;
                        Program.PrintColor("The " + enemy.name + " ROARS and rears back its tail!", ConsoleColor.Yellow);
                    }
                }

                if (player.health <= 0)
                {
                    Program.PrintColor("As the " + enemy.name + " stands above you and deals the final blow. You have been slain by the MIGHTY " + enemy.name + ".", ConsoleColor.Red, 100);
                    player.isDead = true;
                    Program.Save();
                    Program.Pause();
                    Environment.Exit(0);
                }

                Program.Pause();
            }

            return CombatResult.Won;
        }

        public static void Victory(Enemy enemy)
        {
            Player p = Program.player;
            int coins = p.GetCoins();

            if (enemy.name == "Caveman")
                Program.Print("The caveman slumps against a tree, groaning. You start to wonder if you got the right guy...");
            else
                Program.Print("As you stand victorious over the " + enemy.name + ", its body dissolves into an unknown substance.", 80);

            if (enemy.name != "Caveman" && rand.Next(0, 10) == 1)
            {
                Program.Print("You put the unknown substance in a bottle.", 100);
                p.heals += 1;
                Program.PrintColor("You gain 1 heal.", ConsoleColor.Green, 100);
            }

            p.kills++;
            p.coins += coins;
            Program.PrintColor("You gain " + coins + " coins. You now have " + p.coins + " coins.", ConsoleColor.Yellow);
            Program.PrintColor("You gain " + enemy.xpReward + " XP.", ConsoleColor.Cyan);

            int levels = p.AddXp(enemy.xpReward);
            if (levels > 0)
                Program.PrintColor("LEVEL UP! You are now level " + p.level + ". Max health is now " + p.maxHealth + ".", ConsoleColor.Green);

            Program.Pause();
        }

        public static void BossFight()
        {
            Player p = Program.player;
            Console.Clear();

            Program.Print("You climb the last slope to the mouth of the lair.");
            Program.Print("The ground shakes with every heavy breath from inside.");

            if (p.damage < 3 || p.armour < 1 || p.heals < 2)
                Program.PrintColor("Something tells you that you're not ready. (Try a better weapon, some armour and at least 2 heals.)", ConsoleColor.Yellow);

            Program.Print("Enter the lair? (Y/N)");
            string answer = (Console.ReadLine() ?? "").Trim().ToLower();
            if (!answer.StartsWith("y"))
            {
                Program.Print("You quietly back away. The T-Rex can wait.");
                Program.Pause();
                return;
            }

            Console.Clear();
            Program.Print("You step into the darkness...");
            Program.Print("Two yellow eyes open.");
            Program.PrintColor("It is the T-REX.", ConsoleColor.Magenta, 120);
            Program.Print("It roars, and the whole cave shakes the way your own cave shook the day you were bruised.");
            Program.Pause();

            Enemy rex = new Enemy("T-Rex", TRexPower + p.mods, TRexHealth + 5 * p.mods, 0, true);
            if (Combat(rex) == CombatResult.Won)
                Ending();
        }

        static void Ending()
        {
            Player p = Program.player;
            Console.Clear();

            Program.PrintColor("The T-Rex staggers, lets out one last rumbling groan, and crashes to the ground.", ConsoleColor.Magenta, 60);
            Program.Pause();
            Console.Clear();

            Program.Print("As the dust settles, you finally understand.", 60);
            Program.Print("The tremors that shook your cave, the fall that bruised you... that was the T-Rex.", 60);
            Program.Print("The caveman with the club had only been trying to keep it away from the valley.", 60);
            Program.Print("And you hit him with a rock for it.", 60);
            Program.Pause();
            Console.Clear();

            Program.Print("You drag a giant tooth back to the valley as proof.", 60);
            Program.Print("The caveman looks at it, looks at you, and quietly hands you his club.", 60);
            Program.Print("\"Sorry about the rock,\" you grunt. He grunts back. That is enough.", 60);
            Program.Print("For the first time in a long while, the valley is safe.", 60);
            Program.Pause();
            Console.Clear();

            Program.PrintColor("=========== THE END ===========", ConsoleColor.Yellow, 40);
            Program.Print("Hero:    " + (p.name == "" ? "Nameless caveman" : p.name), 20);
            Program.Print("Level:   " + p.level, 20);
            Program.Print("Victories: " + p.kills, 20);
            Program.Print("Coins:   " + p.coins, 20);
            Program.Print("================================", 20);

            p.hasWon = true;
            p.storyStage = 4;
            Program.Save();

            Console.WriteLine();
            Program.Print("Keep exploring the wilds? (Y/N)");
            string answer = (Console.ReadLine() ?? "").Trim().ToLower();
            if (!answer.StartsWith("y"))
                Program.Quit();
        }
    }
}