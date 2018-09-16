using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderMgr : MonoBehaviour
{
    public static WorldRenderMgr instance;

    public GameObject renderPrefab;

    private GameObject ChunkRoot;

    private Dictionary<string, Mesh> chunkMeshDict = new Dictionary<string, Mesh>();
    private Dictionary<Mesh, GameObject> meshObjectDict = new Dictionary<Mesh, GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            GameObject.DestroyImmediate(this);
            return;
        }
        instance = this;

        if (ChunkRoot != null)
        {
            GameObject.DestroyImmediate(ChunkRoot);
        }
        ChunkRoot = new GameObject("SceneRoot");
    }


    public void UpdateRenderAllChunk()
    {
        StartCoroutine(GenAllWorld());
    }

    IEnumerator GenAllWorld()
    {
        int i = 0;
        foreach (var val in WorldMgr.Instance.allChunkDict)
        {
            UpdateRenderOneChunk(val.Value);
            if (i++ > 0)
            {
                i = 0;
                yield return 0;
            }
        }
    }

    private void UpdateRenderOneChunk(Chunk chunk)
    {
        Mesh mesh = GetMesh(chunk);
        mesh.Clear();

        List<Vector3> vertexs;
        List<Vector2> uvs;
        List<int> triangles;

        GenChunk(chunk, out vertexs, out uvs, out triangles);
        meshObjectDict[mesh].transform.position = new Vector3(chunk.x * Chunk.XSIZE, chunk.y * Chunk.YSIZE, chunk.z * Chunk.ZSIZE);
        mesh.SetVertices(vertexs);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
    }

    private Mesh GetMesh(Chunk chunk)
    {
        string key = GetChunkKey(chunk);

        if (!chunkMeshDict.ContainsKey(key))
        {
            GameObject newObj = GameObject.Instantiate(renderPrefab, this.ChunkRoot.transform);
            newObj.name = key;
            Mesh mesh = newObj.GetComponent<MeshFilter>().mesh;
            chunkMeshDict.Add(key, mesh);
            meshObjectDict.Add(mesh, newObj);
        }
        return chunkMeshDict[key];
    }

    private string GetChunkKey(Chunk chunk)
    {
        return string.Format("{0}_{1}_{2}",chunk.x , chunk.y , chunk.z);
    }

    private void GenChunk(Chunk chunk, out List<Vector3> vertexs, out List<Vector2> uvs, out List<int> triangles)
    {
        vertexs = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();

        for (int y = 0; y < Chunk.YSIZE; y++)
        {
            for (int x = 0; x < Chunk.XSIZE; x++)
            {
                for (int z = 0; z < Chunk.ZSIZE; z++)
                {
                    float realX = (chunk.x * Chunk.XSIZE + x);
                    float realY = (chunk.y * Chunk.YSIZE + y);
                    float realZ = (chunk.z * Chunk.ZSIZE + z);

                    string state = GetCubeEightVoxStates(new Vector3(realX, realY ,realZ));
                    List<Vector3> ve = GetVertexsByTriTable(state ,new Vector3(x , y ,z ) );
                    vertexs.AddRange(ve);

                }
            }
        }

        for (int i = 0; i <= vertexs.Count - 3; i += 3)
        {
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            triangles.Add(i);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }
    }

    private string GetCubeEightVoxStates(Vector3 point)
    {
        string str = "";
        Vector3 pt;
        for (int i = 0; i < TriTableDefine.Points.Count; i++)
        {
            pt = point + TriTableDefine.Points[i];
            str = GetVoxState(pt.x, pt.y, pt.z) + str;
        }
        return str;
    }

    public int GetVoxState(float x, float y, float z)
    {
        int state = WorldMgr.Instance.GetPointState(x, y, z);
        return state> 0 ? 1 : 0;
    }

    private void OnChunkUpdateAction(Chunk chunk)
    {
        UpdateRenderOneChunk(chunk);
    }

    private List<Vector3> GetVertexsByTriTable(string state , Vector3 pos)
    {
        List<Vector3> ves = TriTableDefine.GetTriangles(state);

        for (int i = 0; i < ves.Count; i++)
        {
            ves[i] += pos;
        }
        return ves ;
    }
}
