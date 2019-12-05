using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDirection : MonoBehaviour
{
    public bool showRays;
    public float maxDistance = 2;
    private float distance;

    public float GetDistance() {
        return distance;
    }

    void Update()
    {
        RaycastHit hit;
        int layer_mask = LayerMask.GetMask("TunnelWalls");
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer_mask))
        {
            if (showRays) Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            distance = 1 - Mathf.Min(maxDistance, hit.distance) / maxDistance;
        }
        else
        {
            distance = 0;
            if (showRays) Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
