using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class Evolution
{
    private const double mutationRate = 20;             // How often mutation happens
    private const double mutationStrength = 0.1;       // How much the mutationc hanges the value
    private const double selectionRate = 5;            // Percentage of individuals selected for breeding

    public static Vector<double>[] SelectFittest(Vector<double>[] chromosomes, double[] scores)
    {
        int selectionSize = Convert.ToInt32(chromosomes.Length * (selectionRate / 100));
        Vector<double> fittestK = DenseVector.Build.Dense(selectionSize, i => double.MinValue);
        Vector<double>[] fittest = new Vector<double>[selectionSize];
        for (int i = 0; i < chromosomes.Length; i++)
        {
            if (scores[i] > fittestK.Minimum())
            {
                int minIndex = fittestK.MinimumIndex();
                fittestK[minIndex] = scores[i];
                fittest[minIndex] = chromosomes[i];
            }
        }
        return fittest;
    }

    public static Vector<double> GetFittestChromosome(Vector<double>[] chromosomes, double[] scores)
    {
        Vector<double> fittest = null;
        double fittestScore = 0;
        for (int i = 0; i < chromosomes.Length; i++)
        {
            if (scores[i] > fittestScore)
            {
                fittestScore = scores[i];
                fittest = chromosomes[i];
            }
        }
        return fittest;
    }

    public static Vector<double>[] Breed(Vector<double>[] chromosomes)
    {
        Random randomizer = new System.Random();
        Func<Random, int> _Rand = r => r.Next(chromosomes.Length);
        int timesToBreed = (int)Math.Floor(100 / selectionRate) - 1;
        List<Vector<double>> newChromosomes = new List<Vector<double>>();
        for (int i = 0; i < chromosomes.Length; i++)
        {
            newChromosomes.Add(chromosomes[i]);
            for (int t = 0; t < timesToBreed; t++)
                newChromosomes.Add(_Mutate(_Crossover(chromosomes[i], chromosomes[_Rand(randomizer)])));
        }
        return newChromosomes.ToArray();
    }

    private static Vector<double> _Crossover(Vector<double> chromosome1, Vector<double> chromosome2)
    {
        Random randomizer = new System.Random();
        int crossPoint = randomizer.Next(chromosome1.Count);
        return DenseVector.Build.Dense(chromosome1.Count, i => i < crossPoint ? chromosome1[i] : chromosome2[i]);
    }

    private static Vector<double> _Mutate(Vector<double> chromosome)
    {
        Random randomizer = new System.Random();
        Func<Random, double, double, double> _RandRange = (r, min, max) => r.NextDouble() * (max - min) + min;
        return DenseVector.Build.Dense(chromosome.Count, i => _RandRange(randomizer, 0, 100) <= mutationRate ? chromosome[i] + _RandRange(randomizer , - mutationStrength, mutationStrength) : chromosome[i]);
    }
    
}
