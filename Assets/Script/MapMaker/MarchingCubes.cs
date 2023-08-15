using System;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    public static MarchingCubes Instance;
    public float threshold = 0.5f;
    public int oneSize;
    public Vector3Int kan;
    [Space()]
    public Material planet;
    private MeshFilter[,,] meshFilter;
    private MeshRenderer[,,] meshRenderer;
    private MeshCollider[,,] meshCollider;
    private GameObject[,,] child;
    float[,,,,,] densityField;
    private Mesh[,,] mesh;
    private Vector3[,,] center;
    float sum;
    Vector3 vec;
    int oneSizePlus;
    int playerMax = 1;
    int floorMax = 5;
    int min = 0;
    int max = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        max = kan.y;
        sum = kan.x * oneSize;
        vec = transform.position + new Vector3(sum * -0.5f, 0, sum * -0.5f);
        meshFilter = new MeshFilter[kan.x, 8000, kan.z];
        meshRenderer = new MeshRenderer[kan.x, 8000, kan.z];
        meshCollider = new MeshCollider[kan.x, 8000, kan.z];
        mesh = new Mesh[kan.x, 8000, kan.z];
        child = new GameObject[kan.x, 8000, kan.z];
        center = new Vector3[kan.x, 8000, kan.z];
        oneSizePlus = oneSize + 1;
        for (int x = 0; x < kan.x; x++)
        {
            for (int y = 0; y < kan.y+1; y++)
            {
                for (int z = 0; z < kan.z; z++)
                {
                    child[x, y, z] = new GameObject($"({x} {y} {z})");
                    
                    child[x, y, z].transform.position = new Vector3(x * oneSize, y * oneSize, z * oneSize);
                    child[x, y, z].transform.parent = transform;
                    meshFilter[x, y, z] = child[x, y, z].AddComponent<MeshFilter>();
                    meshRenderer[x, y, z] = child[x, y, z].AddComponent<MeshRenderer>();
                    meshRenderer[x, y, z].material = planet;
                    meshCollider[x, y, z] = child[x, y, z].AddComponent<MeshCollider>();
                    center[x, y, z] = new Vector3(x * oneSize + oneSize * 0.5f, y * oneSize + oneSize * 0.5f, z * oneSize + oneSize * 0.5f);
                }
            }
        }
        densityField = new float[kan.x, 8000, kan.z, oneSizePlus, oneSizePlus, oneSizePlus];
        transform.position = vec;
    }
    void Update()
    {
        if(playerMax < PlayerMove.playerTrm.position.y + 5)
        {
            playerMax += 5;
            MaxUp();
        }
        if(FloorUp.y > floorMax)
        {
            floorMax += 5;
            MinUp();
        }
    }
    void MaxUp()
    {
        max++;
        transform.position = Vector3.zero;
        for (int x = 0; x < kan.x; x++)
        {
            for (int z = 0; z < kan.z; z++)
            {
                child[x, max, z] = new GameObject($"({x} {max} {z})");
                child[x, max, z].transform.position = new Vector3(x * oneSize, max * oneSize, z * oneSize);
                child[x, max, z].transform.parent = transform;
                meshFilter[x, max, z] = child[x, max, z].AddComponent<MeshFilter>();
                meshRenderer[x, max, z] = child[x, max, z].AddComponent<MeshRenderer>();
                meshRenderer[x, max, z].material = planet;
                meshCollider[x, max, z] = child[x, max, z].AddComponent<MeshCollider>();
                center[x, max, z] = new Vector3(x * oneSize + oneSize * 0.5f, max * oneSize + oneSize * 0.5f, z * oneSize + oneSize * 0.5f);
            }
        }
        transform.position = vec;
    }
    void MinUp()
    {
        for (int x = 0; x < kan.x; x++)
        {
            for (int z = 0; z < kan.z; z++)
            {
                Destroy(child[x, min, z]);
            }
        }
        min++;
    }
    private Mesh GenerateMesh(float[,,] field)
    {
        Mesh mesh = MarchingCubesAlgorithm.GenerateMesh(field, threshold);
        if (mesh == null)
        {
            Debug.LogError("Failed to generate mesh!");
            return null;
        }
        return mesh;
    }
    public void MakeGround(Vector3 trm, float dis)
    {
        for (int x = 0; x < kan.x; x++)
        {
            for (int y = min; y < max; y++)
            {
                for (int z = 0; z < kan.z; z++)
                {
                    if (Vector3.SqrMagnitude(trm - (center[x, y, z]+vec)) < (dis+ oneSize) * (dis+ oneSize) * 4)
                    {
                        float[,,] field = new float[oneSizePlus, oneSizePlus, oneSizePlus];
                        bool isChange = false;
                        for (int x2 = 0; x2 < oneSizePlus; x2++)
                        {
                            for (int y2 = 0; y2 < oneSizePlus; y2++)
                            {
                                for (int z2 = 0; z2 < oneSizePlus; z2++)
                                {
                                    if (densityField[x, y, z, x2, y2, z2] == 0)
                                    {
                                        if (Vector3.SqrMagnitude(child[x, y, z].transform.position + new Vector3(x2, y2, z2) - trm) < dis * dis)
                                        {
                                            densityField[x, y, z, x2, y2, z2] = 1;
                                            field[x2, y2, z2] = 1;
                                            isChange = true;
                                        }
                                        else
                                        {
                                            field[x2, y2, z2] = 0;
                                        }
                                    }
                                    else
                                    {
                                        field[x2, y2, z2] = 1;
                                    }
                                }
                            }
                        }
                        if (isChange)
                        {
                            mesh[x, y, z] = GenerateMesh(field);
                            meshFilter[x, y, z].mesh = mesh[x, y, z];
                            meshCollider[x, y, z].sharedMesh = meshFilter[x, y, z].mesh;
                        }
                    }
                }
            }
        }
    }
    public void MakeHole(Vector3 trm, float dis)
    {
        for (int x = 0; x < kan.x; x++)
        {
            for (int y = min; y < max; y++)
            {
                for (int z = 0; z < kan.z; z++)
                {
                    if (Vector3.SqrMagnitude(trm - (center[x, y, z] + vec)) < (dis + oneSize) * (dis + oneSize) * 4)
                    {
                        float[,,] field = new float[oneSizePlus, oneSizePlus, oneSizePlus];
                        bool isChange = false;
                        for (int x2 = 0; x2 < oneSizePlus; x2++)
                        {
                            for (int y2 = 0; y2 < oneSizePlus; y2++)
                            {
                                for (int z2 = 0; z2 < oneSizePlus; z2++)
                                {
                                    if (densityField[x, y, z, x2, y2, z2] == 1)
                                    {
                                        if (Vector3.SqrMagnitude(child[x, y, z].transform.position + new Vector3(x2, y2, z2) - trm) < dis * dis)
                                        {
                                            densityField[x, y, z, x2, y2, z2] = 0;
                                            field[x2, y2, z2] = 0;
                                            isChange = true;
                                        }
                                        else
                                        {
                                            field[x2, y2, z2] = 1;
                                        }
                                    }
                                    else
                                    {
                                        field[x2, y2, z2] = 0;
                                    }
                                }
                            }
                        }
                        if (isChange)
                        {
                            mesh[x, y, z] = GenerateMesh(field);
                            meshFilter[x, y, z].mesh = mesh[x, y, z];
                            meshCollider[x, y, z].sharedMesh = meshFilter[x, y, z].mesh;
                        }
                    }
                }
            }
        }
    }
}