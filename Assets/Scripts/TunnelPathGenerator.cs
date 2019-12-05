using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPathGenerator : MonoBehaviour
{
    public GameObject pointPrefab;
    public int maxPoints = 50;
    public float maxRotation = 20;
    public float distanceBetweenPoints = 0.3f;

    private TunnelMeshGenerator[] meshGenerators;
    private Transform pointsParent;
    List<GameObject> allPoints;

    private void Start()
    {
        pointsParent = GameObject.Find("TunnelPoints").transform;
        meshGenerators = FindObjectsOfType<TunnelMeshGenerator>();
        Generate();
    }

    private void Generate() {
        bool isValid = GenerateTunnelPath();
        while (!isValid)
            isValid = GenerateTunnelPath();
        foreach (TunnelMeshGenerator meshGenerator in meshGenerators)
            meshGenerator.GenerateTunnel();
    }

    public void Regenerate() {
        DestroyPoints();
        DestroyMesh();
        Generate();
    }

    private void DestroyPoints() {
        foreach (GameObject point in allPoints)
            DestroyImmediate(point);
    }

    private void DestroyMesh() {
        foreach (TunnelMeshGenerator meshGenerator in meshGenerators)
            meshGenerator.DeleteMesh();
    }

    private bool GenerateTunnelPath()
    {
        allPoints = new List<GameObject>();
        Transform previousPoint = Instantiate(
                pointPrefab,
                transform.position,
                transform.rotation,
                pointsParent
            ).transform;
        allPoints.Add(previousPoint.gameObject);
        for (int i = 0; i < maxPoints; i++) {
            previousPoint = Instantiate(
                pointPrefab,
                previousPoint.position + previousPoint.forward * distanceBetweenPoints,
                previousPoint.rotation * Quaternion.Euler(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), 0), 
                pointsParent
            ).transform;
            if (i == maxPoints - 1)
                previousPoint.tag = "WinMark";
            allPoints.Add(previousPoint.gameObject);
            if (HasLoop(previousPoint)) {
                DestroyPoints();
                return false;
            }
        }
        return true;
    }

    private bool HasLoop(Transform point)
    {
        Collider[] hitColliders = Physics.OverlapBox(point.position, new Vector3(1f, 1f, .05f), point.rotation);
        int TunnelPointsFound = 0;
        foreach (Collider hit in hitColliders) {
            if (hit.gameObject.CompareTag("TunnelPoint"))
                TunnelPointsFound++;
        }
        return TunnelPointsFound >= 2 ? true : false;
    }
}
