using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using DelaunayVoronoi;
using UnityEditor;

public enum Direction {left,down,right,up}

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private TerrainStoredSO storedTerrainSO;

    //[SerializeField] private GameObject playerControllerParent;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIController UIController;

    [SerializeField] private List<GameObject> floorTilesPrefab;
    [SerializeField] private List<GameObject> wallTilesPrefab;
    [SerializeField] private List<GameObject> debreePrefabs;
    [SerializeField] private List<GameObject> decalPrefabs;
    [SerializeField] private List<GameObject> specialPrefabs;

    [SerializeField] private List<GameObject> resourcesPrefab;
    [SerializeField] private GameObject enemySpawnPointPrefab;

    [SerializeField] private GameObject highLightSquare;
    [SerializeField] private NavMeshSurface TileHolder;

	[Space(10, order = 0)]
	[Header ("Separation Dungeon Creator")]
	[SerializeField] private int SeparationRoomsAmt = 5;
	[SerializeField] private int SeparateTries = 10;

	[Space(10, order = 0)]
	[Header ("Random Walk Dungeon Creator")]
	[SerializeField] private RandomWalkGeneratorPresetSO walkGeneratorPreset;

	IEnumerable<Triangle> delaunayTriangles;
	List<PathPoint> delaunayPathPoints;
	Dictionary<Point,List<Point>> delaunayDictionary;
	Dictionary<Point,List<Point>> delaunayCartesianPathsDictionary;

	[Space(10, order = 0)]
	[Header("Dispersion Dungeon Creator")]
	[SerializeField] private int DispersionRoomsAmt = 8;
	[SerializeField, Range(1f,5f)] private float HallwayRoomsRatio = 2.5f;
	[SerializeField] private bool ShowLines = false;
	[SerializeField] private bool ShowSize = true;
	[SerializeField] private bool VisualizeCorridors = true;

	List<DoorInfo> doorOpenings = new List<DoorInfo>();

	Vector2Int roomTypeStart;
	Vector2Int roomTypeSize;

	private TerrainGenerator terrainGenerator;
	private List<Mesh> surfacMeshes = new List<Mesh>();
    int GroundLayer;
    int ResourceLayer;
	public const float Tilesize = 2f;
	public const float EnemyDistanceFromPlayerSpawn = 8f;
	private Vector2Int Levelsize = new Vector2Int(25,15);

	int[,] roomType = new int[200, 200];
	Vector2Int roomTypeOffset = new Vector2Int(100, 100);

	private void Start()
    {
        GroundLayer = LayerMask.NameToLayer("Ground");
        ResourceLayer = LayerMask.NameToLayer("Resources");
		Inputs.Instance.Controls.Land.X.performed += _ => PrintCurrentTilePosition();

		terrainGenerator = FindObjectOfType<TerrainGenerator>();

		CreateNewLevel();

	}

	public void CreateNewLevel()
	{
		RequestActivatePlayerNavmesh(false);
		CreateRoomDispersionDungeon();
		BakeLevelNavMesh();
		RequestActivatePlayerNavmesh(true);
		
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
		Debug.Log("CLEARING LEVEL, CHILDREN BEFORE: "+children.Length);
		for (int i = children.Length-1; i > 0; i--)	
			DestroyImmediate(children[i].gameObject);		
	}

	private GameObject GenerateForcedFloorTileAt(Vector2Int pos,int index)
	{
		GameObject tile;
		tile = Instantiate(floorTilesPrefab[index], TileHolder.transform);
		tile.transform.localPosition = new Vector3(pos.x * Tilesize, 0, pos.y * Tilesize);
		tile.layer = GroundLayer;
		return tile;
	}

	private GameObject GenerateFloorTileAt(Vector2Int pos)
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

		return tile;
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
		Debug.Log("Set player at: " + pos);
        playerController.SetToPosition(new Vector3(pos.x*Tilesize,0,pos.y*Tilesize));
		Debug.Log("PLayerController now at: " + playerController.transform.position);
    }

    private void RequestActivatePlayerNavmesh(bool activate)
    {
        playerController.EnableNavMesh(activate);
	}

    private void BakeLevelNavMesh()
    {
        foreach(Mesh mesh in surfacMeshes)
        {
            mesh.UploadMeshData(false);
        }
        // Bake the NavMesh
        Debug.Log("Build NavMesh");
        TileHolder.BuildNavMesh();

        foreach(Mesh mesh in surfacMeshes)
        {
			// Make sure the new Mesh Data is Uploaded
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

	public void CreateRoomDispersionDungeon()
	{
		// Start Method Execution Timer 
		double time = EditorApplication.timeSinceStartup;

		//Clear The Level
		ClearLevel();

		// Generate The Level
		List<Room> rooms = RoomMaker.GenerateRandomDungeon(DispersionRoomsAmt, HallwayRoomsRatio);

		// Set Gameplay Area
		roomTypeStart = RoomMaker.GetStartVector(rooms);
		roomTypeSize = RoomMaker.GetSize(rooms);

		rooms = rooms.OrderByDescending(r => r.size.magnitude).ToList();

		//Separate Rooms into Main and Rest
		List<Room> mainRooms = rooms.Take(DispersionRoomsAmt).ToList();
		List<Room> restRooms = rooms.GetRange(DispersionRoomsAmt,rooms.Count-DispersionRoomsAmt).ToList();

		// Get all Rooms Centerpoints
		List<Vector2> roomCentersAsFloats = RoomMaker.GetCentersAsFloats(mainRooms);
		List<Vector2Int> roomCenters = RoomMaker.GetCentersAsInts(mainRooms);

		// Find Dalaunay Triangulation triangles
		delaunayTriangles = DelaunayTriangulator.GenerateBowyerWatsonFromList(roomCentersAsFloats);
		// Determine Minimum Path as Dictionary
		delaunayDictionary = DelaunayTriangulator.FindMinimumPath(delaunayTriangles);
		// Determine Actual Cartesian Paths
		delaunayCartesianPathsDictionary = DelaunayTriangulator.DelunayDictionaryToCartesianPaths(delaunayDictionary);

		// Select Rooms that the Cartesian Paths passes
		List<Room> selectedRestRooms = new List<Room>();
		selectedRestRooms = RoomMaker.SelectRooms(restRooms, delaunayCartesianPathsDictionary);

		// Remove the selected rooms from the remaining rooms, remove this restRooms array later, currently not needed any more
		restRooms.RemoveAll(r => selectedRestRooms.Contains(r));
		
		// Reset the roomTypeArray (used for determine tile types and doorways)
		roomType = new int[roomTypeSize.x, roomTypeSize.y];

		// Fill In Main Rooms, Corridor Rooms and Corridor
		FillInRooms(mainRooms,100);
		FillInRooms(selectedRestRooms,2);
		FillInCorridorLoop(delaunayCartesianPathsDictionary,3);

		// Finally just create all tiles
		GenerateAllTilesFromRoomTypeArray();

		// TODO
		// PathWays working but are just angled straight lines between rooms
		// Better to use A* alg to find shortest path with weights?
		// Preplaces Unused Rooms should be lower cost to move through

		// Find Leaf Rooms (All Rooms with One Exit)
		var leafRooms = delaunayCartesianPathsDictionary.Where(r => r.Value.Count==1).ToDictionary(r => r.Key, r => r.Value);

		// Calculate Player Start Position and Portal Room Position		
		int index = Random.Range(0, leafRooms.Count);
		Point testStartRoom = leafRooms.Keys.ElementAt(index);
		Point endRoom = RoomMaker.FurthestRoomFrom(leafRooms, testStartRoom);
		Point startRoom = RoomMaker.FurthestRoomFrom(leafRooms, endRoom);

		// Place End Portal
		Vector2Int endRoomCenter = endRoom.ToVector2Int();
		SetPortal(endRoomCenter);

		//Place Player
		Vector2Int startRoomCenter = startRoom.ToVector2Int();
		SetPlayerAtStart(startRoomCenter);


		// Generate Ground Terrain
		if (terrainGenerator == null) terrainGenerator = FindObjectOfType<TerrainGenerator>();
		terrainGenerator.GenerateTerrain(roomType, roomTypeStart);
				
		// Add Random SpawnPoints
		List<Vector2Int> spawnPoints = GetSpawnPoints(100,startRoomCenter);
		List<GameObject> spawnPointsAsGameObjects = PlaceSpawnPoints(spawnPoints, TileHolder.transform);

		// Stop Method Execution Timer
		double timeTaken = EditorApplication.timeSinceStartup - time;

		// Show Method Complete Time
		Debug.Log("Time taken: "+timeTaken);
	}

	private void SetPortal(Vector2Int pos)
	{
		Debug.Log("Create Portal at: "+pos);
		//Portal
		GameObject portal = Instantiate(specialPrefabs[0], TileHolder.transform);
		portal.transform.position = new Vector3(pos.x*Tilesize,0,pos.y*Tilesize);
	}

	private List<GameObject> PlaceSpawnPoints(List<Vector2Int> positions, Transform parent)
	{
		List<GameObject> spawnPoints = new List<GameObject>();
		foreach (var position in positions)
		{
			GameObject newEnemySpawnPoint = Instantiate(enemySpawnPointPrefab);
			Vector3 placeAt = new Vector3(position.x*Tilesize, newEnemySpawnPoint.transform.position.y, position.y* Tilesize);
			newEnemySpawnPoint.transform.position = placeAt;
			newEnemySpawnPoint.transform.parent = parent;
			spawnPoints.Add(newEnemySpawnPoint);
		}
		return spawnPoints;
	}
	
	private List<Vector2Int> GetSpawnPoints(int v, Vector2Int start)
	{
		List<Vector2Int> allLegalPoints = new List<Vector2Int>();
		List<Vector2Int> selectedPoints = new List<Vector2Int>();

		for (int i = 0; i < roomType.GetLength(0); i++)
		{
			for (int j = 0; j < roomType.GetLength(1); j++)
			{
				if (roomType[i, j] == 0) continue;
				Vector2Int TilePos = new Vector2Int(roomTypeStart.x + i, roomTypeStart.y + j);

				//Check if to close to Player start position
				if (Vector2.Distance(start, TilePos) < EnemyDistanceFromPlayerSpawn) continue;

				allLegalPoints.Add(TilePos);				
			}
		}

		if (v > allLegalPoints.Count) Debug.LogError("Requesting more points than available");

		for (int i = 0; i < v; i++)
		{
			int index = Random.Range(0, allLegalPoints.Count);
			selectedPoints.Add(allLegalPoints[index]);
			allLegalPoints.RemoveAt(index);
		}
		return selectedPoints;
	}

	private void GenerateAllTilesFromRoomTypeArray()
	{
		// Array to Use roomType
		for (int i = 0; i < roomType.GetLength(0); i++)
		{
			for (int j = 0; j < roomType.GetLength(1); j++)
			{
				if (roomType[i, j] == 0) continue;

				Vector2Int TilePos = new Vector2Int(roomTypeStart.x + i, roomTypeStart.y + j);
				//Create Tile
				GameObject tile;
				if (roomType[i, j] >= 100 || !VisualizeCorridors) tile = GenerateFloorTileAt(TilePos);
				else tile = GenerateForcedFloorTileAt(TilePos, roomType[i, j]+6);
								
				//Create Wall
				GenerateWallIfNeeded(i,j, tile);
			}
		}
	}

	private void GenerateWallIfNeeded(int i, int j, GameObject tile)
	{
		// Check for Door
		List<Direction> doors = IsDoorOpening(new Vector2Int(roomTypeStart.x + i, roomTypeStart.y + j));

		// Qhickfix to clean up doorways from clutter
		if (doors.Count > 0) RemoveAllExtra(tile);

		int currentType = roomType[i, j];
		int walltype = currentType < 100 ? 1 : 0;
		if (currentType >= 100)
		{
			if(Random.Range(0, 10) <= 1)
			{
				walltype = 2+Random.Range(0, wallTilesPrefab.Count-3);
			}
		}

		int nextType = roomType[i, j + 1];
		if (nextType != currentType && (nextType== 0 || currentType >= 100) && !doors.Contains(Direction.up)) CreateWallAt(Direction.up, tile, walltype);
		nextType = roomType[i + 1, j];
		if (nextType != currentType && (nextType == 0 || currentType >= 100) && !doors.Contains(Direction.right)) CreateWallAt(Direction.right, tile, walltype);
		nextType = roomType[i, j - 1];
		if (nextType != currentType && (nextType == 0 || currentType >= 100) && !doors.Contains(Direction.down)) CreateWallAt(Direction.down, tile, walltype);
		nextType = roomType[i - 1, j];
		if (nextType != currentType && (nextType == 0 || currentType >= 100) && !doors.Contains(Direction.left)) CreateWallAt(Direction.left, tile, walltype);
	}

	private void RemoveAllExtra(GameObject tile)
	{
		Transform[] children = tile.transform.GetComponentsInChildren<Transform>();
		if(children.Length <= 1) return;

		for (int i = children.Length-1; i >= 0; i--)
		{
			if (children[i] == tile.transform) continue;
			if(children[i] != tile.transform) DestroyImmediate(children[i].gameObject);
		}
	}

	private List<Direction> IsDoorOpening(Vector2Int p)
	{
		List<Direction> doorsDirections = new List<Direction>();

		foreach (DoorInfo doorOpening in doorOpenings)		
		{
			if (doorOpening.pos == p) 
			doorsDirections.Add(doorOpening.dir);
		}
		return doorsDirections;
	}

	private void FillInCorridorLoop(Dictionary<Point, List<Point>> delaunayPathwayDictionary, int v,int sideSteps=1)
	{
		// Reset Door List
		doorOpenings = new List<DoorInfo>();

		foreach (var path in delaunayPathwayDictionary)
		{
			foreach (var startPoint in delaunayPathwayDictionary.Keys)
			{
				foreach (var endPoint in delaunayPathwayDictionary[startPoint])
				{
					FillInCorridor(startPoint,endPoint,sideSteps);
				}
			}
		}
	}

	private void FillInCorridor(Point startPoint, Point endPoint, int sideSteps)
	{
		Vector2Int startID = Vector2Int.RoundToInt(startPoint.ToVector2());
		Vector2Int endID = Vector2Int.RoundToInt(endPoint.ToVector2());
		int steps = Math.Abs(startID.x - endID.x) + Math.Abs(startID.y - endID.y);
		Vector2Int step = Vector2Int.RoundToInt(((Vector2)endID - (Vector2)startID).normalized);
		Vector2Int currentID = startID;

		for (int i = 0; i < steps; i++)
		{
			if (roomType[-roomTypeStart.x + currentID.x, -roomTypeStart.y + currentID.y] == 0 || roomType[-roomTypeStart.x + currentID.x, -roomTypeStart.y + currentID.y] == 3)
			{
				roomType[-roomTypeStart.x + currentID.x, -roomTypeStart.y + currentID.y] = 3;
				// Check neighboring positions within sideSteps distance and set them to 3
				for (int x = -roomTypeStart.x + currentID.x - sideSteps; x <= -roomTypeStart.x + currentID.x + sideSteps; x++)
				{
					for (int y = -roomTypeStart.y + currentID.y - sideSteps; y <= -roomTypeStart.y + currentID.y + sideSteps; y++)
					{
						// Make sure the position is within the bounds of the array
						if (x >= 0 && x < roomType.GetLength(0) && y >= 0 && y < roomType.GetLength(1))
						{
							if (roomType[x, y] == 0)
							{
								// Check if the position is within sideSteps distance
								Vector2Int neighborID = new Vector2Int(x, y);
								roomType[x, y] = 3;
							}
						}
					}
				}
			}
			int lastType = roomType[-roomTypeStart.x + currentID.x, -roomTypeStart.y + currentID.y];
			Vector2Int lastID = currentID;
			currentID += step;
			int thisType = roomType[-roomTypeStart.x + currentID.x, -roomTypeStart.y + currentID.y];

			//if (((thisType >= 100 && thisType <= 199)|| (lastType >= 100 && lastType <= 199)) && lastType != 0 && thisType != lastType)
			if (lastType != 0 && thisType != lastType)
			{
				doorOpenings.Add(new DoorInfo(currentID, -step));
				doorOpenings.Add(new DoorInfo(lastID, step));
			}
		}
	}

	private void FillInRooms(List<Room> rooms, int type)
	{
		int addedRoomId = 0;
		foreach (Room room in rooms)
		{
			for (int i = 0; i < room.size.x; i++)
			{
				for (int j = 0; j < room.size.y; j++)
				{
					roomType[-roomTypeStart.x + room.pos.x+i, -roomTypeStart.y + room.pos.y+j] = type+addedRoomId;
				}
			}
			if(type == 100) addedRoomId += 1;
		}
	}

	private void OnDrawGizmos()
	{
		if(!ShowLines) return;
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Vector3.up, Vector3.right);
		
		if (delaunayCartesianPathsDictionary != null)
		{
			int lines = 0;
			Gizmos.color = Color.red;

			foreach (Point point in delaunayCartesianPathsDictionary.Keys)
			{
				foreach (var point2 in delaunayCartesianPathsDictionary[point])
				{
					Vector3 startPoint = new Vector3((float)point.X, 0.2f, (float)point.Y);
					//PathPoint closestNeighbor = pathPoint.closest;
					Vector3 endPoint = new Vector3((float)point2.X, 0.2f, (float)point2.Y);
					//Debug.Log("Drawing line "+startPoint+" to "+endPoint);
					Gizmos.DrawLine(startPoint*2, endPoint*2);
					lines++;
				}

			}
		}
		
		if (ShowSize)
		{
			Gizmos.color = Color.cyan;

			Vector3 pointCorrection = new Vector3(1,0,1);
			Vector3 pointCorrection2 = new Vector3(1.1f,0,1.1f);
			Vector3 startPoint = new Vector3(roomTypeStart.x, 0.2f, roomTypeStart.y);
			Vector3 endPoint = new Vector3(roomTypeStart.x+roomTypeSize.x, 0.2f, roomTypeStart.y);
			Vector3 endPoint2 = new Vector3(roomTypeStart.x, 0.2f, roomTypeStart.y+roomTypeSize.y);
			Vector3 endPoint3 = new Vector3(roomTypeStart.x+roomTypeSize.x, 0.2f, roomTypeStart.y+roomTypeSize.y);
			Gizmos.DrawLine(startPoint*2- pointCorrection, endPoint*2- pointCorrection);		
			Gizmos.DrawLine(startPoint*2 - pointCorrection, endPoint2*2 - pointCorrection);					
			Gizmos.DrawLine(endPoint3*2 - pointCorrection, endPoint*2 - pointCorrection);					
			Gizmos.DrawLine(endPoint3*2 - pointCorrection, endPoint2*2 - pointCorrection);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(startPoint * 2 - pointCorrection2, endPoint * 2 - pointCorrection2);
			Gizmos.DrawLine(startPoint * 2 - pointCorrection2, endPoint2 * 2 - pointCorrection2);
			Gizmos.DrawLine(endPoint3 * 2 - pointCorrection2, endPoint * 2 - pointCorrection2);
			Gizmos.DrawLine(endPoint3 * 2 - pointCorrection2, endPoint2 * 2 - pointCorrection2);
		}
		
		// RoomArray Show Size
		if (delaunayDictionary != null)
		{
			Gizmos.color = Color.grey;

			foreach (Point point in delaunayDictionary.Keys)
			{
				foreach (var point2 in delaunayDictionary[point])
				{
					Vector3 startPoint = new Vector3((float)point.X, 0.2f, (float)point.Y);
					//PathPoint closestNeighbor = pathPoint.closest;
					Vector3 endPoint = new Vector3((float)point2.X, 0.2f, (float)point2.Y);
					//Debug.Log("Drawing line "+startPoint+" to "+endPoint);
					Gizmos.DrawLine(startPoint*2, endPoint*2);		
				}

			}
		}

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
}
