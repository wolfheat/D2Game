using System;
using UnityEditor;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    int width = 256;
    int length = 256;
    int tileSize = 2;
	[SerializeField] int depth = -10;
    [SerializeField] int scale = 30;

    public Terrain terrain;
    public Terrain runtimeTerrain;
    public Terrain editorTerrain;

    int[,] terrainLevel;
    float[,] terrainDataArray;

    [SerializeField] TerrainStoredSO storedTerrainData;

    Vector2Int startPosition;

	public void GenerateTerrain(int[,] levelIn,Vector2Int pos, int tileSizeIn)
    {
        /* Try to better version
        startPosition = pos;
        tileSize = tileSizeIn;
        terrainLevel = LevelToTerrainB(levelIn);
        CalculateTerrainSizeNeeded(levelIn);

        GenerateTerrainData();
        */
        
        terrainLevel = LevelToTerrain(levelIn);
        startPosition = pos;
        GenerateTerrainData();
    }

    private void CalculateTerrainSizeNeeded(int[,] level)
    {
        int margin = 20;
        int levelMaxWidthOrHeightUnscaled = level.GetLength(0)>level.GetLength(1)?level.GetLength(0):level.GetLength(1);
        int levelMaxWidthOrHeight = levelMaxWidthOrHeightUnscaled * tileSize+margin;
        int size = 1;
        while (size < levelMaxWidthOrHeight)
        {
            size *= 2;
        }
               
        Debug.Log("Size of Terrain set to: "+size+" Level was size: "+levelMaxWidthOrHeight);
        width = size;
        length = size;
    }

    private int[,] LevelToTerrainB(int[,] levelIn)
    {
        
		int[,] newTerrainLevel = new int[length*tileSize, width*tileSize];

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
				        newTerrainLevel[(-area.xMin)+ j + tileSize * y+(-area.yMin)+i, tileSize * x+j] = 1; 
                    }
                }
			}
		}

        // Calculate Holes under
        bool[,] holes = new bool[newTerrainLevel.GetLength(0), newTerrainLevel.GetLength(1)];
        for (int x = -area.xMin; x < newTerrainLevel.GetLength(0)-area.xMax-1; x++)
        {
            for (int y = -area.yMin; y < newTerrainLevel.GetLength(1)-area.yMax-1; y++)
            {

                bool isHole = true;
                for (int i = area.xMin; i < area.xMax; i++)
                {
                    for (int j = area.yMin; j < area.yMax; j++)
                    {
                        if (newTerrainLevel[x + i, y + j] != 1)
                        {
                            isHole = false;
                            continue;
                        }
                    }
                    if (!isHole) continue;
                }                
                if(isHole) holes[x, y] = true;
            }
        }

        // Add the Holes
        for (int x = 0; x < newTerrainLevel.GetLength(0); x++)
        {
            for (int y = 0; y < newTerrainLevel.GetLength(1); y++)
            {
                if (holes[x, y] == true) newTerrainLevel[x, y] = -1;
            }
        }
        //t.terrainData.SetHoles(0, 0, b);
        
        return newTerrainLevel;

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

        // Calculate Holes under
        bool[,] holes = new bool[newTerrainLevel.GetLength(0), newTerrainLevel.GetLength(1)];
        for (int x = -area.xMin; x < newTerrainLevel.GetLength(0)-area.xMax-1; x++)
        {
            for (int y = -area.yMin; y < newTerrainLevel.GetLength(1)-area.yMax-1; y++)
            {

                bool isHole = true;
                for (int i = area.xMin; i < area.xMax; i++)
                {
                    for (int j = area.yMin; j < area.yMax; j++)
                    {
                        if (newTerrainLevel[x + i, y + j] != 1)
                        {
                            isHole = false;
                            continue;
                        }
                    }
                    if (!isHole) continue;
                }                
                if(isHole) holes[x, y] = true;
            }
        }

        // Add the Holes
        for (int x = 0; x < newTerrainLevel.GetLength(0); x++)
        {
            for (int y = 0; y < newTerrainLevel.GetLength(1); y++)
            {
                if (holes[x, y] == true) newTerrainLevel[x, y] = -1;
            }
        }
        //t.terrainData.SetHoles(0, 0, b);
        
        return newTerrainLevel;

	}

    private void GenerateTerrainData()
    {
        ActivateCorrectTerrain();  
            
		terrain.terrainData.SetDetailResolution(width,length);
		terrain.terrainData.heightmapResolution = width+1;
		terrain.terrainData.size = new Vector3(width,depth,length);
		terrain.terrainData.SetHeights(0,0,GenerateHeights());
        
    }

	private void ActivateCorrectTerrain()
	{
        if (Application.isPlaying)
        {
            terrain = runtimeTerrain;
            runtimeTerrain.GetComponent<Terrain>().enabled = true;
            editorTerrain.GetComponent<Terrain>().enabled = false;
		}
        else
        {
            terrain = editorTerrain;
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

        int Xoffset = width/2 + startPosition.y*tileSize;
        int Yoffset = length/2 + startPosition.x*tileSize;   

        for (int x = 0; x < terrainLevel.GetLength(0); x++)
        {
            for (int y = 0; y < terrainLevel.GetLength(1); y++)
            {
                // If under level make 0
                if (terrainLevel[x, y] == 1)
                {
					terrainDataArray[x + Xoffset, y + Yoffset] = 0;
                }else if (terrainLevel[x, y] == -1)
                {
                    terrainDataArray[x + Xoffset, y + Yoffset] = 0.1f;
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
 }
