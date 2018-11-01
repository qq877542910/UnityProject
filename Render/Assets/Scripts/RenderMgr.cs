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

    public void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = x1 - x0;
        int dy = y1 - y0;
        int k;
        float xincrement, yincrement;
        float x = x0, y = y0;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
            k = Mathf.Abs(dx);
        else
            k = Mathf.Abs(dy);

        xincrement = (dx) / (float)(k);
        yincrement = (dy) / (float)(k);

        DrawPixel(Mathf.RoundToInt(x0), Mathf.RoundToInt(y0), 0, color);
        for (int i = 0; i < k; i++)
        {
            x += xincrement;
            y += yincrement;
            DrawPixel(Mathf.RoundToInt(x), Mathf.RoundToInt(y),0 , color);//只有坐标轴上增加斜率K大于0.5时才会在坐标轴上加1
        }
    }

    public void DrawLine(Vector2 v1, Vector2 v2, Color color)
    {
        DrawLine((int)v1.x, (int)v1.y, (int)v2.x, (int)v2.y, color);
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
