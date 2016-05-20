using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class GeneticAlgorithm
    {
        public double CrossoverRate { get; set; }
        public double MutationRate { get; set; }
        public int PopulationSize { get; set; }
        public double MaxGeneration { get; set; }
        public int MaxGenomeDepth { get; set; }
        public int MaxGenomeSize { get; set; }
        private readonly Random _random;
        private readonly SwapGenomeVisitor _swapper;
        private readonly List<Node> _currentGeneration;
        private readonly MutationVisitor _mutationVisitor;
        private readonly NodeCreator _creator;
        private readonly List<double> _fitnessList;
        private double _totalFitness;
        private VariableVisitor _variableSetVisitor;

        public Node BestNode { get; private set; }
        public Func<GeneticAlgorithm, Node, double> FitnessFunction { get; set; }
        public bool Elitism { get; set; }
        public bool ReverseComparison { get; set; }

        public GeneticAlgorithm(double crossoverRate, double mutationRate, int populationSize, double maxGeneration, int maxGenomeDepth, int maxGenomeSize, int seed, Func<GeneticAlgorithm, Node, double> fitnessFunction = null)
        {
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            PopulationSize = populationSize;
            MaxGeneration = maxGeneration;
            MaxGenomeDepth = maxGenomeDepth;
            MaxGenomeSize = maxGenomeSize;
            _fitnessList = new List<double>();
            _currentGeneration = new List<Node>();
            _swapper = new SwapGenomeVisitor();
            _random = new Random(seed);
            _creator = new NodeCreator(_random, 2);
            _variableSetVisitor = new VariableVisitor();
            _mutationVisitor = new MutationVisitor(_creator, mutationRate, maxGenomeDepth, maxGenomeSize, seed);
            FitnessFunction = fitnessFunction;
            Elitism = false;
            ReverseComparison = false;
        }

        public List<Node> Run(int maxBestResult = 10)
        {
            if (FitnessFunction == null) throw new ArgumentNullException(nameof(FitnessFunction));

            var bestByGeneration = new List<Node>();
            int bestResultCount = 0;

            CreateGenome();
            RankPopulation();
            Node firstBest = _currentGeneration.LastOrDefault();
            bestByGeneration.Add(firstBest);
            Debug.WriteLine("Gen 0");
            Debug.WriteLine($"Best node : {firstBest}");
            Debug.WriteLine($"Best node fitness : {firstBest?.Fitness}");
            if (firstBest?.Fitness == 0) bestResultCount++;

            for (int i = 0; i < MaxGeneration; i++)
            {
                if (bestResultCount == maxBestResult)
                {
                    return bestByGeneration;
                }

                CreateNextGeneration();
                RankPopulation();
                Node currentBest = _currentGeneration.LastOrDefault(x => bestResultCount == 0 || !bestByGeneration.Any(y => y?.Fitness == 0 && y.ToString() == x.ToString()));
                if (bestResultCount > 0 && currentBest?.Fitness != 0)
                {
                    currentBest = _currentGeneration.LastOrDefault();
                }
                bestByGeneration.Add(currentBest);
                Debug.WriteLine($"Gen {i + 1}");
                Debug.WriteLine($"Best node : {currentBest}");
                Debug.WriteLine($"Best node fitness : {currentBest?.Fitness}");

                if (currentBest?.Fitness == 0) bestResultCount++;
            }
            return bestByGeneration;
        }

        private void CreateGenome()
        {
            for (int i = 0; _currentGeneration.Count < PopulationSize; i++)
            {
                Node genome = _creator.RandomNode(MaxGenomeDepth, MaxGenomeSize);

                if (!NodeContainsVariable(genome, "A") ||
                    !NodeContainsVariable(genome, "B")) continue;

                _currentGeneration.Add(genome);
            }
        }

        private bool NodeContainsVariable(Node genome, string variableName)
        {
            _variableSetVisitor.SetVariable(genome, variableName);
            return _variableSetVisitor.VariableOccurence != 0;
        }

        private Node CrossOver(Node parent1, Node parent2)
        {
            int nbTry = 0;
            Node child;
            do
            {
                int first = _random.Next(parent1.Count);
                int second = _random.Next(parent2.Count);
                child = nbTry % 2 == 0
                    ? _swapper.SwapGenome(parent1, parent2, first, second)
                    : _swapper.SwapGenome(parent2, parent1, second, first);
                nbTry++;
                if (nbTry > 250)
                {
                    child = _creator.RandomNode(MaxGenomeDepth, MaxGenomeSize);
                }
                else if (nbTry > 50)
                {
                    Node fitNode = _creator.RandomNode(MaxGenomeDepth / 2, MaxGenomeSize / 2);
                    int third = _random.Next(fitNode.Count);
                    if (nbTry % 4 == 0) child = _swapper.SwapGenome(fitNode, parent2, third, second);
                    else if (nbTry % 4 == 1) child = _swapper.SwapGenome(fitNode, parent1, third, first);
                    else if (nbTry % 4 == 2) child = _swapper.SwapGenome(parent1, fitNode, first, third);
                    else if (nbTry % 4 == 3) child = _swapper.SwapGenome(parent2, fitNode, second, third);
                }
            } while (child.Count > MaxGenomeSize || child.Depth > MaxGenomeDepth || !NodeContainsVariable(child, "A") || !NodeContainsVariable(child, "B"));
            return child;
        }

        private int RouletteSelection()
        {
            double randomFitness = _random.NextDouble() * _totalFitness;
            int idx = -1;
            int first = 0;
            int last = PopulationSize - 1;
            int mid = (last - first) / 2;

            while (idx == -1 && first <= last)
            {
                if (ReverseComparison ? randomFitness > _fitnessList[mid] : randomFitness < _fitnessList[mid])
                {
                    last = mid;
                }
                else if (ReverseComparison ? randomFitness < _fitnessList[mid] : randomFitness > _fitnessList[mid])
                {
                    first = mid;
                }
                mid = (first + last) / 2;
                if (last - first == 1)
                {
                    idx = last;
                }
            }
            return idx;
        }

        private void RankPopulation()
        {
            _totalFitness = 0;
            _currentGeneration.ForEach(x => x.Fitness = FitnessFunction(this, x));
            _totalFitness = _currentGeneration.Where(x => !double.IsNaN(x.Fitness) && !double.IsInfinity(x.Fitness)).Sum(x => x.Fitness);
            _currentGeneration.Sort(new GenomeComparer(false));

            double fitness = 0.0;
            _fitnessList.Clear();

            foreach (var genome in _currentGeneration)
            {
                fitness += double.IsNaN(genome.Fitness) || double.IsInfinity(genome.Fitness) ? 0 : genome.Fitness;
                _fitnessList.Add(fitness);
            }
        }

        private void CreateNextGeneration()
        {
            List<Node> nextGeneration = new List<Node>();
            Node g = _currentGeneration.LastOrDefault();

            for (int i = 0; i < PopulationSize; i += 2)
            {
                int pidx1 = RouletteSelection();
                int pidx2 = RouletteSelection();
                var parent1 = _currentGeneration[pidx1];
                var parent2 = _currentGeneration[pidx2];
                Node child1, child2;

                if (_random.NextDouble() < CrossoverRate)
                {
                    child1 = CrossOver(parent1, parent2);
                    child2 = CrossOver(parent2, parent1);
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }

                int nbTry = 0;
                int maxTry = 10;
                Node child1Backup = child1.Clone();
                Node child2Backup = child2.Clone();
                do
                {
                    child1 = child1Backup.Clone();
                    _mutationVisitor.Mutate(ref child1);
                    nbTry++;
                } while (nbTry < maxTry && (!NodeContainsVariable(child1, "A") || !NodeContainsVariable(child1, "B")));
                if (nbTry == maxTry)
                {
                    child1 = child1Backup.Clone();
                }

                nbTry = 0;
                do
                {
                    child2 = child2Backup.Clone();
                    _mutationVisitor.Mutate(ref child2);
                    nbTry++;
                } while (nbTry < maxTry && (!NodeContainsVariable(child2, "A") || !NodeContainsVariable(child2, "B")));
                if (nbTry == maxTry)
                {
                    child2 = child2Backup.Clone();
                }

                nextGeneration.Add(child1);
                nextGeneration.Add(child2);
            }
            if (Elitism && g != null)
            {
                nextGeneration[0] = g;
            }

            _currentGeneration.Clear();
            _currentGeneration.AddRange(nextGeneration);
        }
    }
}