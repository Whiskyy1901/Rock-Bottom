using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace RPG
{
    public class Program
    {
        public const string GameTitle = "ROCK BOTTOM";

        public static Player player = new Player();
        public static bool mainLoop = true;

        static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            IncludeFields = true
        };

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = GameTitle;

            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");

            TitleScreen();

            Player? loaded = PlayerLoad();
            if (loaded != null)
            {
                player = loaded;
                if (player.health > player.maxHealth)
                    player.health = player.maxHealth;
            }
            else
            {
                player.id = NextPlayerId();
                Start();
                Encounters.FirstEncounter();
                Save();
            }

            MainMenu();
        }

        static void TitleScreen()
        {
            Console.Clear();
            string line = new string('=', GameTitle.Length + 12);
            Console.WriteLine();
            PrintColor("   " + line, ConsoleColor.Yellow, 5);
            PrintColor("   ===  " + GameTitle + "  ===", ConsoleColor.Yellow, 40);
            PrintColor("   " + line, ConsoleColor.Yellow, 5);
            Console.WriteLine();
            PrintColor("        a caveman adventure", ConsoleColor.DarkYellow, 30);
            Console.WriteLine();
            Console.WriteLine();
            Pause();
        }

        static void MainMenu()
        {
            while (mainLoop)
            {
                Console.Clear();
                bool lairOpen = player.storyStage >= 3 && !player.hasWon;

                PrintColor("=== THE WILDS ===", ConsoleColor.Cyan, 5);
                Print(player.name + "   Lv " + player.level + "   HP " + player.health + "/" + player.maxHealth
                    + "   Coins " + player.coins + "   XP " + player.xp + "/" + player.XpToNext(), 5);
                Console.WriteLine();
                Print("(E)xplore    (R)est    (C)amp shop", 5);
                if (lairOpen)
                    PrintColor("(L)air of the T-Rex", ConsoleColor.Magenta, 5);
                Print("(T)ext speed: " + SpeedLabel() + "    (Q)uit", 5);

                string input = (Console.ReadLine() ?? "").Trim().ToLower();
                switch (input)
                {
                    case "e":
                        Encounters.RandomEncounter();
                        Encounters.StoryBeat();
                        Save();
                        break;
                    case "r":
                        Encounters.Rest();
                        Save();
                        break;
                    case "c":
                        Shop.LoadShop(player);
                        Save();
                        break;
                    case "l":
                        if (lairOpen)
                        {
                            Encounters.BossFight();
                            Save();
                        }
                        break;
                    case "t":
                        player.textSpeed = (player.textSpeed + 1) % 3;
                        break;
                    case "q":
                        Quit();
                        break;
                }
            }
        }

        static string SpeedLabel()
        {
            return player.textSpeed switch
            {
                1 => "Fast",
                2 => "Instant",
                _ => "Normal"
            };
        }

        static int NextPlayerId()
        {
            int max = 0;
            foreach (string path in Directory.GetFiles("Saves", "*.json"))
            {
                string name = Path.GetFileNameWithoutExtension(path);
                if (int.TryParse(name, out int id) && id > max)
                    max = id;
            }
            return max + 1;
        }

        static void Start()
        {
            Console.Clear();
            Print("What is your name?");
            player.name = Console.ReadLine() ?? "";
            Console.Clear();
            Print("You wakeup in your cave, bruised.");
            if (player.name == "")
                Print("You don't even remember you own name....");
            else
                Print("You remember only your name; " + player.name);
            Pause();
            Console.Clear();
            Print("You see the sunlight, lighting your cave.");
            Print("You go out to explore.");
        }

        public static void Save()
        {
            string path = Path.Combine("Saves", player.id + ".json");
            File.WriteAllText(path, JsonSerializer.Serialize(player, options));
        }

        public static Player? PlayerLoad()
        {
            List<Player> players = new List<Player>();
            foreach (string path in Directory.GetFiles("Saves", "*.json"))
            {
                Player? temp = JsonSerializer.Deserialize<Player>(File.ReadAllText(path), options);
                if (temp != null)
                    players.Add(temp);
            }

            if (players.Count == 0)
                return null;

            while (true)
            {
                Console.Clear();
                Print("Select your player", 10);
                foreach (Player p in players)
                {
                    string status = p.isDead ? " (dead)" : p.hasWon ? " (victorious)" : "";
                    PrintColor(p.id + ": " + p.name + "  Lv " + p.level + status,
                        p.isDead ? ConsoleColor.DarkGray : p.hasWon ? ConsoleColor.Yellow : ConsoleColor.White, 10);
                }
                Console.WriteLine();
                Print("Enter player id, or type \"new\" to start a new game", 10);

                string? data = Console.ReadLine()?.Trim();
                if (string.Equals(data, "new", StringComparison.OrdinalIgnoreCase))
                    return null;

                foreach (Player p in players)
                {
                    if (p.id.ToString() == data)
                    {
                        if (p.isDead)
                        {
                            Console.WriteLine();
                            Print(p.name + " could not make it to the end. Their story is over.");
                            Pause();
                            break;
                        }
                        return p;
                    }
                }
            }
        }

        public static void Quit()
        {
            Save();
            Environment.Exit(0);
        }

        public static void Print(string text, int speed = 40)
        {
            int delay = player.textSpeed switch
            {
                1 => speed / 4,
                2 => 0,
                _ => speed
            };

            foreach (char c in text)
            {
                Console.Write(c);
                if (delay > 0)
                {
                    if (SkipRequested())
                        delay = 0;
                    else
                        System.Threading.Thread.Sleep(delay);
                }
            }
            Console.WriteLine();
        }

        public static void PrintColor(string text, ConsoleColor color, int speed = 40)
        {
            Console.ForegroundColor = color;
            Print(text, speed);
            Console.ResetColor();
        }

        static bool SkipRequested()
        {
            try
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
               
            }
            return false;
        }

        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[press any key]");
            Console.ResetColor();
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}