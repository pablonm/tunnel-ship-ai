using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Data.Text;

public class Training : MonoBehaviour
{

    public GameObject spaceShipPrefab;
    public int generations;
    public int populationSize;
    public int batches;
    public int startRandomizingTunnelAtGeneration;

    Vector<double>[] chromosomes = null;

    private double[] scores;
    private int finishedShips;
    private int finishedBatchShips;
    private List<SpaceShip> batchShips;
    private int currentGeneration = 0;
    private int currentBatch = 0;
    private int shipsPerBatch;

    private Chart chart;
    private TunnelPathGenerator tunnelGenerator;

    private Text generationValue;
    private Text batchValue;

    void Start()
    {
        chart = FindObjectOfType<Chart>();
        tunnelGenerator = FindObjectOfType<TunnelPathGenerator>();
        generationValue = GameObject.Find("GenerationValue").GetComponent<Text>();
        batchValue = GameObject.Find("BatchValue").GetComponent<Text>();
        chromosomes = new Vector<double>[populationSize];
        shipsPerBatch = populationSize / batches;
        StartGeneration();
    }

    private void StartGeneration()
    {
        generationValue.text = (currentGeneration + 1).ToString() + "/" + generations.ToString();
        finishedShips = 0;
        currentBatch = 0;
        scores = new double[populationSize];
        StartBatch();
    }

    private void StartBatch()
    {
        batchValue.text = (currentBatch + 1).ToString() + "/" + batches.ToString();
        finishedBatchShips = 0;
        batchShips = new List<SpaceShip>();
        for (int i = 0; i < shipsPerBatch; i++)
        {
            int index = currentBatch * shipsPerBatch + i;
            GameObject obj = Instantiate(spaceShipPrefab, Vector3.zero, Quaternion.identity);
            SpaceShip ship = obj.GetComponent<SpaceShip>();
            batchShips.Add(ship);
            ship.InitializeBrain(chromosomes[index]);
            ship.StartEngine(s => OnShipFinished(s, index));
        }
    }

    private void NewGeneration()
    {
        if (currentGeneration >= startRandomizingTunnelAtGeneration)
            tunnelGenerator.Regenerate();
        Vector<double>[] newChromosomes = new Vector<double>[populationSize];
        newChromosomes = Evolution.Breed(Evolution.SelectFittest(chromosomes, scores));
        chromosomes = newChromosomes;
        StartGeneration();
    }

    private void EndBatch()
    {
        for (int i = 0; i < shipsPerBatch; i++)
            chromosomes[currentBatch * shipsPerBatch + i] = batchShips[i].GetChromosome();
        foreach (SpaceShip ship in batchShips)
            Destroy(ship.gameObject);
    }

    private void OnShipFinished(float score, int index)
    {
        finishedShips++;
        finishedBatchShips++;
        scores[index] = score;
        
        if (finishedBatchShips >= shipsPerBatch)
        {
            EndBatch();
            currentBatch++;
            if (currentBatch >= batches)
            {
                currentGeneration++;
                chart.AddGenerationData(currentGeneration, GetMaxScore(), GetAvgScore());
                if (currentGeneration < generations)
                {
                    NewGeneration();
                }
                else
                {
                    DelimitedWriter.Write(Application.dataPath + "/trained.csv", Evolution.GetFittestChromosome(chromosomes, scores).ToColumnMatrix(), ",", null, null, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    SceneManager.LoadScene("Playing");
                }
            }
            else
            {
                StartBatch();
            }
        }
    }

    private double GetMaxScore()
    {
        double max = 0;
        foreach (double score in scores)
            if (score > max)
                max = score;
        return max;
    }

    private double GetAvgScore()
    {
        double sum = 0;
        foreach (double score in scores)
            sum += score;
        return sum / scores.Length;
    }
}

