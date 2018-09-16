using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGame : MonoBehaviour
{
    public float power;
    public int seed;

    public int width;
    public int height;
    private void Start()
    {
        WorldMgr.Instance.InitWorld(this.power, this.seed);

        for (int x = -width; x <= width; x++)
        {
            for (int z = -height; z <= height; z++)
            {
                WorldMgr.Instance.GenChunk(x, 0, z);
            }
        }

        WorldRenderMgr.instance.UpdateRenderAllChunk();
    }
}
