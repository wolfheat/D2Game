using UnityEngine;

public class ShowDungeonLoadScreen : MonoBehaviour
{
    [SerializeField] LoadDungeon loadDungeonScreen;

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Load Dungeon");
            loadDungeonScreen.OpenMenu();
        }        
    }
}
