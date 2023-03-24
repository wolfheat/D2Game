using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkPreset_", menuName = "RandomWalkSO")]
public class RandomWalkGeneratorPresetSO : ScriptableObject
{
	public Vector2Int playerStartPosition = new Vector2Int(10, 10);
	public int iterations = 10;
	public int walkLength = 10;
	public bool startRandom = false;
}
