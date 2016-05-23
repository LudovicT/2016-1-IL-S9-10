using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Algo
{
    public class RecoContext
    {
        public User[] Users { get; private set; }
        public Movie[] Movies { get; private set; }

        public void LoadFrom(string folder)
        {
            Users = User.ReadUsers(Path.Combine(folder, "users.dat"));
            Movies = Movie.ReadMovies(Path.Combine(folder, "movies.dat"));
            User.ReadRatings(Users, Movies, Path.Combine(folder, "ratings.dat"));
        }

        public RecoContext()
        {
            _cacheDistance = new Dictionary<TwoUserKey, double>();
        }

        struct TwoUserKey : IEquatable<TwoUserKey>
        {
            public readonly User U1;
            public readonly User U2;

            public TwoUserKey(User u1, User u2)
            {
                U1 = u1;
                U2 = u2;
            }

            public bool Equals(TwoUserKey other)
            {
                return (U1 == other.U1 && U2 == other.U2)
                    || (U1 == other.U2 && U2 == other.U2);
            }

            public override bool Equals(object obj)
            {
                if (obj is TwoUserKey) return Equals((TwoUserKey)obj);
                return false;
            }

            public override int GetHashCode() => U1.GetHashCode() ^ U2.GetHashCode();
        }

        Dictionary<TwoUserKey, double> _cacheDistance;

        public double Distance(User u1, User u2)
        {
            if (u1 == u2) return 0;
            double value;
            var key = new TwoUserKey(u1, u2);
            if (_cacheDistance.TryGetValue(key, out value))
            {
                return value;
            }
            var commonMovies = u1.Ratings.Join(u2.Ratings,
                                                u1R => u1R.Key,
                                                u2R => u2R.Key,
                                                (u1R, u2R) => new
                                                {
                                                    Movie = u1R.Key,
                                                    R1 = u1R.Value,
                                                    R2 = u2R.Value
                                                });
            if (commonMovies.Any())
            {
                var commonMoviesList = commonMovies.ToList();
                if (commonMoviesList.Count == 1)
                {
                    double distance = 1 / (1 + Math.Sqrt(commonMoviesList.Sum(x => Math.Pow(x.R1 - x.R2, 2))));
                    _cacheDistance.Add(key, distance);
                    return distance;
                }

                double r1 = commonMoviesList.Sum(x => x.R1);
                double r2 = commonMoviesList.Sum(x => x.R2);
                double r1Pow = commonMoviesList.Sum(x => Math.Pow(x.R1, 2));
                double r2Pow = commonMoviesList.Sum(x => Math.Pow(x.R2, 2));
                double r1r2 = commonMoviesList.Sum(x => x.R1 * x.R2);
                int n = commonMoviesList.Count;

                return (r1r2 - (r1 * r2) / n)
                    / Math.Sqrt((r1Pow - Math.Pow(r1, 2) / n) * (r2Pow - Math.Pow(r2, 2) / n));
            }

            return -1.0;

        }
    }
}
