using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int XSIZE = 16;
    public const int YSIZE = 16;
    public const int ZSIZE = 16;

    public float scale;
    public int x, y, z;
    public bool isLoaded;

    public Chunk(int x, int y, int z, float scale =1 )
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.scale = scale;
    }
}
