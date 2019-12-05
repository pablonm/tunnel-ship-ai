using UnityEngine;
using UnityEngine.SceneManagement;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Data.Text;

public class Playing : MonoBehaviour
{
    public GameObject spaceShipPrefab;
    
    private Vector<double> trainedModelValues;
    private GameObject ship;

    void Start()
    {
        trainedModelValues = DelimitedReader.Read<double>(Application.dataPath + "/trained.csv", false, ",", false, System.Globalization.CultureInfo.InvariantCulture.NumberFormat).Column(0);
        ship = Instantiate(spaceShipPrefab, Vector3.zero, Quaternion.identity);
        ship.GetComponent<SpaceShip>().InitializeBrain(trainedModelValues);
        ship.GetComponent<SpaceShip>().StartEngine(s => Restart());
    }

    private void Restart()
    {
        SceneManager.LoadScene("Playing");
    }
}
