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
    public Terrain runtimeTerrain;
    public Terrain editorTerrain;

    int[,] terrainLevel;
    float[,] terrainDataArray;
    float[,] storedTerrainDataArray;

    [SerializeField] TerrainStoredSO storedTerrainData;

    Vector2Int startPosition;

	public void GenerateTerrain(int[,] levelIn,Vector2Int pos)
    {
        terrainLevel = LevelToTerrain(levelIn);
        startPosition = pos;
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
        ActivateCorrectTerrain();
        if (Application.isPlaying)
        {
            runtimeTerrain.terrainData.heightmapResolution = 257;
		    runtimeTerrain.terrainData.size = new Vector3(width,depth,length);
		    runtimeTerrain.terrainData.SetHeights(0,0,GenerateHeights());
        }
        else
        {
			editorTerrain.terrainData.heightmapResolution = 257;
			editorTerrain.terrainData.size = new Vector3(width, depth, length);
			editorTerrain.terrainData.SetHeights(0, 0, GenerateHeights());
		}

    }

	private void ActivateCorrectTerrain()
	{
        if (Application.isPlaying)
        {
            runtimeTerrain.GetComponent<Terrain>().enabled = true;
            editorTerrain.GetComponent<Terrain>().enabled = false;
		}
        else
        {
			editorTerrain.GetComponent<Terrain>().enabled = true;
			runtimeTerrain.GetComponent<Terrain>().enabled = false;
		}
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
        ActivateCorrectTerrain();
	}

	internal void StoreTerrain()
	{
        // Should autostore"
        //editorTerrain = terrain;        
	}

	private int CountNonZeros()
	{
        Debug.Log("Counting Zeros, nonzerosstoredsince before: "+storedTerrainData.amount);
        int amt = 0;
		for (int i = 0; i < terrainDataArray.GetLength(0); i++)
		{
			for (int j = 0; j < terrainDataArray.GetLength(1); j++)
			{
				if (terrainDataArray[i, j] == 0)
				{
					amt++;
				}
			}
		}
        Debug.Log("NON ZEROS: "+amt);
        return amt;
	}
}
