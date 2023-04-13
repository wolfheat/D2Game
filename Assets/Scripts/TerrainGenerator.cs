using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    int width = 256;
    int length = 256;
	[SerializeField] int depth = -10;
    [SerializeField] int scale = 30;

    Terrain terrain;

    int[,] level;

    Vector2Int startPosition;

	public void GenerateTerrain(int[,] levelIn,Vector2Int pos)
    {
        level = levelIn;
        startPosition = pos;
        terrain = GetComponent<Terrain>();
        GenerateTerrainData();
    }

    private void GenerateTerrainData()
    {
        terrain.terrainData.heightmapResolution = 257;
		terrain.terrainData.size = new Vector3(width,depth,length);
        terrain.terrainData.SetHeights(0,0,GenerateHeights());        
    }

    private float[,] GenerateHeights()
    {
        float[,] heightsArray = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                // If under level make 0
                heightsArray[x,y] = CalculateHeight(x,y);
            }
        }

        int Xoffset = 128 + startPosition.x*2;
        int Yoffset = 128 + startPosition.y*2;   

		for (int x = 0; x < level.GetLength(0); x++)
        {
            for (int y = 0; y < level.GetLength(1); y++)
            {
                // If under level make 0
                if (level[x, y] != 0)
                {
                    heightsArray[2 * y + Yoffset, 2 * x + Xoffset] = 0;
                    heightsArray[2 * y + 1 + Yoffset,2 * x + Xoffset] = 0;
                    heightsArray[2 * y + 1 + Yoffset,2 * x + 1 + Xoffset] = 0;
                    heightsArray[2 * y + Yoffset,2 * x + 1 + Xoffset] = 0;
                }
            }
        }
        return heightsArray;
    }

    private float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width *scale;   
        float yCoord = (float)y / length*scale;
        return Mathf.PerlinNoise(xCoord,yCoord);
    }
}
