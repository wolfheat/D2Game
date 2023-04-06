using DelaunayVoronoi;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RoomGameObject: MonoBehaviour
{
	public Room originalRoom;
	public Collider2D col;
	public static List<Room> SelectRooms(List<RoomGameObject> rooms, Dictionary<Point, List<Point>> lines)
	{
		List<Room> selectedRooms = new List<Room>();

		foreach (RoomGameObject room in rooms)
		{
			foreach (Point point in lines.Keys)
			{
				Vector2 start = point.ToVector2();
				foreach (Point target in lines[point])
				{
					Vector2 end = target.ToVector2();
					Ray ray = new Ray(start, end);

					if (room.col.bounds.IntersectRay(ray))
					{
						selectedRooms.Add(room.originalRoom);
						break;
					}
				}
			}
		}
		return selectedRooms;
	}
}
public class Room
{
	public Vector2Int size;
	public Vector2Int pos;
	public Point[] corners;
	public Vector2 tempPos;
	public List<Room> closest = new List<Room>();
	public List<Room> overlapping = new List<Room>();

	
	internal bool CheckOverlap(Point p1, Point p2)
	{
		Point lowLeft = p1;
		Point upRight = p2;

		// make sure P1 is bottom left corner
		if(p1.X > p2.X || p1.Y > p2.Y)
		{
			lowLeft = p2;
			upRight = p1;
		}

		if ( (lowLeft.X < corners[0].X && upRight.X > corners[0].X) || (lowLeft.X < corners[2].X && lowLeft.X > corners[0].X))
			if(lowLeft.Y > corners[0].Y && lowLeft.Y < corners[2].Y) return true;

		if ( (lowLeft.Y < corners[0].Y && upRight.Y > corners[0].Y) || (lowLeft.Y < corners[2].Y && lowLeft.Y > corners[0].Y))
			if(lowLeft.X > corners[0].X && lowLeft.X < corners[2].X) return true;

		return false;
	}

	internal bool OLDCheckOverlap(Point point, Point destination)
	{
		bool result = false;

		Debug.Log("Checking four edges");
		// Check if the line overlaps with any of the four edges of the room
		for (int i = 0; i < 4; i++)
		{
			Debug.Log("Checking edge "+(i+1));
			Point p1 = corners[i];
			Point p2 = corners[(i + 1) % 4];

			// Calculate the normal of the edge
			Vector2 normal = new Vector2((float)(p2.Y - p1.Y), (float)(p1.X - p2.X)).normalized;

			// Calculate the projection of the line onto the normal
			float proj1 = Vector2.Dot(normal, new Vector2((float)(point.X - (float)p1.X), (float)(point.Y - (float)p1.Y)));
			float proj2 = Vector2.Dot(normal, new Vector2((float)(destination.X - (float)p1.X), (float)(destination.Y - (float)p1.Y)));

			Debug.Log("Room edge: " + p1+ "," + p2);
			Debug.Log("Line startpoint: " + point+ " Line endpoint: " + destination);
			Debug.Log("Projections: " + proj1 +","+ proj2);


			// Check if the line overlaps with the edge
			if (proj1 * proj2 < 0)
			{
				Debug.Log("OVERLAPS");
				result = true;
				break;
			}
		}

		return result;
	}

	public Room(Vector2Int s, Vector2 p)
	{
		size = s;
		tempPos = p;
		pos = Vector2Int.RoundToInt(tempPos);
		corners = new Point[]
		{
			new Point(pos.x-0.5f, pos.y-0.5f),
			new Point(pos.x-0.5f + size.x, pos.y-0.5f),
			new Point(pos.x-0.5f + size.x, pos.y-0.5f + size.y),
			new Point(pos.x-0.5f, pos.y-0.5f + size.y),
		};
	}

	internal Vector2Int GetCenter()
	{
		Vector2 center = new Vector2(pos.x + (size.x / 2), pos.y + (size.y / 2));
		return Vector2Int.RoundToInt(center);
	}
	
	internal Vector2 GetFloatCenter()
	{
		Vector2 center = new Vector2(pos.x + (size.x / 2), pos.y + (size.y / 2));
		return center;
	}

	internal void Move()
	{
		tempPos += tempPos.normalized;
		pos = Vector2Int.RoundToInt(tempPos);
		corners = new Point[]
		{
			new Point(pos.x-0.5f, pos.y-0.5f),
			new Point(pos.x-0.5f + size.x, pos.y-0.5f),
			new Point(pos.x-0.5f + size.x, pos.y-0.5f + size.y),
			new Point(pos.x-0.5f, pos.y-0.5f + size.y),
		};
	}
}