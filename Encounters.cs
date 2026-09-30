using System;

namespace RPG
{
    public class Encounters
    {
        static Random rand = new Random();
        public static void FirstEncounter()
        {
            Program.Print("Outside the cave, you see a man with a wooden club.");
            Program.Print("You assume that must be the man that bruised you.");
            Program.Print("You pickup the nearest rock and charge towards him.");
            Console.ReadKey();
            Combat(false, "Caveman", 2, 2);
        }

        public static void RandomFight()
        {
            Console.Clear();
            Program.Print("You walk around a tree and are faced with a BEAST...");
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
                Program.Print(name, 10);
                Program.Print(power+"/"+health, 10);
                Program.Print("---------------------------", 10);
                Program.Print("|  (A)ttack     (D)efend  |", 10);
                Program.Print("|  (R)un        (H)eal    |", 10);
                Program.Print("---------------------------", 10);
                Program.Print("  Heals: "+Program.player.heals+"  Health: "+Program.player.health, 10);

                string input = Console.ReadLine();

                if(input.ToLower() == "a")
                {
                    //Attack
                    Program.Print("You rush forwards with your rock and attack. As you do, the "+name+" strikes you back.");
                    int damage = rand.Next(1, Program.player.damage);
                    int incomming = power - Program.player.armour;
                    if(incomming <=0)
                        incomming = 0;
                    Program.Print("You deal "+damage+" damage and lose "+incomming+" health.");
                    Program.player.health -= incomming;
                    health -= damage;
                } 
                else if(input.ToLower() == "d")
                {
                    //Defend
                    Program.Print("As the "+name+" prepares to attack, you take a defensive stance with your rock");
                    int damage = rand.Next(1, Program.player.damage)/2;
                    int incomming = (power/3) - Program.player.armour;
                    if(incomming <=0)
                        incomming = 0;
                    Program.Print("You deal "+damage+" damage and lose "+incomming+" health.");
                    Program.player.health -= incomming;
                    health -= damage;
                } 
                else if(input.ToLower() == "r")
                {
                    //Run
                    int isEscape = rand.Next(0,3);
                    if(isEscape == 0)
                    {
                        Program.Print("You try to run away but trip and fall.");
                        Program.player.health -= 1;
                        Program.Print("You take 1 damage.");
                    }
                    else
                    {
                        Program.Print("You awaken your inner Usain Bolt and successfully run away.");
                        Console.Write("Coawadice increased.");
                        Console.ReadKey();
                        Shop.LoadShop(Program.player);
                    }
                } 
                else if(input.ToLower() == "h")
                {
                    if(Program.player.heals == 0)
                    {
                        Program.Print("You reach into your pocket but to your horror, you do not have heals.");
                        int incomming = power - Program.player.armour;
                        if(incomming <=0)
                            incomming = 0;
                        Program.Print("You lose "+incomming+" health.");
                        Program.player.health -= incomming;
                    }
                    else
                    {
                        Program.Print("You reach into your pocket and drink a bottle of unknown sustance.");
                        int healValue = 2;
                        Program.Print("You gain "+healValue+" health.");
                        Program.player.health += healValue;
                        Program.player.heals -= 1;
                    }
                }
                if (Program.player.health <= 0)
                {
                    if (Program.player.health <= 0)
                    {
                        Program.Print("As the "+name+" stand above you and deals the final blow. You have been slayn by the MIGHTY "+name+".", 100);
                        Program.player.isDead = true;
                        Program.Save();
                        Console.ReadKey();
                        System.Environment.Exit(0);
                    }
                }
                Console.ReadKey();
            }
            int coins = Program.player.GetCoins();
            Program.Print("As you stand victorious over the "+name+", it's body dissolves into an unknown substance.", 80);
            if (rand.Next(0, 10) == 1)    
            {
                Program.Print("You put the unknown substance in a bottle., 100");
                Program.player.heals +=1;
                Program.Print("You gain 1 heal.", 100);
            }
            Program.Print("You gain "+coins+" coins.");
            Program.player.coins += coins;
            Program.Print("You now have "+Program.player.coins+" coins.");
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