using UnityEngine;
using ChartAndGraph;

public class Chart : MonoBehaviour
{
    private GraphChartBase graph;

    void Start()
    {
        graph = GetComponent<GraphChartBase>();
        if (graph != null)
        {
            graph.DataSource.StartBatch();
            graph.DataSource.ClearCategory("MaxScore");
            graph.DataSource.ClearCategory("AvgScore");
            graph.DataSource.AddPointToCategory("MaxScore", 0, 0);
            graph.DataSource.AddPointToCategory("AvgScore", 0, 0);
            graph.DataSource.EndBatch();
        }
    }

    public void AddGenerationData(int generation, double max, double avg)
    {
        graph.DataSource.StartBatch();
        graph.DataSource.AddPointToCategory("MaxScore", generation, max);
        graph.DataSource.AddPointToCategory("AvgScore", generation, avg);
        graph.DataSource.EndBatch();
    }
}
