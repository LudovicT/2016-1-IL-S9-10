using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Algo.Tests
{
    [TestFixture]
    public class Reco
    {
        static string _badDataPath = @"..\..\..\ThirdParty\MovieData\MovieLens\";
        static string _goodDataPath = @"..\..\..\ThirdParty\MovieData\";

        [Test]
        public void CorrectData()
        {
            Dictionary<int, Movie> firstMovies;
            Dictionary<int, List<Movie>> duplicateMovies;
            Movie.ReadMovies(Path.Combine(_badDataPath, "movies.dat"), out firstMovies, out duplicateMovies);
            int idMovieMin = firstMovies.Keys.Min();
            int idMovieMax = firstMovies.Keys.Max();
            Console.WriteLine("{3} Movies from {0} to {1}, {2} duplicates.", idMovieMin, idMovieMax, duplicateMovies.Count, firstMovies.Count);

            Dictionary<int, User> firstUsers;
            Dictionary<int, List<User>> duplicateUsers;
            User.ReadUsers(Path.Combine(_badDataPath, "users.dat"), out firstUsers, out duplicateUsers);
            int idUserMin = firstUsers.Keys.Min();
            int idUserMax = firstUsers.Keys.Max();
            Console.WriteLine("{3} Users from {0} to {1}, {2} duplicates.", idUserMin, idUserMax, duplicateUsers.Count, firstUsers.Count);

            Dictionary<int, string> badLines;
            int nbRating = User.ReadRatings(Path.Combine(_badDataPath, "ratings.dat"), firstUsers, firstMovies, out badLines);
            Console.WriteLine("{0} Ratings: {1} bad lines.", nbRating, badLines.Count);

            Directory.CreateDirectory(_goodDataPath);
            // Saves Movies
            using (TextWriter w = File.CreateText(Path.Combine(_goodDataPath, "movies.dat")))
            {
                int idMovie = 0;
                foreach (Movie m in firstMovies.Values)
                {
                    m.MovieID = ++idMovie;
                    w.WriteLine("{0}::{1}::{2}", m.MovieID, m.Title, String.Join("|", m.Categories));
                }
            }

            // Saves Users
            string[] occupations = new string[]{
                "other",
                "academic/educator",
                "artist",
                "clerical/admin",
                "college/grad student",
                "customer service",
                "doctor/health care",
                "executive/managerial",
                "farmer",
                "homemaker",
                "K-12 student",
                "lawyer",
                "programmer",
                "retired",
                "sales/marketing",
                "scientist",
                "self-employed",
                "technician/engineer",
                "tradesman/craftsman",
                "unemployed",
                "writer" };
            using (TextWriter w = File.CreateText(Path.Combine(_goodDataPath, "users.dat")))
            {
                int idUser = 0;
                foreach (User u in firstUsers.Values)
                {
                    u.UserID = ++idUser;
                    string occupation;
                    int idOccupation;
                    if (int.TryParse(u.Occupation, out idOccupation)
                        && idOccupation >= 0
                        && idOccupation < occupations.Length)
                    {
                        occupation = occupations[idOccupation];
                    }
                    else occupation = occupations[0];
                    w.WriteLine("{0}::{1}::{2}::{3}::{4}", u.UserID, u.Male ? 'M' : 'F', u.Age, occupation, "US-" + u.ZipCode);
                }
            }
            // Saves Rating
            using (TextWriter w = File.CreateText(Path.Combine(_goodDataPath, "ratings.dat")))
            {
                foreach (User u in firstUsers.Values)
                {
                    foreach (var r in u.Ratings)
                    {
                        w.WriteLine("{0}::{1}::{2}", u.UserID, r.Key.MovieID, r.Value);
                    }
                }
            }
        }

        [Test]
        public void ReadMovieData()
        {
            RecoContext c = new RecoContext();
            c.LoadFrom(_goodDataPath);
            for (int i = 0; i < c.Users.Length; ++i)
                Assert.That(c.Users[i].UserID, Is.EqualTo(i + 1));
            for (int i = 0; i < c.Movies.Length; ++i)
                Assert.That(c.Movies[i].MovieID, Is.EqualTo(i + 1));
        }

        [Test]
        public void DistanceBetweenUserTest()
        {
            RecoContext c = new RecoContext();
            c.LoadFrom(_goodDataPath);
            double d = c.Distance(c.Users[3], c.Users[4836]);
            var users = c.Users.ToList();

            var us1 = Partitioner.Create(users);
            var us2 = Partitioner.Create(users);

            List<string> values = new List<string>();
            Parallel.ForEach(us1, x => Parallel.ForEach(us2, y =>
            {
                double dist = c.Distance(x, y);
                values.Add($"{x.UserID} & {y.UserID} : {dist}");
            }));
        }

        [Test]
        public void Test_best_keeper()
        {
            BestKeeper<int> truc = new BestKeeper<int>(5, Comparer<int>.Default);
            truc.AddCandidate(50);
            truc.AddCandidate(40);
            truc.AddCandidate(30);
            truc.AddCandidate(20);
            truc.AddCandidate(20);
            truc.AddCandidate(20);
            truc.AddCandidate(20);
            truc.AddCandidate(20);
            truc.AddCandidate(10);
            truc.AddCandidate(0);
            truc.AddCandidate(45);
            truc.AddCandidate(25);

            foreach ( var i in truc )
            {
                Console.WriteLine(i);
            }
        }
    }
}
