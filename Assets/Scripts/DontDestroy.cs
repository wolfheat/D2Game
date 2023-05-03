using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance { get; set; }

	private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
