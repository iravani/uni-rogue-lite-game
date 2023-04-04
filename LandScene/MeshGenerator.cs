using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh _mesh;
    Vector2[] uvs;

    public int xSize = 200;
    public int zSize = 200;
    public float maxHeight = 20;
    public float perlinScale = 1;
    public float mapScale = 1;

    Vector3[] vertices;
    int[] triangles;

    [Header("Stones")]
    public GameObject[] stones;
    [Header("trees")]
    public GameObject[] palmTrees;
    public GameObject[] normalVegitations;

    MeshCollider meshColider;

    private void Start()
    {
        meshColider = gameObject.AddComponent<MeshCollider>();
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreateShape();
        UpdateMesh();
        meshColider.sharedMesh = _mesh;
    }

    //creates a mesh base on vertecies position
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float[,] map = Noise.GenerateNoiseMap(xSize + 1, zSize + 1, perlinScale);
        float[,] falloffMap = FalloffGenerator.GenerateFalloff(zSize + 1);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float temp_y = ((Mathf.Clamp(map[x, z] - falloffMap[x, z], 0, 1)) - 0.1f);
                float _y = temp_y * maxHeight;
                vertices[i] = new Vector3(x, _y, z);
                vertices[i] *= mapScale;
                vertices[i] = vertices[i] + gameObject.transform.position;
                int chance = Random.Range(0, 10000);
                if (chance < 40)//stones
                {
                    int r = Random.Range(0, stones.Length);
                    GameObject stone = Instantiate(stones[r]);
                    stone.transform.position = new Vector3(0, 0.5f, 0);
                    stone.transform.position += vertices[i];
                    stone.transform.position = stone.transform.position + gameObject.transform.position;
                    stone.transform.rotation = Quaternion.Euler(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
                    if (chance > 22)
                        stone.transform.localScale *= (1.0f * Random.Range(0.5f, 1.5f));
                    else if (chance < 1)
                        stone.transform.localScale *= (1.0f * Random.Range(20.0f, 40.0f));
                    else
                    {
                        stone.transform.localScale *= (1.0f * Random.Range(3.0f, 6.0f));
                    }
                    stone.GetComponent<Rigidbody>().mass *= 200 * stone.transform.localScale.x;
                }
                else if (chance < 300) //trees
                {
                    if (temp_y >= 0.01f)//upper then water
                    {
                        if (temp_y < 0.1f)//sand
                        {
                            int r = Random.Range(0, palmTrees.Length);
                            GameObject tree = Instantiate(palmTrees[r]);
                            tree.transform.position = vertices[i] + gameObject.transform.position;
                            tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 90), 0);
                            tree.transform.localScale *= (1.0f * Random.Range(0.5f, 1.5f));
                        }
                        else
                        {
                            int r = Random.Range(0, normalVegitations.Length);
                            GameObject tree = Instantiate(normalVegitations[r]);
                            tree.transform.position = vertices[i] + gameObject.transform.position;
                            tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 90), 0);
                            tree.transform.localScale *= (1.0f * Random.Range(0.9f, 3f));
                        }
                    }
                }
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tries = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tries + 0] = vert + 0;
                triangles[tries + 1] = vert + xSize + 1;
                triangles[tries + 2] = vert + 1;
                triangles[tries + 3] = vert + 1;
                triangles[tries + 4] = vert + xSize + 1;
                triangles[tries + 5] = vert + xSize + 2;

                vert++;
                tries += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }
    }

    //generates mesh base on the date defined in "createShape" function
    void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.uv = uvs;

        _mesh.RecalculateNormals();
    }
}
