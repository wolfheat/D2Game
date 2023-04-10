using UnityEngine;

class DoorInfo
{
	public Vector2Int pos;
	public Direction dir;

	public DoorInfo(Vector2Int p, Vector2Int d)
	{
		dir = VectorToDir(d);
		pos = p;
	}

	private Direction VectorToDir(Vector2Int d)
	{
		if (d.y > 0) return Direction.up;
		if (d.x > 0) return Direction.right;
		if (d.y < 0) return Direction.down;

		return Direction.left;
	}
}
