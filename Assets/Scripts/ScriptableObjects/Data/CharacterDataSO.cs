using UnityEngine;

[CreateAssetMenu(fileName = "SceneTransferData", menuName = "Game/GameData")]
public class CharacterDataSO : ScriptableObject
{	
    public int health;
    public int energy;
    public int xp;
    public ItemData[] items;
}