using TMPro;
using UnityEngine;

public class PlayerDontDestroy : MonoBehaviour
{
    public static PlayerDontDestroy Instance { get; set; }

    private void Awake()
    {
        //Singelton
        if (Instance != null)
        {
            Instance.DestroySelf();

        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void DestroySelf()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(gameObject);
    }

}
