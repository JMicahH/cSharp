using System;
using DbConnection;
using System.Collections.Generic;


namespace simpleCrud
{
    class Program
    {

        static void read()
        {
            // List<Dictionary<string, object>> allUsers = DbConnector.Query("SELECT * FROM mydb.Users");

            // foreach (List<Dictionary<string, object>> user in allUsers)
            // {
            //     System.Console.WriteLine("{0} - {1} - {2} - {3}", user["_id"], user["FirstName"], user["LastName"], user["FavoriteNumber"]);
            // }
        }

        static void create()
        {

        }


        static void Main(string[] args)
        {
            // string query = "insert into users (FirstName, LastName, FavoriteNumber) values ('John', 'Doe', 42)";

            string fname = "";
            string lname = "";
            string favNum = "";
            string Id = "";

            Console.WriteLine("Please enter the ID of the user you want to update: ");
            Id = Console.ReadLine();
            string selectQuery = $"select * from users where id = {Id}";
            List<Dictionary<string, object>> result = DbConnector.Query(selectQuery);

            if (result.Count > 0)
            {


                Console.WriteLine("Great, found that ID. What would you like to do? ");
                Console.WriteLine("1. Update the user");
                Console.WriteLine("2. Delete the user");
                string answer = Console.ReadLine();

                if (answer.Trim() == "1")
                {

                    Console.Write("What is the new First Name: ");
                    fname = Console.ReadLine();

                    Console.Write("Got it. What is the new Last Name: ");
                    lname = Console.ReadLine();

                    Console.Write("Got it. What is the new favorite number: ");
                    favNum = Console.ReadLine();

                    string updateQuery = $"update users set FirstName = '{fname}', LastName = '{lname}', FavoriteNumber = '{favNum}', where id ='{Id}'";

                    DbConnector.Execute(updateQuery);

                    System.Console.WriteLine("Success!");
                }

                else if (answer.Trim() == "2")
                {

                    string deleteQuery = $"delete from users where id = {Id}";

                    DbConnector.Execute(deleteQuery);

                    System.Console.WriteLine("Success!");
                }

                else{
                    System.Console.WriteLine("Invalid option.");
                }



            }

            else
            {
                System.Console.WriteLine("User does not exist.");
            }


            Console.Write("please enter your first name: ");
            fname = Console.ReadLine();

            Console.Write("please enter your last name: ");
            lname = Console.ReadLine();

            Console.Write("please enter your fav number: ");
            favNum = Console.ReadLine();

            string query = $"insert into users (FirstName, LastName, FavoriteNumber) values ('{fname}', '{lname}', '{favNum}')";

            System.Console.WriteLine(query); //for testing to see if query is wrong...can copy to SQLWorkbench


            DbConnector.Execute(query);
        }
    }
}
