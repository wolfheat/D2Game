using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    int width = 256;
    int length = 256;
	[SerializeField] int depth = -10;
    [SerializeField] int scale = 30;

    Terrain terrain;

    int[,] terrainLevel;

    Vector2Int startPosition;

	public void GenerateTerrain(int[,] levelIn,Vector2Int pos)
    {
        terrainLevel = LevelToTerrain(levelIn);
        startPosition = pos;
        terrain = GetComponent<Terrain>();
        GenerateTerrainData();
    }

    private int[,] LevelToTerrain(int[,] levelIn)
    {
        int scaling = 2;
		int[,] newTerrainLevel = new int[length*scaling, width*scaling];

        BoundsInt area = new BoundsInt(new Vector3Int(-2,-2,0), new Vector3Int(5,5,1));

		for (int x = 0; x < levelIn.GetLength(0); x++)
		{
			for (int y = 0; y < levelIn.GetLength(1); y++)
			{
                if (levelIn[x, y] == 0) continue;

                for (int i = area.xMin; i < area.xMax; i++)
                {
                    for (int j = area.yMin; j < area.yMax; j++)
                    {
				        newTerrainLevel[scaling * y+i, scaling * x+j] = 1;
                    }
                }
			}
		}
        return newTerrainLevel;

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

        int Xoffset = 128 + startPosition.y*2;
        int Yoffset = 128 + startPosition.x*2;   
        
		for (int x = 0; x < terrainLevel.GetLength(0); x++)
        {
            for (int y = 0; y < terrainLevel.GetLength(1); y++)
            {
                // If under level make 0
                if (terrainLevel[x, y] != 0)
                {
                    heightsArray[x+Xoffset, y+Yoffset] = 0;
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
