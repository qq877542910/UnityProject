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

        //RenderMgr.Instance.DrawLine(new Vector3(0, height * 0.5f), new Vector3(width, height * 0.5f), Color.red);
        //RenderMgr.Instance.DrawLine(new Vector3(0, 0), new Vector3(width, height), Color.red);

        Vector3 v0 = new Vector3(0, 0, 0);
        Vector3 v1 = new Vector3(width, 0, 0);
        Vector3 v2 = new Vector3(width * 0.5f, height, 0);
        RenderMgr.Instance.DrawLine(v0, v2, Color.red);
       // RenderMgr.Instance.DrawTrangle(v0, v1, v2, Color.red, Color.red, Color.red);
        RenderMgr.Instance.Render();
    }

    private void Update()
    {
    }
}
