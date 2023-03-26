using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
