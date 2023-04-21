using System;
using UnityEditor;
using UnityEngine;
using Math = System.Math;

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
        ///* Try to better version
        startPosition = pos;
        tileSize = tileSizeIn;
        terrainLevel = LevelToTerrainB(levelIn);
        CalculateTerrainSizeNeeded(levelIn,pos);
        SetTerrainDataStats();

        GenerateTerrainData();
        
        /*
        terrainLevel = LevelToTerrain(levelIn);
        startPosition = pos;
        GenerateTerrainData();
        */
    }

    private void CalculateTerrainSizeNeeded(int[,] level, Vector2Int pos)
    {
        // Calculate top corner
        Vector2Int top = pos+ new Vector2Int(level.GetLength(0),level.GetLength(1));
        int maxX = Math.Max(top.x, Math.Abs(pos.x));        
        int maxY = Math.Max(top.y, Math.Abs(pos.y));
        int max = Math.Max(maxX, maxY);


        int margin = 20;
        int levelMaxWidthOrHeightUnscaled = max*2;
        int levelMaxWidthOrHeight = levelMaxWidthOrHeightUnscaled * tileSize+margin;
        int size = 1;
        while (size < levelMaxWidthOrHeight)
        {
            size *= 2;
        }
               
        Debug.Log("Size of Terrain set to: "+size+" Level was size: "+levelMaxWidthOrHeight);
        width = size;
        length = size;

        PlaceInCenter();

    }

    private void PlaceInCenter()
    {
        transform.position = new Vector3(-width/2,transform.position.y,-length/2);
    }

    private int[,] LevelToTerrainB(int[,] levelIn)
    {
        int border = 2;
        
		int[,] newTerrainLevel = new int[length*tileSize+border*2, width*tileSize+border*2];


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
				        newTerrainLevel[border + j + tileSize * y, border + i + tileSize * x] = 1; 
                    }
                }
			}
		}

        /*
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
        */
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

    private void SetTerrainDataStats()
    {
        ActivateCorrectTerrain();

        if(width<=512)terrain.terrainData.SetDetailResolution(width, length);
        else terrain.terrainData.SetDetailResolution(512, 512);
        terrain.terrainData.heightmapResolution = width + 1;
        terrain.terrainData.size = new Vector3(width, depth, length);
    }
    
    private void GenerateTerrainData()
    {       
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

        int Xoffset = width/2 + startPosition.y*tileSize-2;
        int Yoffset = length/2 + startPosition.x*tileSize-2;   

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
        float xCoord = (float)x / 256 *scale;   
        float yCoord = (float)y / 256*scale;
        return Mathf.PerlinNoise(xCoord,yCoord);
    }
 }
