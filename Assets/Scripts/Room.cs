using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public Vector2Int size;
	public Vector2Int pos;
	public Vector2 tempPos;
	public List<Room> closest = new List<Room>();
	public List<Room> overlapping = new List<Room>();

	public Room(Vector2Int s, Vector2 p)
	{
		size = s;
		tempPos = p;
		pos = Vector2Int.RoundToInt(tempPos);
	}

	internal Vector2Int GetCenter()
	{
		Vector2 center = new Vector2(pos.x + (size.x / 2), pos.y + (size.y / 2));
		return Vector2Int.RoundToInt(center);
	}

	internal void Move()
	{
		tempPos += tempPos.normalized;
		pos = Vector2Int.RoundToInt(tempPos);
	}
}