using System.Collections.Generic;
using UnityEngine;

public class TunnelMeshGenerator : MonoBehaviour
{
    public bool inverse;
    public float radius = 1f;
    private int numOfVertices = 6;

    public void GenerateTunnel() {
        Transform pointsParent = GameObject.Find("TunnelPoints").transform;
        MeshFilter filter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        filter.mesh = mesh;

        List<Vector3> vertices = new List<Vector3>();
        foreach (Transform point in pointsParent) {
            vertices.AddRange(GetVerticesInPosition(point));
        }
        mesh.vertices = vertices.ToArray();

        List<int> triangles = new List<int>();
        for (int i = 0; i < vertices.Count - numOfVertices; i++)
        {
            triangles.AddRange(GetTrianglesForVertex(i));
        }
        mesh.triangles = triangles.ToArray();

        List<Vector3> normals = new List<Vector3>();
        foreach (Vector3 vertex in vertices)
        {
            normals.Add(-Vector3.forward);
        }
        mesh.normals = normals.ToArray();

        gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void DeleteMesh() {
        MeshFilter filter = GetComponent<MeshFilter>();
        DestroyImmediate(filter.mesh);
        DestroyImmediate(gameObject.GetComponent<MeshCollider>());
    }

    private List<Vector3> GetVerticesInPosition(Transform point) {
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < numOfVertices; i++)
        {
            float angle = i * Mathf.PI * 2 / numOfVertices;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 pos = point.localRotation * new Vector3(x, y, 0);
            vertices.Add(pos + point.position);
        }
        return vertices;
    }

    private List<int> GetTrianglesForVertex(int vertexIndex) {
        List<int> vertices = new List<int>();
        if (!inverse)
        {
            if ((vertexIndex + 1) % numOfVertices == 0)
            {
                vertices.Add(vertexIndex);
                vertices.Add(vertexIndex - numOfVertices + 1);
                vertices.Add(vertexIndex + numOfVertices);
                vertices.Add(vertexIndex - numOfVertices + 1);
                vertices.Add(vertexIndex - numOfVertices + 1 + numOfVertices);
                vertices.Add(vertexIndex + numOfVertices);
            }
            else
            {
                vertices.Add(vertexIndex);
                vertices.Add(vertexIndex + 1);
                vertices.Add(vertexIndex + numOfVertices);
                vertices.Add(vertexIndex + 1);
                vertices.Add(vertexIndex + 1 + numOfVertices);
                vertices.Add(vertexIndex + numOfVertices);
            }
        }
        else {
            if ((vertexIndex + 1) % numOfVertices == 0)
            {
                vertices.Add(vertexIndex + numOfVertices);
                vertices.Add(vertexIndex - numOfVertices + 1);
                vertices.Add(vertexIndex);
                vertices.Add(vertexIndex + numOfVertices);
                vertices.Add(vertexIndex - numOfVertices + 1 + numOfVertices);
                vertices.Add(vertexIndex - numOfVertices + 1);
            }
            else
            {
                vertices.Add(vertexIndex + numOfVertices);
                vertices.Add(vertexIndex + 1);
                vertices.Add(vertexIndex);
                vertices.Add(vertexIndex + numOfVertices);
                vertices.Add(vertexIndex + 1 + numOfVertices);
                vertices.Add(vertexIndex + 1);
            }
        }
        return vertices;
    }
}
