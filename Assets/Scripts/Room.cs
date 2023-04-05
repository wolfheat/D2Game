using DelaunayVoronoi;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public Vector2Int size;
	public Vector2Int pos;
	private Point[] corners;
	public Vector2 tempPos;
	public List<Room> closest = new List<Room>();
	public List<Room> overlapping = new List<Room>();

	internal bool CheckOverlap(Point point, Point destination)
	{
		// Check if the line overlaps with any of the four edges of the room
		for (int i = 0; i < 4; i++)
		{
			Point p1 = corners[i];
			Point p2 = corners[(i + 1) % 4];

			// Calculate the normal of the edge
			Vector2 normal = new Vector2((float)(p2.Y - p1.Y), (float)(p1.X - p2.X)).normalized;

			// Calculate the projection of the line onto the normal
			float proj1 = Vector2.Dot(normal, new Vector2((float)(point.X - (float)p1.X), (float)(point.Y - (float)p1.Y)));
			float proj2 = Vector2.Dot(normal, new Vector2((float)(destination.X - (float)p1.X), (float)(destination.Y - (float)p1.Y)));

			Debug.Log("Room corner 1: " + p1);
			Debug.Log("Room corner 2: " + p2);
			Debug.Log("Line endpoint 1: " + point);
			Debug.Log("Line endpoint 2: " + destination);
			Debug.Log("Projection 1: " + proj1);
			Debug.Log("Projection 2: " + proj2);


			// Check if the line overlaps with the edge
			if (proj1 * proj2 >= 0)
			{
				return false;
			}
		}

		return true;
	}

	public Room(Vector2Int s, Vector2 p)
	{
		size = s;
		tempPos = p;
		pos = Vector2Int.RoundToInt(tempPos);
		corners = new Point[]
		{
			new Point(pos.x, pos.y),
			new Point(pos.x + size.x-1, pos.y),
			new Point(pos.x + size.x-1, pos.y + size.y-1),
			new Point(pos.x, pos.y + size.y-1),
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
			new Point(pos.x, pos.y),
			new Point(pos.x + size.x-1, pos.y),
			new Point(pos.x + size.x-1, pos.y + size.y-1),
			new Point(pos.x, pos.y + size.y-1),
		};
	}
}