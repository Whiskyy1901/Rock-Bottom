using System;

namespace RPG
{
    public class Encounters
    {
        static Random rand = new Random();
        public static void FirstEncounter()
        {
            Console.WriteLine("Outside the cave, you see a man with a wooden club.");
            Console.WriteLine("You assume that must be the man that bruised you.");
            Console.WriteLine("You pickup the nearest rock and charge towards him.");
            Console.ReadKey();
            Combat(false, "Caveman", 2, 2);
        }

        public static void RandomFight()
        {
            Console.Clear();
            Console.WriteLine("You walk around a tree and are faced with a BEAST...");
            Console.ReadKey();
            Combat(true,"",0,0);
        }

        public static void RandomEncounter()
        {
            switch (rand.Next(0,1))
            {
                case 0:
                    RandomFight();
                    break;
            }
        }

        public static void Combat(bool random, string creatureName, int Power, int Health)
        {
            int health;
            int power;
            string name;
            if (random)
            {
                name = GetName();
                power = Program.player.GetPower();
                health = Program.player.GetHealth();
            }
            else
            {    
                health = Health;
                power = Power;
                name = creatureName;
            }

            while (health > 0)
            {
                Console.Clear();
                Console.WriteLine(name);
                Console.WriteLine(power+"/"+health);
                Console.WriteLine("---------------------------");
                Console.WriteLine("|  (A)ttack     (D)efend  |");
                Console.WriteLine("|  (R)un        (H)eal    |");
                Console.WriteLine("---------------------------");
                Console.WriteLine("  Heals: "+Program.player.heals+"  Health: "+Program.player.health);

                string input = Console.ReadLine();

                if(input.ToLower() == "a")
                {
                    //Attack
                    Console.WriteLine("You rush forwards with your rock and attack. As you do, the "+name+" strikes you back.");
                    int damage = rand.Next(1, Program.player.damage);
                    int incomming = power - Program.player.armour;
                    if(incomming <=0)
                        incomming = 0;
                    Console.WriteLine("You deal "+damage+" damage and lose "+incomming+" health.");
                    Program.player.health -= incomming;
                    health -= damage;
                } 
                else if(input.ToLower() == "d")
                {
                    //Defend
                    Console.WriteLine("As the "+name+" prepares to attack, you take a defensive stance with your rock");
                    int damage = rand.Next(1, Program.player.damage)/2;
                    int incomming = (power/3) - Program.player.armour;
                    if(incomming <=0)
                        incomming = 0;
                    Console.WriteLine("You deal "+damage+" damage and lose "+incomming+" health.");
                    Program.player.health -= incomming;
                    health -= damage;
                } 
                else if(input.ToLower() == "r")
                {
                    //Run
                    int isEscape = rand.Next(0,3);
                    if(isEscape == 0)
                    {
                        Console.WriteLine("You try to run away but trip and fall.");
                        Program.player.health -= 1;
                        Console.WriteLine("You take 1 damage.");
                    }
                    else
                    {
                        Console.WriteLine("You awaken your inner Usain Bolt and successfully run away.");
                        Console.Write("Coawadice increased.");
                        Console.ReadKey();
                        Shop.LoadShop(Program.player);
                    }
                } 
                else if(input.ToLower() == "h")
                {
                    if(Program.player.heals == 0)
                    {
                        Console.WriteLine("You reach into your pocket but to your horror, you do not have heals.");
                        int incomming = power - Program.player.armour;
                        if(incomming <=0)
                            incomming = 0;
                        Console.WriteLine("You lose "+incomming+" health.");
                        Program.player.health -= incomming;
                    }
                    else
                    {
                        Console.WriteLine("You reach into your pocket and drink a bottle of unknown sustance.");
                        int healValue = 2;
                        Console.WriteLine("You gain "+healValue+" health.");
                        Program.player.health += healValue;
                        Program.player.heals -= 1;
                    }
                }
                if (Program.player.health <= 0)
                {
                    if (Program.player.health <= 0)
                    {
                        Console.WriteLine("As the "+name+" stand above you and deals the final blow. You have been slayn by the MIGHTY "+name+".");
                        Program.player.isDead = true;
                        Program.Save();
                        Console.ReadKey();
                        System.Environment.Exit(0);
                    }
                }
                Console.ReadKey();
            }
            int coins = Program.player.GetCoins();
            Console.WriteLine("As you stand victorious over your the "+name+", it's body dissolves into an unknown substance.");
            if (rand.Next(0, 10) == 1)    
            {
                Console.WriteLine("You put the unknown substance in a bottle.");
                Program.player.heals +=1;
                Console.WriteLine("You gain 1 heal.");
            }
            Console.WriteLine("You gain "+coins+" coins.");
            Program.player.coins += coins;
            Console.WriteLine("You now have "+Program.player.coins+" coins.");
            Console.ReadKey();
        }

        public static string GetName()
        {
            switch (rand.Next(0, 5))
            {
                case 0:
                    return "Snake";
                case 1:
                    return "Lizard";
                case 2:
                    return "Sloth";
                case 3:
                    return "Spider";
                case 4:
                    return "Mosquito";  
                default:
                    return "Fly";              
            }
        }
    }
}