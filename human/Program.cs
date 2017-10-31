using System;

namespace human
{
    public class Human{
        public string name;
        public int strength = 3;
        public int intelligence = 3;
        public int dexterity = 3;
        public int health = 100;

        public Human(string givenName = ""){
            name = givenName;
        }
        public Human(string givenName, int givenStrength, int givenIntelligence, int givenDexterity, int givenHealth){
            name = givenName;
            strength = givenStrength;
            intelligence = givenIntelligence;
            dexterity = givenDexterity;
            health = givenHealth;
        }

        public void Attack(object opponent){
            if(opponent is Human){
                Human theOpponent = opponent as Human;
                theOpponent.health -= strength*5;
            }

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Human Bob = new Human("Bob");
            Human Dave = new Human("Dave");

            Console.WriteLine("Human Bob: " + Bob.health);
            Console.WriteLine("Human Dave: " + Dave.health);
            Bob.Attack(Dave);
                        Console.WriteLine("Human Bob: " + Bob.health);
            Console.WriteLine("Human Dave: " + Dave.health);
                        Bob.Attack(Dave);
                        Console.WriteLine("Human Bob: " + Bob.health);
            Console.WriteLine("Human Dave: " + Dave.health);


        }
    }
}
