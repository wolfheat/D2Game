using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public enum Direction {left,down,right,up}

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

	[SerializeField] private RandomWalkGeneratorPresetSO walkGeneratorPreset;

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

    public void CreateGeneratedLevel()
    {
		//Clear The level
		ClearLevel();
        //Generate the level
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

		Vector2Int startPos = walkGeneratorPreset.playerStartPosition;

		for (int k = 0; k < walkGeneratorPreset.iterations; k++)
		{
			floorPositions.UnionWith(RandomWalkGenerator.RandomWalk(startPos, walkGeneratorPreset.walkLength));
			if(walkGeneratorPreset.startRandom) startPos = floorPositions.ElementAt(floorPositions.Count-1);
		}

		foreach (var floor in floorPositions)
		{
			GenerateFloorTileAt(floor);
			//GenerateFloorTileAt(new Vector2Int(floor.x,floor.y));
		}

		SetPlayerAtStart(walkGeneratorPreset.playerStartPosition);
	}

	public void ClearLevel()
	{
		Transform[] children = TileHolder.GetComponentsInChildren<Transform>();
		for (int i = children.Length-1; i > 0; i--)	
			DestroyImmediate(children[i].gameObject);
		Debug.Log("Transforms left in Array: "+children.Length);
		children = null;
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

	public void CreateRoomSeparationDungeon()
	{
		Debug.Log("CreateRoomSeparationDungeon RUN");
		//Clear The level
		ClearLevel();

		// Generate the level
		List<Room> rooms = new List<Room>(); 


		// Generate X Rooms at Distance X from center at random
		for (int i = 0; i < 3; i++)
		{
			Room newRoom = RoomMaker.GenerateRandomRoom();
			rooms.Add(newRoom);
		}

		// Separate Rooms Until they dont overlap

		RoomMaker.SetClosestRooms(rooms);
		RoomMaker.MoveOverlap(rooms);














		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

		// Generate Map to show it
		foreach (Room room in rooms)
		{
			floorPositions.UnionWith(GetRoomBorders(room));
		}



		// Select X largest rooms as main rooms.

		// Find Dalaunay Triangulation lines

		// Make Hallway Lines between Main Rooms

		// Activate secondaryRooms that these hallways intercept

		// Add in a Main Hallway of a certain width around the hallway lines


		GenerateAllTilesForThisFloorPosition(floorPositions);
		SetPlayerAtStart(walkGeneratorPreset.playerStartPosition);
	}

	private void GenerateAllTilesForThisFloorPosition(HashSet<Vector2Int> floorPositions)
	{
		foreach (var floor in floorPositions)
		{
			GenerateFloorTileAt(floor);
		}

	}

	private HashSet<Vector2Int> GetRoomBorders(Room room)
	{
		HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();
		for (int i = 0; i < room.size.x; i++)
		{
			for (int j = 0; j < room.size.y; j++)
			{
				if(i==0 || j == 0 || i == room.size.x - 1 || j == room.size.y - 1)
					tiles.Add(new Vector2Int(room.pos.x+i, room.pos.y + j));
			}
		}
		return tiles;
	}
}

public static class RoomMaker
{
	private static int RoomMinSize = 4;
	private static int RoomMaxSize = 10;
	private static int RoomMaxDev = 3;
	private static int MoveStrength = 2;

	public static Room GenerateRandomRoom()
	{
		return new Room(
			new Vector2Int(Random.Range(RoomMinSize,RoomMaxSize), Random.Range(RoomMinSize, RoomMaxSize)), 
			new Vector2Int(Random.Range(-RoomMaxDev, RoomMaxDev), Random.Range(-RoomMaxDev, RoomMaxDev)));
	}

	internal static bool Overlap(Room r1, Room r2)
	{
		bool overlapX = r1.pos.x <= r2.pos.x+r2.size.x && r1.pos.x+r1.size.x >= r2.pos.x;
		bool overlapY = r1.pos.y <= r2.pos.y+r2.size.y && r1.pos.y+r1.size.y >= r2.pos.y;
		return overlapX && overlapY;		
	}
	
	internal static void MoveOverlap(List<Room> rooms)
	{
		foreach (var room in rooms)
		{
			bool overlap = false;
			foreach (var roomToCheck in room.closest)
			{
				if (Overlap(room, roomToCheck))
				{
					overlap = true;
					break;
				}
			}
			MoveAwayFromClosest(room);
		}
	}

	private static void MoveAwayFromClosest(Room room)
	{
		Vector2 moveDirection = Vector2.zero;
		foreach (Room otherRoom in room.closest)
		{
			Debug.Log("Room Difference: "+ (room.pos - otherRoom.pos)+" Room1: "+room.pos+" Room2: "+otherRoom.pos);
			moveDirection += (room.pos - otherRoom.pos);
		}
		moveDirection = moveDirection.normalized;
		Vector2Int moveVector = Vector2Int.RoundToInt(moveDirection * MoveStrength);
		Debug.Log("MoveVector "+moveVector + "because "+moveDirection);
		Debug.Log("Moving Room from "+room.pos+" to "+(room.pos + moveVector));
		room.pos += moveVector;
	}

	internal static void SetClosestRooms(List<Room> rooms)
	{
		foreach (var room in rooms)
		{
			List<Room> closestRooms = new List<Room>();
			foreach (var checkedRoom in rooms)
			{
				if (room == checkedRoom) continue;
				closestRooms.Add(checkedRoom);
				closestRooms.Sort((r1, r2) => Vector2Int.Distance(room.pos, r1.pos).CompareTo(Vector2Int.Distance(room.pos, r2.pos)));
			}
			room.closest = closestRooms;
		}	

	}
}
public class Room
{
	public Vector2Int size;
	public Vector2Int pos;
	public List<Vector2Int> corners;
	public List<Room> closest;

	public Room(Vector2Int s, Vector2Int p)
	{
		size = s;
		pos = p;
		corners = new List<Vector2Int>() { p,p+s-Vector2Int.one,new Vector2Int(p.x+s.x-1,p.y), new Vector2Int(p.x, p.y + s.y-1)};
	}
}