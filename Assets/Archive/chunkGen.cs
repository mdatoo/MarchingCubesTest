using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkGen : MonoBehaviour
{

    public Transform cube;

    public int iCount;
    public int jCount;
    public int kCount;
    [Range(1, 10)]
    public int octaves;
    [Range(1.0f, 10f)]
    public float lacunarity;
    [Range(0.0f, 1.0f)]
    public float persistence;
    [Range(0.0f, 2.0f)]
    public float samplingLimit;
    [Range(0.0f, 1.0f)]
    public float scaling;

    Vector3 pos;
    int oldOctaves;
    float oldLacunarity;
    float oldPersistence;
    float oldSamplingLimit;
    float oldScaling;

    float Perlin3DNoise(float x, float y, float z, int octaves, float lacunarity, float persistence)
    {
        float val = 0;

        for (int i = 0; i < octaves; i++)
        {
            val += Perlin3D(x * Mathf.Pow(lacunarity, i) + 0.5f + transform.position.x, y * Mathf.Pow(lacunarity, i) + 0.5f + transform.position.y, z * Mathf.Pow(lacunarity, i) + 0.5f + transform.position.z) * Mathf.Pow(persistence, i);
        }

        return val;
    }

    float Perlin3D(float x, float y, float z)
    {
        float AB = Mathf.PerlinNoise(x * scaling, y * scaling);
        float BC = Mathf.PerlinNoise(y * scaling, z * scaling);
        float AC = Mathf.PerlinNoise(x * scaling, z * scaling);

        float BA = Mathf.PerlinNoise(y * scaling, x * scaling);
        float CB = Mathf.PerlinNoise(z * scaling, y * scaling);
        float CA = Mathf.PerlinNoise(z * scaling, x * scaling);

        return (AB + BC + AC + BA + CB + CA) / 6;
    }

    void Start()
    {
        pos = transform.position;
        oldOctaves = octaves;
        oldLacunarity = lacunarity;
        oldPersistence = persistence;
        oldSamplingLimit = samplingLimit;

        bool[,,] cubeExists = new bool[iCount, jCount, kCount];

        for (int i = 0; i < iCount; i++)
        {
            for (int j = 0; j < jCount; j++)
            {
                for (int k = 0; k < kCount; k++)
                {
                    /*if (Mathf.PerlinNoise(0.5f + i * 0.1f, 0.5f + k * 0.1f) >= (1.0f / jCount) * j) {
                        cubeExists[i, j, k] = true;
                    }*/
                    if (Perlin3DNoise(i, j, k, octaves, lacunarity, persistence) > samplingLimit)
                    {
                        cubeExists[i, j, k] = true;
                    }
                }
            }
        }

        for (int i = 0; i < iCount; i++)
        {
            for (int j = 0; j < jCount; j++)
            {
                for (int k = 0; k < kCount; k++)
                {
                    bool[] faces = new bool[6];
                    if (i == 0 || !cubeExists[i - 1, j, k])
                    {
                        faces[4] = true;
                    }
                    if (j == 0 || !cubeExists[i, j - 1, k])
                    {
                        faces[3] = true;
                    }
                    if (k == 0 || !cubeExists[i, j, k - 1])
                    {
                        faces[0] = true;
                    }
                    if (i == iCount - 1 || !cubeExists[i + 1, j, k])
                    {
                        faces[5] = true;
                    }
                    if (j == jCount - 1 || !cubeExists[i, j + 1, k])
                    {
                        faces[2] = true;
                    }
                    if (k == kCount - 1 || !cubeExists[i, j, k + 1])
                    {
                        faces[1] = true;
                    }

                    if (cubeExists[i, j, k])
                    {
                        Transform obj = Instantiate(cube, new Vector3(i, j, k), Quaternion.identity);
                        obj.GetComponent<cubeGen>().faces = faces;
                        obj.GetComponent<cubeGen>().Generate();
                        obj.SetParent(transform);
                    }
                }
            }
        }

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().mesh.Optimize();
        transform.gameObject.SetActive(true);
    }

    void Update()
    {
        if (transform.position != pos || octaves != oldOctaves || lacunarity != oldLacunarity || persistence != oldPersistence || samplingLimit != oldSamplingLimit || scaling != oldScaling)
        {
            pos = transform.position;
            oldOctaves = octaves;
            oldLacunarity = lacunarity;
            oldPersistence = persistence;
            oldSamplingLimit = samplingLimit;
            oldScaling = scaling;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            bool[,,] cubeExists = new bool[iCount, jCount, kCount];

            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < jCount; j++)
                {
                    for (int k = 0; k < kCount; k++)
                    {
                        /*if (Mathf.PerlinNoise(0.5f + i * 0.1f, 0.5f + k * 0.1f) >= (1.0f / jCount) * j) {
                            cubeExists[i, j, k] = true;
                        }*/
                        if (Perlin3DNoise(i, j, k, octaves, lacunarity, persistence) > samplingLimit)
                        {
                            cubeExists[i, j, k] = true;
                        }
                    }
                }
            }

            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < jCount; j++)
                {
                    for (int k = 0; k < kCount; k++)
                    {
                        bool[] faces = new bool[6];
                        if (i == 0 || !cubeExists[i - 1, j, k])
                        {
                            faces[4] = true;
                        }
                        if (j == 0 || !cubeExists[i, j - 1, k])
                        {
                            faces[3] = true;
                        }
                        if (k == 0 || !cubeExists[i, j, k - 1])
                        {
                            faces[0] = true;
                        }
                        if (i == iCount - 1 || !cubeExists[i + 1, j, k])
                        {
                            faces[5] = true;
                        }
                        if (j == jCount - 1 || !cubeExists[i, j + 1, k])
                        {
                            faces[2] = true;
                        }
                        if (k == kCount - 1 || !cubeExists[i, j, k + 1])
                        {
                            faces[1] = true;
                        }

                        if (cubeExists[i, j, k])
                        {
                            Transform obj = Instantiate(cube, new Vector3(i - iCount / 2.0f, j - jCount / 2.0f, k - kCount / 2.0f), Quaternion.identity);
                            obj.GetComponent<cubeGen>().faces = faces;
                            obj.GetComponent<cubeGen>().Generate();
                            obj.SetParent(transform);
                        }
                    }
                }
            }

            transform.GetComponent<MeshFilter>().mesh.Clear();

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.GetComponent<MeshFilter>().mesh.Optimize();
            transform.gameObject.SetActive(true);
        }
    }
}
