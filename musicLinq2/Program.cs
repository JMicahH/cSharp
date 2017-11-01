using System;
using System.Collections.Generic;
using System.Linq;
using JsonData;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Collections to work with
            List<Artist> Artists = JsonToFile<Artist>.ReadJson();
            List<Group> Groups = JsonToFile<Group>.ReadJson();

            //========================================================
            //Solve all of the prompts below using various LINQ queries
            //========================================================

            //There is only one artist in this collection from Mount Vernon, what is their name and age?

            IEnumerable<Artist> vernonArtists = from vernArtist in Artists
                                                where (vernArtist.Hometown == "Mount Vernon")
                                                select vernArtist;

            foreach (Artist person in vernonArtists)
            {
                System.Console.WriteLine("NAME: " + person.ArtistName);
                System.Console.WriteLine("AGE: " + person.Age);
            }


            //Who is the youngest artist in our collection of artists?

            var youngestArtists = from yArtist in Artists
                                  orderby yArtist.Age ascending
                                  select new { yArtist.RealName };

            var theYoungest = youngestArtists.First();


            //Display all artists with 'William' somewhere in their real name

            var willArtists = from wartist in Artists
                              where wartist.RealName.Contains("William") || wartist.ArtistName.Contains("William")
                              select new { wartist.ArtistName, wartist.RealName };

            foreach (var artist in willArtists)
            {
                System.Console.WriteLine(artist.ArtistName + " :: " + artist.RealName);
            }


            //Display the 3 oldest artist from Atlanta

            var oldArtists = (from oldartist in Artists
                              orderby oldartist.Age descending
                              select new { oldartist.ArtistName, oldartist.Age }).Take(3);

            foreach (var oldfart in oldArtists)
            {
                System.Console.WriteLine(oldfart.ArtistName + "  " + oldfart.Age);
            }

            //(Optional) Display the Group Name of all groups that have members that are not from New York City


                            // NOT WORKING
            // var nonNewYorkArtists = Artists.Join(Groups,
            //                         a => a.GroupId,
            //                         g => g.Id,
            //                         (a, g) => new { a.Group = g , return a}
            //                         )
            //                         .Where(a.Hometown != "New York City");


            var y = from a in Artists
                    join g in Groups on a.GroupId equals g.Id
                    where a.Hometown != "New York City"
                    select new
                    {
                        g.GroupName,
                        a.Hometown
                    };

            //(Optional) Display the artist names of all members of the group 'Wu-Tang Clan'

            var w = from a in Artists
            join g in Groups on a.GroupId equals g.Id
            where g.GroupName == "Wu-Tang Clan"
            select new {
                a.ArtistName
            };

        }
    }
}
