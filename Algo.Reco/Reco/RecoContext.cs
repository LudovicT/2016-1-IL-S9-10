using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;

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
            _cacheDistance = new ConcurrentDictionary<TwoUserKey, double>();
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

        ConcurrentDictionary<TwoUserKey, double> _cacheDistance;

        public double GetSimilarity(User u1, User u2)
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

            double distance = 0;
            if (commonMovies.Any())
            {
                var commonMoviesList = commonMovies.ToList();
                if (commonMoviesList.Count == 1)
                {
                    distance = 1 / (1 + Math.Sqrt(commonMoviesList.Sum(x => Math.Pow(x.R1 - x.R2, 2))));
                    _cacheDistance.TryAdd(key, distance);
                    return distance;
                }

                double r1 = 0;
                double r2 = 0;
                double r1Pow = 0;
                double r2Pow = 0;
                double r1r2 = 0;
                int n = commonMoviesList.Count;

                commonMoviesList.ForEach(x =>
                {
                    r1 += x.R1;
                    r2 += x.R2;
                    r1Pow += x.R1 * x.R1;
                    r2Pow += x.R2 * x.R2;
                    r1r2 += x.R1 * x.R2;
                });

                double r1Coeff = (r1Pow - (r1 * r1) / n);
                double r2Coeff = (r2Pow - (r2 * r2) / n);

                if (r1Coeff == 0 || r2Coeff == 0)
                {
                    if (r1Coeff == r2Coeff)
                    {
                        distance = 1;
                        _cacheDistance.TryAdd(key, distance);
                        return distance;
                    }
                    distance = 1 / (1 + Math.Sqrt(commonMoviesList.Sum(x => Math.Pow(x.R1 - x.R2, 2))));
                    _cacheDistance.TryAdd(key, distance);
                    return distance;
                }

                distance = (r1r2 - (r1 * r2) / n)
                    / Math.Sqrt(r1Coeff * r2Coeff);

                _cacheDistance.TryAdd(key, distance);
                return distance;
            }

            distance = -1.0;
            _cacheDistance.TryAdd(key, distance);
            return -1.0;

        }

        public IReadOnlyCollection<SimilarUser> GetSimilarUsers( User u, int count = 100 )
        {
            BestKeeper<SimilarUser> best = new BestKeeper<SimilarUser>(count, (u1, u2) => Math.Sign( u1.Similarity - u2.Similarity ) );

            foreach( var other in Users )
            {
                if (other != u) best.AddCandidate(new SimilarUser(other, GetSimilarity(u, other)));
            }
            return best;
        }

        public IEnumerable<Movie> GetRecommendationsFor( User u, int count = 100 )
        {
            if (u == null) throw new ArgumentNullException(nameof( u ));

            if (u.Ratings.Count == 0) return Enumerable.Empty<Movie>();

            var similarUsers = GetSimilarUsers(u, count);

            return similarUsers.SelectMany(
                x => x.User.Ratings
                    .Where(y => !u.Ratings.ContainsKey(y.Key))
                    .Select(
                        y => new
                        {
                            Movie = y.Key,
                            Rating = y.Value,
                            Similarity = x.Similarity
                        })
                ).ToLookup(x => x.Movie).Select(x =>
                    {
                        return new
                        {
                            Movie = x.Key,
                            Weight = x.Sum(y => y.Rating * y.Similarity)
                        };
                    }).OrderByDescending( x => x.Weight ).Select( x => x.Movie );
        }
    }
}
