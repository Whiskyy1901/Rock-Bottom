using System;
using System.Collections.Generic;
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
            Console.WriteLine("Game Name");
            Console.WriteLine("What is your name?");
            player.name = Console.ReadLine() ?? "";
            Console.Clear();
            Console.WriteLine("You wakeup in your cave, bruised.");
            if (player.name == "")
                Console.WriteLine("You don't even remember you own name....");
            else
                Console.WriteLine("You remember only your name; " + player.name);
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You see the sunlight, lighting your cave.");
            Console.WriteLine("You go out to explore.");
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
                Console.WriteLine("Select your player");
                foreach (Player p in players)
                    Console.WriteLine(p.id + ": " + p.name);
                Console.WriteLine();
                Console.WriteLine("Enter player id, or type \"new\" to start a new game");

                string? data = Console.ReadLine()?.Trim();
                if (string.Equals(data, "new", StringComparison.OrdinalIgnoreCase))
                    return null;

                foreach (Player p in players)
                {
                    if (p.id.ToString() == data)
                        return p;
                }
            }
        }
    }
}