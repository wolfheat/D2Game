using UnityEngine;

public class ShowDungeonLoadScreen : MonoBehaviour
{
    [SerializeField] LoadDungeon loadDungeonScreen;

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Load Dungeon");
        loadDungeonScreen.HideMenu(false);
        
    }


}
