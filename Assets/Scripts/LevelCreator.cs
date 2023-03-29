using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using DelaunayVoronoi;
using System.Net;

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

	[Space(10, order = 0)]
	[Header ("Separation Dungeon Creator")]
	[SerializeField] private int SeparationRoomsAmt = 5;
	[SerializeField] private int SeparateTries = 10;

	[Space(10, order = 0)]
	[Header ("Random Walk Dungeon Creator")]
	[SerializeField] private RandomWalkGeneratorPresetSO walkGeneratorPreset;

	IEnumerable<Triangle> delaunay;
	List<PathPoint> delaunayPathPoints;


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
	}

	private void GenerateLavaFloorTileAt(Vector2Int pos)
	{
		GameObject tile;
		tile = Instantiate(floorTilesPrefab[7], TileHolder.transform);
		tile.transform.localPosition = new Vector3(pos.x * Tilesize, 0, pos.y * Tilesize);
		tile.layer = GroundLayer;
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
			int specialTile = Random.Range(0, 7);
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


	public void CreateRoomDispersionDungeon()
	{

		// Generate New Room Move until not overlapping

		Debug.Log("CreateRoomDispersionDungeon RUN");
		//Clear The level
		ClearLevel();

		// Generate the level
		List<Room> rooms = new List<Room>();


		int totalMovementsNeeded = 0;
		int maxIterations = 100;
		int iteration;
		// Generate X Rooms at Distance X from center at random
		for (int i = 0; i < SeparationRoomsAmt; i++)
		{
			iteration = 0;
			Room newRoom = RoomMaker.GenerateRandomRoom();
			while (RoomMaker.OverlapAny(newRoom, rooms) && iteration<maxIterations)
			{
				newRoom.Move();
				iteration++;
				totalMovementsNeeded++;
			}
			if (iteration == maxIterations)
			{
				Debug.LogWarning("Move new room iteration reached MAX, stopped moving overlapping room. Room at: "+newRoom.tempPos);

			}
			//Lock Room and Add to Rooms
			rooms.Add(newRoom);
		}
		Debug.Log("Creation of Dispersion Dungeon needed "+totalMovementsNeeded+" movements to complete.");

		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

		// Select X largest rooms as main rooms.		
		List<Room> mainRooms = rooms.OrderByDescending(r => r.size.magnitude).Take(8).ToList();
		List<Room> restRooms = rooms.OrderBy(r => r.size.magnitude).Take(rooms.Count-8).ToList();
		
		
		List<Vector2Int> roomCenters = new List<Vector2Int>();

		foreach (Room room in mainRooms)
		{
			roomCenters.Add(room.GetCenter());
		}


		string roomSizeString = "";

		for (int i = 0; i < mainRooms.Count; i++)
		{
			roomSizeString += mainRooms[i].size.magnitude+" , ";
		}
		Debug.Log("Room Sizes: "+ roomSizeString);


		// Find Dalaunay Triangulation lines

		DelaunayTriangulator delaunayTriangulator = new DelaunayTriangulator();
		
		delaunay = delaunayTriangulator.BowyerWatson(roomCenters);

		Debug.Log("Delaunay Triangles Created");
		Debug.Log("Delaunay Triangles Amount: "+delaunay.Count());

		delaunayPathPoints = DelaunayTriangulator.FindMinimumPath(delaunay);

		// Make Hallway Lines between Main Rooms

		// Activate secondaryRooms that these hallways intercept

		// Add in a Main Hallway of a certain width around the hallway lines





		// Generate Map to show it
		foreach (Room room in mainRooms){floorPositions.UnionWith(GetAllRoomTiles(room));}
		GenerateAllTilesForThisFloorPosition(floorPositions);
		floorPositions.Clear();
		foreach (Room room in restRooms){floorPositions.UnionWith(GetAllRoomTiles(room));}
		GenerateAllTilesForThisFloorPosition(floorPositions,true);

		SetPlayerAtStart(walkGeneratorPreset.playerStartPosition);
	}

	private void OnDrawGizmos()
	{
		
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Vector3.up, Vector3.right);
		/*
		if (delaunay != null)
		{
			Gizmos.color = Color.red;

			foreach (Triangle triangle in delaunay)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector3 startPoint = new Vector3((float)triangle.Vertices[i].X, 0.2f, (float)triangle.Vertices[i].Y);
					Vector3 endPoint = new Vector3((float)triangle.Vertices[(i+1)%3].X, 0.2f, (float)triangle.Vertices[(i + 1) % 3].Y);
					//Debug.Log("Drawing line "+startPoint+" to "+endPoint);
					Gizmos.DrawLine(startPoint*2, endPoint*2);		
				}
			}
		}*/

		if (delaunayPathPoints != null)
		{
			Gizmos.color = Color.green;

			foreach (PathPoint pathPoint in delaunayPathPoints)
			{
				foreach (var connectedNeighbor in pathPoint.connectedNeighbors)
				{
					Vector3 startPoint = new Vector3(pathPoint.pos.x, 0.2f, pathPoint.pos.y);
					//PathPoint closestNeighbor = pathPoint.closest;
					Vector3 endPoint = new Vector3(connectedNeighbor.pos.x, 0.2f, connectedNeighbor.pos.y);
					//Debug.Log("Drawing line "+startPoint+" to "+endPoint);
					Gizmos.DrawLine(startPoint*2, endPoint*2);		
				}

			}
		}
		UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
	}

	public void CreateRoomSeparationDungeon()
	{
		Debug.Log("CreateRoomSeparationDungeon RUN");
		//Clear The level
		ClearLevel();

		// Generate the level
		List<Room> rooms = new List<Room>(); 


		// Generate X Rooms at Distance X from center at random
		for (int i = 0; i < SeparationRoomsAmt; i++)
		{
			Room newRoom = RoomMaker.GenerateRandomRoom();
			rooms.Add(newRoom);
		}

		// Separate Rooms Until they dont overlap
		for (int i = 0; i < SeparateTries; i++)
		{
			RoomMaker.SetClosestRooms(rooms);
			RoomMaker.MoveOverlap(rooms);
		}



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

	private void GenerateAllTilesForThisFloorPosition(HashSet<Vector2Int> floorPositions,bool IsLava = false)
	{
		foreach (var floor in floorPositions)
		{
			if (IsLava) GenerateLavaFloorTileAt(floor);
			else GenerateFloorTileAt(floor);
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
	private HashSet<Vector2Int> GetAllRoomTiles(Room room)
	{
		HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();
		for (int i = 0; i < room.size.x; i++)
		{
			for (int j = 0; j < room.size.y; j++)
			{			
				tiles.Add(new Vector2Int(room.pos.x+i, room.pos.y + j));
			}
		}
		return tiles;
	}
}
