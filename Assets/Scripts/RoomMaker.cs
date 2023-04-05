using DelaunayVoronoi;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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
			Random.insideUnitCircle);
	}

	internal static bool Overlap(Room r1, Room r2)
	{
		bool overlapX = r1.pos.x < r2.pos.x+r2.size.x && r1.pos.x+r1.size.x > r2.pos.x;
		bool overlapY = r1.pos.y < r2.pos.y+r2.size.y && r1.pos.y+r1.size.y > r2.pos.y;
		return overlapX && overlapY;		
	}
	
	internal static void MoveOverlap(List<Room> rooms)
	{
		foreach (var room in rooms)
		{
			bool overlap = false;
			room.overlapping.Clear();
			foreach (var roomToCheck in room.closest)
			{
				if (Overlap(room, roomToCheck))
				{
					room.overlapping.Add(roomToCheck);
					overlap = true;
					break;
				}
			}
			if(overlap) MoveAwayFromOverlaps(room);
		}
	}

	private static void MoveAwayFromOverlaps(Room room)
	{
		if (room.overlapping.Count == 0) Debug.LogError("Trying to move away from rooms when not overlapping.");
		Vector2 moveDirection = Vector2.zero;
		foreach (Room otherRoom in room.overlapping)
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

	internal static bool OverlapAny(Room room, List<Room> rooms)
	{
		foreach (var roomToCheck in rooms)
		{
			if(Overlap(room,roomToCheck))return true;			
		}
		return false;
	}

	internal static List<Room> GenerateRandomDungeon(int amt, float hallwayRoomsRatio)
	{
		List<Room> rooms = new List<Room>();
		int totalMovementsNeeded = 0;
		int maxIterations = 100;
		int iteration;

		int totalRooms = Mathf.RoundToInt(amt * hallwayRoomsRatio);

		// Generate X Rooms at Distance X from center at random
		for (int i = 0; i < totalRooms; i++)
		{
			iteration = 0;
			Room newRoom = RoomMaker.GenerateRandomRoom();
			while (RoomMaker.OverlapAny(newRoom, rooms) && iteration < maxIterations)
			{
				newRoom.Move();
				iteration++;
				totalMovementsNeeded++;
			}
			if (iteration == maxIterations)
			{
				Debug.LogWarning("Move new room iteration reached MAX, stopped moving overlapping room. Room at: " + newRoom.tempPos);

			}
			//Lock Room and Add to Rooms
			rooms.Add(newRoom);
		}
		Debug.Log("Creation of Dispersion Dungeon needed " + totalMovementsNeeded + " movements to complete.");

		return rooms;
	}

	internal static List<Vector2> GetCentersAsFloats(List<Room> mainRooms)
	{
		List<Vector2> centers = new List<Vector2>();

		foreach (Room room in mainRooms)
		{
			centers.Add(room.GetFloatCenter());
		}
		return centers;

	}
	internal static List<Vector2Int> GetCentersAsInts(List<Room> mainRooms)
	{
		List<Vector2Int> centers = new List<Vector2Int>();

		foreach (Room room in mainRooms)
		{
			centers.Add(room.GetCenter());
		}
		return centers;

	}

	internal static List<Room> SelectRooms(List<Room> restRooms, Dictionary<Point, List<Point>> delaunayPathwayDictionary)
	{
		List<Room> selectedRooms = new List<Room>();
		int countTrue = 0;
		int count = 0;

		foreach (var point in delaunayPathwayDictionary.Keys)
		{
			foreach (var destination in delaunayPathwayDictionary[point])
			{
				foreach(Room room in restRooms)
				{
					if (selectedRooms.Contains(room)) continue;

					if (room.CheckOverlap(point, destination))
					{
						selectedRooms.Add(room);
						countTrue++;
					}
					else count++;

				}
			}
		}
		Debug.Log("Count of Overlaps: "+countTrue+" Not Overlap:"+count);
		return selectedRooms;
	}
}
