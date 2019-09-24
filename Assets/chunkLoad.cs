using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkLoad : MonoBehaviour
{
    public int initCount = 0;
    public GameObject chunk;
    public GameObject player;
    //public bool inProgress = false;
    GameObject[] instChunks = new GameObject[27];
    Vector3Int renderCentre;
    void Start()
    {
        Vector3 newPos = player.transform.position;
        renderCentre = new Vector3Int(Mathf.RoundToInt(newPos.x / 20), Mathf.RoundToInt(newPos.y / 20), Mathf.RoundToInt(newPos.z / 20));
        init();
    }

    void init()
    {
        for (int x = renderCentre.x - 1; x <= renderCentre.x + 1; x++)
        {
            for (int y = renderCentre.y - 1; y <= renderCentre.y + 1; y++)
            {
                for (int z = renderCentre.z - 1; z <= renderCentre.z + 1; z++)
                {
                    instChunks[(x + 1 - renderCentre.x) * 9 + (y + 1 - renderCentre.y) * 3 + (z + 1 - renderCentre.z)] = Instantiate(chunk, new Vector3(20 * x - 9, 20 * y - 9, 20 * z - 9), Quaternion.identity);
                }
            }
        }
        Debug.Log(renderCentre);

    }

    void updater()
    {
        Vector3 newPos = player.transform.position;
        Vector3Int newRenderCentre = new Vector3Int(Mathf.RoundToInt(newPos.x / 20), Mathf.RoundToInt(newPos.y / 20), Mathf.RoundToInt(newPos.z / 20));

        bool[] valid = new bool[27];
        bool[] accountedFor = new bool[27]; //x * 9 + y * 3 + z

        for (int i = 0; i < instChunks.Length; i++)
        {
            if (Mathf.Abs(newRenderCentre.x - Mathf.RoundToInt(instChunks[i].transform.position.x / 20)) <= 1 && Mathf.Abs(newRenderCentre.y - Mathf.RoundToInt(instChunks[i].transform.position.y / 20)) <= 1 && Mathf.Abs(newRenderCentre.z - Mathf.RoundToInt(instChunks[i].transform.position.z / 20)) <= 1)
            {
                valid[i] = true;
                int x = Mathf.RoundToInt(instChunks[i].transform.position.x / 20) - newRenderCentre.x + 1;
                int y = Mathf.RoundToInt(instChunks[i].transform.position.y / 20) - newRenderCentre.y + 1;
                int z = Mathf.RoundToInt(instChunks[i].transform.position.z / 20) - newRenderCentre.z + 1;
                accountedFor[x * 9 + y * 3 + z] = true;
            }
        }

        int count = 0;
        for (int i = -1 + newRenderCentre.x; i <= 1 + newRenderCentre.x; i++)
        {
            for (int j = -1 + newRenderCentre.y; j <= 1 + newRenderCentre.y; j++)
            {
                for (int k = -1 + newRenderCentre.z; k <= 1 + newRenderCentre.z; k++)
                {
                    if (!accountedFor[count])
                    {
                        accountedFor[count] = true;
                        for (int a = 0; a < instChunks.Length; a++)
                        {
                            if (!valid[a])
                            {
                                valid[a] = true;
                                instChunks[a].transform.position = new Vector3(i * 20 - 9, j * 20 - 9, k * 20 - 9);
                                instChunks[a].GetComponent<marching>().update = true;
                                break;
                            }
                        }
                    }
                    count++;
                }
            }
        }
    }

    
    void Update()
    {
        if (player.active)
        {
            updater();
        }
        else if (initCount >= 27)
        {
            player.SetActive(true);
            //inProgress = false;
        }
    }
}