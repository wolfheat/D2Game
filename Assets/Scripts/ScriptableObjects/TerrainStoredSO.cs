using UnityEngine;

[CreateAssetMenu(menuName = "StoredTerrain")]
[System.Serializable]
public class TerrainStoredSO : ScriptableObject
{
	public float[,] terrainDataArray;
	public string arrayAsString;
	public string testString;
	public bool test = false;
	public int amount = 0;


	public void SaveTerrain(float[,] array)
	{
		// Convert the float array to a JSON string and pretty-print it
		arrayAsString = JsonUtility.ToJson(array, true);

		testString = "Just Test Text";
	}

	public float[,] LoadTerrain()
	{
		// Convert the JSON string back to a float array
		float[,] terrainDataArray = JsonUtility.FromJson<float[,]>(arrayAsString);

		return terrainDataArray;
	}
	public string ReadTestText()
	{
		return testString;
	}

}
