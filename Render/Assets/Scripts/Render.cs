using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
    public Material mat;
    public int width, height;

    private void Start()
    {
        RenderMgr.Instance.Init(width, height);
        mat.mainTexture = RenderMgr.Instance.RenderTexture;

        for (int i = 0; i < 3; i++)
        {
            int x0 = Random.Range(0, width);
            int y0 = Random.Range(0, height);
            int x1 = Random.Range(0, width);
            int y1 = Random.Range(0, height);
            Debug.LogFormat("[{0},{1}] -- [{2},{3}]", x0, y0, x1, y1);
            RenderMgr.Instance.DrawLine(x0, y0, x1, y1, Color.red);
        }
        RenderMgr.Instance.Render();
    }

    private void Update()
    {
    }
}
