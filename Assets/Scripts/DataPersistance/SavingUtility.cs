using UnityEngine;

public class SavingUtility : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        CharacterStats.SaveToFile();
    }
    private void Start()
    {
        CharacterStats.LoadFromFile();
    }
}
