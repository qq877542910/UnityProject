using LibNoise.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMgr : Singleton<WorldMgr>
{
    public float power;
    public int seed;
    public float WORLDMAXSIZEY = 16;

    public System.Action<Chunk> onChunkChangedAction;

    public Dictionary<string, Chunk> allChunkDict = new Dictionary<string, Chunk>();

    private Perlin myPerlin = new Perlin();

    public void InitWorld(float power, int seed)
    {
        this.power = power;
        this.seed = seed;

        myPerlin.Seed = seed;
    }

    public void GenChunk(int x , int y , int z, float scale = 1)
    {
        string key = GetChunkKey(x, y, z);
        if (!allChunkDict.ContainsKey(key))
        {
            Chunk chunk = new Chunk(x, y, z, scale);
            allChunkDict.Add(key, chunk);

            if (onChunkChangedAction != null)
            {
                onChunkChangedAction(chunk);
            }
        }
    }

    public int GetPointState(float x , float y , float z)
    {
        float value = GetPerlinNoise(x * power, y * power, z * power) * WORLDMAXSIZEY;
        if (value >= WORLDMAXSIZEY || value <= 0) return 0;
        return value > y ? 1 : 0;
    }

    private float GetPerlinNoise(float x, float y, float z)
    {
        return (float)myPerlin.GetValue(x, y, z);
    }

    public void SetPointState(int x, int z)
    {

    }

    private string GetChunkKey(int x,int y, int z)
    {
        return string.Format("{0}_{1}_{2}", x, y, z);
    }
}
