using System.Collections.Generic;
using UnityEngine;

public static class MarchingCubesAlgorithm
    {
        public static Mesh GenerateMesh(float[,,] densityField, float threshold)
        {
            int sizeX = densityField.GetLength(0);
            int sizeY = densityField.GetLength(1);
            int sizeZ = densityField.GetLength(2);

            int resolutionX = sizeX - 1;
            int resolutionY = sizeY - 1;
            int resolutionZ = sizeZ - 1;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // Marching Cubes �˰��� ����
            for (int x = 0; x < resolutionX; x++)
            {
                for (int y = 0; y < resolutionY; y++)
                {
                    for (int z = 0; z < resolutionZ; z++)
                    {
                        // ����ü �������� density ���� ������
                        float[] cubeDensity = new float[8];
                        cubeDensity[0] = densityField[x, y, z];
                        cubeDensity[1] = densityField[x + 1, y, z];
                        cubeDensity[2] = densityField[x + 1, y, z + 1];
                        cubeDensity[3] = densityField[x, y, z + 1];
                        cubeDensity[4] = densityField[x, y + 1, z];
                        cubeDensity[5] = densityField[x + 1, y + 1, z];
                        cubeDensity[6] = densityField[x + 1, y + 1, z + 1];
                        cubeDensity[7] = densityField[x, y + 1, z + 1];

                        // ����ü�� �ε��� ��� (�ش� ����ü�� ��Ÿ���� 8��Ʈ�� �������� �ε��� ����)
                        int cubeIndex = 0;
                        if (cubeDensity[0] < threshold) cubeIndex |= 1;
                        if (cubeDensity[1] < threshold) cubeIndex |= 2;
                        if (cubeDensity[2] < threshold) cubeIndex |= 4;
                        if (cubeDensity[3] < threshold) cubeIndex |= 8;
                        if (cubeDensity[4] < threshold) cubeIndex |= 16;
                        if (cubeDensity[5] < threshold) cubeIndex |= 32;
                        if (cubeDensity[6] < threshold) cubeIndex |= 64;
                        if (cubeDensity[7] < threshold) cubeIndex |= 128;

                        int[] edgeVertices = new int[12];
                        if (MarchingCubesTables.EdgeTable[cubeIndex] != 0)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                if ((MarchingCubesTables.EdgeTable[cubeIndex] & (1 << i)) != 0)
                                {
                                    int[] edgeIndices = new int[2];
                                    for (int k = 0; k < 2; k++)
                                    {
                                        edgeIndices[k] = MarchingCubesTables.EdgeIndices[i, k];
                                    }
                                    float t = (threshold - cubeDensity[edgeIndices[0]]) / (cubeDensity[edgeIndices[1]] - cubeDensity[edgeIndices[0]]);
                                    edgeVertices[i] = vertices.Count;
                                    vertices.Add(new Vector3(
                                        x + MarchingCubesTables.VertexOffset[edgeIndices[0], 0] + t * MarchingCubesTables.EdgeDirection[i, 0],
                                        y + MarchingCubesTables.VertexOffset[edgeIndices[0], 1] + t * MarchingCubesTables.EdgeDirection[i, 1],
                                        z + MarchingCubesTables.VertexOffset[edgeIndices[0], 2] + t * MarchingCubesTables.EdgeDirection[i, 2]
                                    ));
                                }
                            }
                        }


                    // Marching Cubes ���̺��� ���� �ش� ����ü�� �̷�� �ﰢ���� ����
                        for (int i = 0; i < 16; i += 3)
                        {
                            if (MarchingCubesTables.TriTable[cubeIndex, i] == -1)
                                break;

                            triangles.Add(edgeVertices[MarchingCubesTables.TriTable[cubeIndex, i + 2]]);
                            triangles.Add(edgeVertices[MarchingCubesTables.TriTable[cubeIndex, i + 1]]);
                            triangles.Add(edgeVertices[MarchingCubesTables.TriTable[cubeIndex, i]]);
                        }
                    }
                }
            }

            // �޽� ����
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
