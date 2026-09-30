using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace RPG
{
    public class Program
    {
        public static Player player = new Player();
        public static bool mainLoop = true;

        static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            IncludeFields = true
        };

        static void Main(string[] args)
        {
            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");

            Player? loaded = PlayerLoad();
            if (loaded != null)
            {
                player = loaded;                // continue the saved game
            }
            else
            {
                player.id = NextPlayerId();     // fresh id for the new game
                Start();
                Encounters.FirstEncounter();
                Save();
            }

            while (mainLoop)
            {
                Encounters.RandomEncounter();
                Save();
            }
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
            Program.Print("Game Name");
            Program.Print("What is your name?");
            player.name = Console.ReadLine() ?? "";
            Console.Clear();
            Program.Print("You wakeup in your cave, bruised.");
            if (player.name == "")
                Program.Print("You don't even remember you own name....");
            else
                Program.Print("You remember only your name; " + player.name);
            Console.ReadKey();
            Console.Clear();
            Program.Print("You see the sunlight, lighting your cave.");
            Program.Print("You go out to explore.");
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
                return null;                    // no saves, go straight to a new game

            while (true)
            {
                Console.Clear();
                Program.Print("Select your player");
                foreach (Player p in players)
                    Program.Print(p.id + ": " + p.name + (p.isDead ? " (dead)" : ""));               
                Console.WriteLine();
                Program.Print("Enter player id, or type \"new\" to start a new game");

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
                            Program.Print(p.name + " could not make it to the end. Their story is over.");
                            Console.ReadKey();
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
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }
    }
}