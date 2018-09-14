using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public MeshRenderer mr;
    private MeshFilter mf;
    private Mesh mesh;

    public int xr, yr, zr;
    public float power;
    private Map map;

    public string testStr;
    public bool isTest;

    public bool showMesh;
    public bool showArea;
    public bool showVoxState;
    public bool showMap;
    public bool showTriangles;
    public bool showEyeLine;

    public int targetX;
    public int targetZ;

    private void Start()
    {
        mf = mr.GetComponent<MeshFilter>();
        mesh = mf.mesh;

        map = new Map();
        map.OnMapChanged += this.OnMapChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapGenScene();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            map.AddPoint(targetX, targetZ);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            map.SubPoint(targetX, targetZ);
        }

        mr.enabled = showMesh;
    }

    private void OnMapChanged()
    {
        mesh.Clear();
        mesh.SetVertices(map.allTrianglePoints);
        mesh.SetTriangles(map.allTriangleIndexs, 0);
        mesh.SetUVs(0, map.allUvs);

        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
    }

    private void MapGenScene()
    {
        if (isTest)
        {
            map.TestByStr(testStr);
        }
        else
        {
            map.Init(xr, yr, zr, power);
            map.GenScene();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one);

        if (map != null && map.maps != null)
        {
            if (showArea)
            {
                Gizmos.color = Color.black;
                DrawArea();
            }
            if (showVoxState)
            {
                Gizmos.color = Color.red;
                DrawVoxStateOne();
            }
            if (showMap)
            {
                Gizmos.color = Color.green;
                DrawMap();
            }
            if (showTriangles)
            {
                Gizmos.color = Color.red;
                DrawTriangles();
            }
        }

        if (showEyeLine)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10);
        }
    }

    void DrawArea()
    {
        for (int y = 0; y <= this.yr; y++)
        {
            for (int x = 0; x <= map.maps.GetLength(0); x++)
            {
                for (int z = 0; z <= map.maps.GetLength(1); z++)
                {
                    Gizmos.DrawLine(new Vector3(0, y, z), new Vector3(map.maps.GetLength(0), y, z));
                    Gizmos.DrawLine(new Vector3(x, 0, z), new Vector3(x, yr, z));
                    Gizmos.DrawLine(new Vector3(x, y, 0), new Vector3(x, y, map.maps.GetLength(1)));
                }
            }
        }
    }

    void DrawVoxStateOne()
    {
        for (int y = 0; y < this.yr; y++)
        {
            for (int x = 0; x < map.maps.GetLength(0); x++)
            {
                for (int z = 0; z < map.maps.GetLength(1); z++)
                {
                    if (map.GetVoxState(x, y, z) == 1)
                    {
                        Gizmos.DrawSphere(new Vector3(x, y, z), 0.2f);
                    }
                }
            }
        }
    }

    void DrawMap()
    {
        for (int i = 0; i < map.maps.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < map.maps.GetLength(1) - 1; j++)
            {
                Gizmos.DrawLine(new Vector3(i, map.maps[i, j], j), new Vector3(i + 1, map.maps[i + 1, j], j));
                Gizmos.DrawLine(new Vector3(i, map.maps[i, j], j), new Vector3(i, map.maps[i, j + 1], j + 1));
            }
        }
    }

    void DrawTriangles()
    {
        for (int i = 0; i < map.allTrianglePoints.Count - 3; i += 3)
        {
            Gizmos.DrawLine(map.allTrianglePoints[i], map.allTrianglePoints[i + 1]);
            Gizmos.DrawLine(map.allTrianglePoints[i + 1], map.allTrianglePoints[i + 2]);
            Gizmos.DrawLine(map.allTrianglePoints[i + 2], map.allTrianglePoints[i]);
        }
    }
}
