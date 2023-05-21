using UnityEngine;

public class ShowDungeonLoadScreen : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Load Dungeon");
            FindObjectOfType<LoadDungeon>().ShowPanel();
        }        
    }
}
