using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeGen : MonoBehaviour
{
    public bool[] faces = new bool[6]; //front, back, top, bottom, left, right

    //debug
    private void Start()
    {
    }

    public void Generate()
    {
        int facesCount = 0;
        for (int c = 0; c < faces.Length; c++)
        {
            if (faces[c])
            {
                facesCount++;
            }
        }

        Vector3[] vertices = new Vector3[facesCount * 4];
        int[] tri = new int[facesCount * 6];
        Vector3[] normals = new Vector3[facesCount * 4];
        Vector2[] uv = new Vector2[facesCount * 4];

        int i = 0;

        for (int c = 0; c < faces.Length; c++)
        {
            if (faces[c])
            {
                switch(c)
                {
                    case 0: //front
                        vertices[4 * i + 0] = new Vector3(-0.5f, -0.5f, -0.5f);
                        vertices[4 * i + 1] = new Vector3(0.5f, -0.5f, -0.5f);
                        vertices[4 * i + 2] = new Vector3(-0.5f, 0.5f, -0.5f);
                        vertices[4 * i + 3] = new Vector3(0.5f, 0.5f, -0.5f);
                        break;
                    case 1: //back
                        vertices[4 * i + 0] = new Vector3(-0.5f, -0.5f, 0.5f);
                        vertices[4 * i + 2] = new Vector3(0.5f, -0.5f, 0.5f);
                        vertices[4 * i + 1] = new Vector3(-0.5f, 0.5f, 0.5f);
                        vertices[4 * i + 3] = new Vector3(0.5f, 0.5f, 0.5f);
                        break;
                    case 2: //top
                        vertices[4 * i + 0] = new Vector3(-0.5f, 0.5f, -0.5f);
                        vertices[4 * i + 1] = new Vector3(0.5f, 0.5f, -0.5f);
                        vertices[4 * i + 2] = new Vector3(-0.5f, 0.5f, 0.5f);
                        vertices[4 * i + 3] = new Vector3(0.5f, 0.5f, 0.5f);
                        break;
                    case 3: //bottom
                        vertices[4 * i + 0] = new Vector3(-0.5f, -0.5f, -0.5f);
                        vertices[4 * i + 2] = new Vector3(0.5f, -0.5f, -0.5f);
                        vertices[4 * i + 1] = new Vector3(-0.5f, -0.5f, 0.5f);
                        vertices[4 * i + 3] = new Vector3(0.5f, -0.5f, 0.5f);
                        break;
                    case 4: //left
                        vertices[4 * i + 0] = new Vector3(-0.5f, -0.5f, -0.5f);
                        vertices[4 * i + 1] = new Vector3(-0.5f, 0.5f, -0.5f);
                        vertices[4 * i + 2] = new Vector3(-0.5f, -0.5f, 0.5f);
                        vertices[4 * i + 3] = new Vector3(-0.5f, 0.5f, 0.5f);
                        break;
                    case 5: //right
                        vertices[4 * i + 0] = new Vector3(0.5f, -0.5f, -0.5f);
                        vertices[4 * i + 2] = new Vector3(0.5f, 0.5f, -0.5f);
                        vertices[4 * i + 1] = new Vector3(0.5f, -0.5f, 0.5f);
                        vertices[4 * i + 3] = new Vector3(0.5f, 0.5f, 0.5f);
                        break;
                    default:
                        break;
                }

                tri[6 * i + 0] = 4 * i + 0;
                tri[6 * i + 1] = 4 * i + 2;
                tri[6 * i + 2] = 4 * i + 1;
                tri[6 * i + 3] = 4 * i + 2;
                tri[6 * i + 4] = 4 * i + 3;
                tri[6 * i + 5] = 4 * i + 1;

                normals[4 * i + 0] = -Vector3.forward;
                normals[4 * i + 1] = -Vector3.forward;
                normals[4 * i + 2] = -Vector3.forward;
                normals[4 * i + 3] = -Vector3.forward;

                uv[4 * i + 0] = new Vector2(0, 0);
                uv[4 * i + 0] = new Vector2(1, 0);
                uv[4 * i + 0] = new Vector2(0, 1);
                uv[4 * i + 0] = new Vector2(1, 1);

                i += 1;
            }
        }

        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = tri,
            normals = normals,
            uv = uv
        };
        mf.mesh = mesh;
    }
}
