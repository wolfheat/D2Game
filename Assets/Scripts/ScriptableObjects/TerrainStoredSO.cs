using UnityEngine;

[CreateAssetMenu(menuName = "StoredTerrain")]
public class TerrainStoredSO : ScriptableObject
{
	public float[,] terrainDataArray;
	public bool test = false;
}
