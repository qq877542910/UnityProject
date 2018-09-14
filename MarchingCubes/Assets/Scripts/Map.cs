using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public int xR;
    public int yR;
    public int zR;
    public float power;
    public float[,] maps;

    public List<Triangle> allTriangles = new List<Triangle>();

    public List<Vector3> allTrianglePoints = new List<Vector3>();

    public List<int> allTriangleIndexs = new List<int>();

    public List<Vector2> allUvs = new List<Vector2>();

    public System.Action OnMapChanged;

    public void Init( int XR , int YR , int ZR , float power = 1)
    {
        this.xR = XR;
        this.yR = YR;
        this.zR = ZR;
        this.power = power;

        allTriangles.Clear();
        allTriangleIndexs.Clear();
        allTrianglePoints.Clear();
        allUvs.Clear();

        maps = new float[xR, zR];

        for (int x = 0; x < xR; x++)
        {
            for (int z = 0; z < zR; z++)
            {
                maps[x, z] = (Mathf.PerlinNoise(x * power ,z * power ) * YR);
            }
        }
    }

    public void TestByStr(string str)
    {
        this.allTriangles.Clear();
        this.allTrianglePoints.Clear();
        this.allTriangleIndexs.Clear();
        this.allUvs.Clear();

        allTriangles = TriTableDefine.GetTriangles(str);

        for (int i = 0; i < allTriangles.Count; i++)
        {
            allTrianglePoints.AddRange(TriangleToWorldPoints(allTriangles[i]));

            allTriangleIndexs.Add(i * 3);
            allTriangleIndexs.Add(i * 3 + 1);
            allTriangleIndexs.Add(i * 3 + 2);

            allUvs.Add(new Vector2(0, 0));
            allUvs.Add(new Vector2(1, 0));
            allUvs.Add(new Vector2(1, 1));
        }
    }

    public void SubPoint(int x, int z)
    {
        if (isPointLegal(x, z) && maps[x, z] - 1 >= 0) maps[x, z]--;

        this.GenScene();
    }

    public void AddPoint(int x, int z)
    {
        if (isPointLegal(x, z) && maps[x, z]+1 < yR) maps[x, z]++;

        this.GenScene();
    }

    private bool isPointLegal(int x, int z)
    {
        return (x >= 0 && x < xR && z >= 0 && z < zR);
    }

    public void GenScene()
    {
        allTriangles.Clear();
        allTriangleIndexs.Clear();
        allTrianglePoints.Clear();
        allUvs.Clear();

        for (int y = 0; y < yR; y++)
        {
            for (int x = 0; x < xR; x++)
            {
                for (int z = 0; z < zR; z++)
                {
                    List<Triangle> cubeTriangles = GetCubeTriangles(x, y, z, new Vector3(x, y, z));

                    allTriangles.AddRange(cubeTriangles);
                }
            }
        }

        for (int i = 0; i < allTriangles.Count; i++)
        {
            allTrianglePoints.AddRange(TriangleToWorldPoints(allTriangles[i]));

            allTriangleIndexs.Add(i * 3);
            allTriangleIndexs.Add(i * 3 + 1);
            allTriangleIndexs.Add(i * 3 + 2);

            allUvs.Add(new Vector2(0, 0));
            allUvs.Add(new Vector2(0, 1));
            allUvs.Add(new Vector2(1, 1));
        }

        if (this.OnMapChanged != null)
        {
            OnMapChanged();
        }
    }

    private List<Triangle> GetCubeTriangles(int x, int y, int z , Vector3 cubePoint)
    {
        return TriTableDefine.GetTriangles(GetCubeEightVoxStates(x, y, z) , cubePoint);
    }

    public int GetVoxState(int x, int y, int z)
    {
        if (IsVoxEmpty(x, y, z)) return 0;
        return 1;
    }

    private bool IsTopVox(int x, int y, int z)
    {
        if (x < 0 || x >= xR || z < 0 || z >= zR)
        {
            return false;
        }

        if (isPointLegal(x, z))
        {
            //顶部的点
            if (maps[x, z] >= y && maps[x, z] - 1 < y)
            {
                return true;
            }
            else//内部的点,但是周围有空白区
            {

            }
        }
        return false;
    }

    private bool IsVoxEmpty(int x, int y, int z)
    {
        if (!isPointLegal(x, z) || maps[x, z] < y)
        {
            return true;
        }
        return false;
    }

    private string  GetCubeEightVoxStates(int x, int y, int z)
    {
        string str = "";

        for (int i = 0; i < TriTableDefine.Points.Count; i++)
        {
            int tx = (int)(x + TriTableDefine.Points[i].x);
            int ty = (int)(y + TriTableDefine.Points[i].y);
            int tz = (int)(z + TriTableDefine.Points[i].z);

            str = GetVoxState(tx, ty, tz) + str;
        }
        return str;
    }

    private List<Vector3> TriangleToWorldPoints(Triangle triangle)
    {
        List<Vector3> points = new List<Vector3>();

        points.Add(triangle.worldV1);
        points.Add(triangle.worldV2);
        points.Add(triangle.worldV3);

        return points;
    }
}
