using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderMgr
{
    private static RenderMgr _instance;
    public static RenderMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RenderMgr();
            }
            return _instance;
        }
     }

    public Texture2D RenderTexture { get; private set; }

    public Color[] colorBuffs { get; private set; }
    public float[] depthBuffs { get; private set; }

    public int width { get; private set; }
    public int height { get; private set; }
    public int length { get; private set; }

    public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.length = width * height;

        colorBuffs = new Color[width * height];
        depthBuffs = new float[width * height];

        RenderTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
    }

    public void DrawPixel(int x, int y, float z, Color color)
    {
        int index = GetIndex(x, y);

        if (index < 0 || index >= length)
        {
            return;
        }

        if (z <= depthBuffs[index])
        {
            depthBuffs[index] = z;
            colorBuffs[index] = color;
        }
    }

    public void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        int minX, maxX, minY, maxY;
        minX = Mathf.CeilToInt(Mathf.Min(start.x, end.x));
        maxX = Mathf.CeilToInt(Mathf.Max(start.x, end.x));
        minY = Mathf.CeilToInt(Mathf.Min(start.y, end.y));
        maxY = Mathf.CeilToInt(Mathf.Max(start.y, end.y));

        if (Mathf.Abs(maxX - minX ) > Mathf.Abs(maxY - minY))
        {
            for (int x = minX; x <= maxX; x++)
            {
                float t = (x - minX) / (float)(maxX - minX);
                int y = Mathf.CeilToInt(Mathf.Lerp(minY, maxY, t));
                float z = Mathf.Lerp(start.z, end.z, t);
                DrawPixel(x, y, z, color);
            }
        }
        else
        {
            for (int y = minY; y <= maxY; y++)
            {
                float t = (y - minY) / (float)(maxY - minY);
                int x = Mathf.CeilToInt(Mathf.Lerp(minX, maxX, t));
                float z = Mathf.Lerp(start.z, end.z, t);
                DrawPixel(x, y, z, color);
            }
        }
    }

    public void DrawTrangle(Vector3 v1, Vector3 v2, Vector3 v3, Color v1Color, Color v2Color, Color v3Color)
    {
        int minX, maxX, minY, maxY;
        float minZ, maxZ;

        minX = Mathf.CeilToInt(Mathf.Min(v1.x, v2.x, v3.x));
        maxX = Mathf.CeilToInt(Mathf.Max(v1.x, v2.x, v3.x));
        minY = Mathf.CeilToInt(Mathf.Min(v1.y, v2.y, v3.y));
        maxY = Mathf.CeilToInt(Mathf.Max(v1.y, v2.y, v3.y));
        minZ = Mathf.Min(v1.z, v2.z, v3.z);
        maxZ = Mathf.Max(v1.z, v2.z, v3.z);

        DrawLine(v1, v2, v1Color);
        DrawLine(v2, v3, v2Color);
       // DrawLine(v1, v3, v3Color);

        //for (int y = minY; y <= maxY; y++)
        //{
        //    for (int x = minX; x <= maxX; x++)
        //    {
        //        Vector3 point = new Vector3(x, y, 0);
        //        float z = (y - minY) / (float)(maxY - minY);

        //        float ct = Vector3.Distance(point , v1) / Vector3.Distance(v1 , v2);
        //        Color color = Color.Lerp(v1Color, v2Color, ct);

        //        ct = Vector3.Distance(point, v2) / Vector3.Distance(v2, v3);
        //        color = Color.Lerp(color, v2Color, ct);

        //        if (PointinTriangle1(v1, v2, v3, point))
        //        {
        //            DrawPixel(x, y, z, color);
        //        }
        //    }
        //}
    }

    public void DrawTrangle(Vector3 v1, Vector3 v2, Vector3 v3, Color color)
    {
        DrawTrangle(v1, v2, v3, color, color, color);
    }

    bool SameSide(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 AP = P - A;

        Vector3 v1 =Vector3.Cross( AB,AC);
        Vector3 v2 = Vector3.Cross(AB,AP);

        // v1 and v2 should point to the same direction
        return Vector3.Dot(v1,v2) >= 0;
    }

    // Same side method
    // Determine whether point P in triangle ABC
    bool PointinTriangle1(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        return SameSide(A, B, C, P) &&
            SameSide(B, C, A, P) &&
            SameSide(C, A, B, P);
    }

    bool PointinTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        Vector3 v0 = C - A;
        Vector3 v1 = B - A;
        Vector3 v2 = P - A;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

        float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
        if (u < 0 || u > 1) // if u out of range, return directly
        {
            return false;
        }

        float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
        if (v < 0 || v > 1) // if v out of range, return directly
        {
            return false;
        }

        return u + v <= 1;
    }

    public void Clear(Color color , float depth = -1)
    {
        for (int i = 0; i < colorBuffs.Length; i++)
        {
            colorBuffs[i] = color;
            depthBuffs[i] = depth;
        }
    }

    public void Render()
    {
        this.RenderTexture.SetPixels(colorBuffs);
        this.RenderTexture.Apply();
    }

    private int GetIndex(int x, int y)
    {
        return y * width + x;
    }
}
