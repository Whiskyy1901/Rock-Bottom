using System;

namespace RPG{
    class Program
    {  
        public static Player player = new Player();
        static void Main(string[] args)
        {
            Start();
        }

        static void Start()
        {
            Console.WriteLine("Game Name");
            Console.WriteLine("What is your name?");
            player.name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("You wakeup in your cave, bruised.");
            if(player.name == "")
                Console.WriteLine("You don't even remember you own name....");
            else
                Console.WriteLine("You remember only your name; " + player.name);
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You see the sunlight, lighting your cave.");
            Console.WriteLine("You go out to explore.");
        }
    }
}