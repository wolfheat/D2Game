using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public enum Direction {left,down,right,up}


public static class RandomWalkGenerator
{
	public static HashSet<Vector2Int> RandomWalk(Vector2Int start, int walkLength)
	{
		HashSet<Vector2Int> result = new HashSet<Vector2Int>();
		Vector2Int oldStep = start;

		for (int i = 0; i < walkLength; i++)
		{
			Vector2Int newStep = oldStep + RandomStep();
			result.Add(newStep);
			oldStep = newStep;
		}
		return result;
	}

	private static List<Vector2Int> stepList = new List<Vector2Int>() { Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};

	private static Vector2Int RandomStep()
	{
		return stepList[Random.Range(0,stepList.Count)];
	}
}

public class LevelCreator : MonoBehaviour
{
    //[SerializeField] private GameObject playerControllerParent;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIController UIController;

    [SerializeField] private List<GameObject> floorTilesPrefab;
    [SerializeField] private List<GameObject> wallTilesPrefab;
    [SerializeField] private List<GameObject> debreePrefabs;
    [SerializeField] private List<GameObject> decalPrefabs;

    [SerializeField] private List<GameObject> resourcesPrefab;

    [SerializeField] private GameObject highLightSquare;
    [SerializeField] private NavMeshSurface TileHolder;

    private List<Mesh> surfacMeshes = new List<Mesh>();
    int GroundLayer;
    int ResourceLayer;
	public const float Tilesize = 2f;
    private Vector2Int Levelsize = new Vector2Int(25,15);

	private void Start()
    {
        GroundLayer = LayerMask.NameToLayer("Ground");
        ResourceLayer = LayerMask.NameToLayer("Resources");
		Inputs.Instance.Controls.Land.X.performed += _ => PrintCurrentTilePosition();
        
        //CreateStartLevel();

        CreateGeneratedLevel();

		BakeLevelNavMesh();
		RequestActivatePlayerNavmesh();
	}

    private void CreateGeneratedLevel()
    {
        //Generate the level

		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		Vector2Int playerStartPosition = new Vector2Int(10, 10);
		int iterations = 10;
		int walkLength = 10;

		for (int k = 0; k < iterations; k++)
		{
			floorPositions.UnionWith(RandomWalkGenerator.RandomWalk(playerStartPosition,walkLength));
		}

		foreach (var floor in floorPositions)
		{
			GenerateFloorTileAt(floor);
			//GenerateFloorTileAt(new Vector2Int(floor.x,floor.y));
		}

		SetPlayerAtStart(playerStartPosition);
	}

	private void GenerateFloorTileAt(Vector2Int pos)
	{

		GameObject tile;
		int specialType = Random.Range(0, 11);
		if (specialType < 10)
		{
			tile = Instantiate(floorTilesPrefab[0], TileHolder.transform);
			if (specialType < 1)
			{
				int debreeType = Random.Range(0, debreePrefabs.Count);
				//Do debree
				GameObject debree = Instantiate(debreePrefabs[debreeType], tile.transform);
				debree.transform.localPosition = Vector3.zero;
			}
			else if (specialType < 2)
			{
				//DECAL
				int decalType = Random.Range(0, decalPrefabs.Count);
				//Do debree
				GameObject decal = Instantiate(decalPrefabs[decalType], tile.transform);
				decal.transform.localPosition = Vector3.zero;
				decal.gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
			}
		}
		else
		{
			int specialTile = Random.Range(0, floorTilesPrefab.Count);
			tile = Instantiate(floorTilesPrefab[specialTile], TileHolder.transform);
		}
		tile.transform.localPosition = new Vector3(pos.x * Tilesize, 0, pos.y * Tilesize);

		tile.layer = GroundLayer;


	}

	private void CreateStartLevel()
    {
		Vector2Int[] doorsAt;
		SetPlayerAtStart(new Vector2Int(3, 3));

		doorsAt = new Vector2Int[] { new Vector2Int(2, 5) };
		CreateMiniLevel(new Vector2Int(5, 6), Vector2Int.zero, doorsAt);

		doorsAt = new Vector2Int[] { new Vector2Int(4, 0), new Vector2Int(4, 8) };
		CreateMiniLevel(new Vector2Int(9, 9), new Vector2Int(-2, 6), doorsAt);

		doorsAt = new Vector2Int[] { new Vector2Int(2, 0) };
		CreateMiniLevel(new Vector2Int(5, 6), new Vector2Int(0, 15), doorsAt);


		CreateResourceAt(new Vector2Int(5, 8), 0);


		Debug.Log("Level created");

	}

	public void PrintCurrentTilePosition()
	{
        if (Inputs.Instance.Shift == 1)
        {
            Vector2Int newPos = new Vector2Int((int)((playerController.transform.position.x+0.75f) / Tilesize), (int)((playerController.transform.position.z + 0.75f) / Tilesize));
            Debug.Log("Player is at " + newPos);
            StartCoroutine(ShowHighlightSquare(newPos));
            UIController.SetInfoText("Player is at tile: " + newPos+" \nactualPos: " + playerController.transform.position);
		}
	}
    private IEnumerator ShowHighlightSquare(Vector2Int pos)
    {
        highLightSquare.transform.position = new Vector3(pos.x*Tilesize,0,pos.y*Tilesize);
		highLightSquare.SetActive(true);
        yield return new WaitForSeconds(1f);
		highLightSquare.SetActive(false);
	}

	private void SetPlayerAtStart(Vector2Int pos)
    {
        playerController.SetToPosition(new Vector3(pos.x*Tilesize,0,pos.y*Tilesize));
    }

    private void RequestActivatePlayerNavmesh()
    {
        playerController.EnableNavMesh(true);
	}

    private void BakeLevelNavMesh()
    {
        Debug.Log("Set UploadMeshData to false");

        
        foreach(Mesh mesh in surfacMeshes)
        {
            mesh.UploadMeshData(false);
        // Bake the NavMesh
        }
        Debug.Log("Set UploadMeshData to false: DONE");
        Debug.Log("Build NavMesh");
        TileHolder.BuildNavMesh();
        Debug.Log("Mesh Built Now set UploadMeshData to true");
        foreach(Mesh mesh in surfacMeshes)
        {
            mesh.UploadMeshData(true);
        }
		
    }

	private void CreateResourceAt(Vector2Int position, int type)
	{
		//Create Resource
		GameObject resource = Instantiate(resourcesPrefab[type], TileHolder.transform);
        resource.transform.position = new Vector3(position.x*Tilesize,0,position.y*Tilesize);
        resource.layer = ResourceLayer;

	}

	private void CreateMiniLevel(Vector2Int roomSize, Vector2Int[] doorsAt)
	{
        CreateMiniLevel(roomSize, Vector2Int.zero, doorsAt);
    }
	private void CreateMiniLevel(Vector2Int roomSize, Vector2Int offset, Vector2Int[] doorsAt)
	{

        GameObject room = new GameObject("Room");
        room.transform.parent = TileHolder.transform;
        room.transform.localPosition = new Vector3(offset.x*Tilesize,0,offset.y*Tilesize);

		//Create First Room = start Room 
		for (int i = 0; i < roomSize.x; i++)
		{
			for (int j = 0; j < roomSize.y; j++)
			{
                //FLOOR TILE
				GameObject tile;
				int specialType = Random.Range(0, 11);
				if (specialType < 10)
				{
					tile = Instantiate(floorTilesPrefab[0], room.transform);
					if (specialType < 1)
					{
						int debreeType = Random.Range(0, debreePrefabs.Count);
						//Do debree
						GameObject debree = Instantiate(debreePrefabs[debreeType], tile.transform);
						debree.transform.localPosition = Vector3.zero;
					}
					else if (specialType < 2)
					{
						//DECAL
						int decalType = Random.Range(0, decalPrefabs.Count);
						//Do debree
						GameObject decal = Instantiate(decalPrefabs[decalType], tile.transform);
						decal.transform.localPosition = Vector3.zero;
						decal.gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
					}
				}
				else
				{
					int specialTile = Random.Range(0, floorTilesPrefab.Count);
					tile = Instantiate(floorTilesPrefab[specialTile], room.transform);
				}
				tile.transform.localPosition = new Vector3(i * Tilesize, 0, j * Tilesize);

				//foreach (Transform child in tile.transform) child.gameObject.layer = GroundLayer;

				tile.layer = GroundLayer;

                //WALL TILES
                GameObject wall;

                
                if(i==0 || i == roomSize.x-1 || j==0 || j== roomSize.y-1)
                {

					bool isDoor = false;
					//Check For Door
					foreach (var door in doorsAt)
					{
						if (door.x == i && door.y == j)
						{
							isDoor = true;
							break;
						}
					}

                    // Set Position
                    if (i==0)
                    {
                        CreateWallAt(Direction.left,tile,isDoor?1:0);
				    }
                    if (i==roomSize.x-1)
                    {
                        CreateWallAt(Direction.right,tile,isDoor?1:0);
				    }
                    if (j==0)
                    {
                        CreateWallAt(Direction.down,tile,isDoor?1:0);
				    }
                    if (j==roomSize.y-1)
                    {
                        CreateWallAt(Direction.up,tile,isDoor?1:0);                        
				    }
				}
			}
		}
	}

    private GameObject CreateWallAt(Direction direction,GameObject tile,int type)
    {
		GameObject wall;
		wall = Instantiate(wallTilesPrefab[type], tile.transform);
		wall.layer = GroundLayer;

		switch (direction)
        {
            case Direction.left:
				wall.transform.localPosition = new Vector3(-Tilesize / 2, 0, 0);
                break;
            case Direction.down:
				wall.transform.localPosition = new Vector3(0, 0, -Tilesize / 2);
				wall.transform.Rotate(0, 90f, 0);
				break;
            case Direction.right:
				wall.transform.localPosition = new Vector3(Tilesize / 2, 0, 0);
				break;
            case Direction.up:
				wall.transform.localPosition = new Vector3(0, 0, Tilesize / 2);
				wall.transform.Rotate(0, 90f, 0);
				break;
			default:
                break;
        }
        return wall;
    }

    private void CreateLevel()
    {
        for (int i = 0; i < Levelsize.x; i++)
        {
            for (int j = 0; j < Levelsize.y; j++)
            {
                GameObject tile;
				int specialType = Random.Range(0,11);
                if (specialType < 10)
                {
                    tile = Instantiate(floorTilesPrefab[0], TileHolder.transform);
                    if (specialType < 1)
                    {
                        int debreeType = Random.Range(0, debreePrefabs.Count);
                        //Do debree
                        GameObject debree = Instantiate(debreePrefabs[debreeType], tile.transform);
                        debree.transform.localPosition = Vector3.zero;
					}else if(specialType < 2)
                    {
						//DECAL
						int decalType = Random.Range(0, decalPrefabs.Count);
						//Do debree
						GameObject decal = Instantiate(decalPrefabs[decalType], tile.transform);
						decal.transform.localPosition = Vector3.zero;
						decal.gameObject.transform.Rotate(0, Random.Range(0,360), 0);
					}
                }
                else
                {
                    int specialTile = Random.Range(0,floorTilesPrefab.Count);
                    tile = Instantiate(floorTilesPrefab[specialTile], TileHolder.transform);
                }
                tile.transform.position = new Vector3(i * Tilesize, 0, j * Tilesize);

				//foreach (Transform child in tile.transform) child.gameObject.layer = GroundLayer;

				tile.layer = GroundLayer;

				//surfacMeshes.Add(tile.GetComponent<MeshFilter>().mesh);

				//tile.gameObject.transform.Rotate(0, 90 * rot, 0);
			}
        }
    }
}
