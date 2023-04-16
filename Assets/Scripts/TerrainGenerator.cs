using System;
using UnityEditor;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    int width = 256;
    int length = 256;
	[SerializeField] int depth = -10;
    [SerializeField] int scale = 30;

    public Terrain terrain;
    public Terrain storedTerrain;

    int[,] terrainLevel;
    float[,] terrainDataArray;

    Vector2Int startPosition;

	public void GenerateTerrain(int[,] levelIn,Vector2Int pos)
    {
        terrainLevel = LevelToTerrain(levelIn);
        startPosition = pos;
        UpdateTerrain();
        GenerateTerrainData();
    }

    public void UpdateTerrain()
    {
		terrain = GetComponent<Terrain>();
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
		terrainDataArray = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
				// If under level make 0
				terrainDataArray[x,y] = CalculateHeight(x,y);
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
					terrainDataArray[x+Xoffset, y+Yoffset] = 0;
                }
            }
        }
        
        return terrainDataArray;
    }

    private float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width *scale;   
        float yCoord = (float)y / length*scale;
        return Mathf.PerlinNoise(xCoord,yCoord);
    }

	internal void LoadTerrain()
	{
		terrain = GetComponent<Terrain>();
		Debug.Log("LOADD Terrain");
        terrain = storedTerrain;
	}
    
	internal void StoreTerrain()
	{
		terrain = GetComponent<Terrain>();
		Debug.Log("STORE Terrain");
        storedTerrain = terrain;
	}

	internal Terrain GetTerrain()
	{
        return GetComponent<Terrain>();
	}
	internal void ApplyTerrain(float[,] terraDataArrayIn)
	{
        Debug.Log("Applyu terrain");
        terrainDataArray = terraDataArrayIn;
        terrain.terrainData.SetHeights(0,0,terrainDataArray);
	}

	internal float[,] GetTerrainDataArray()
	{
        return terrainDataArray;
	}
}
