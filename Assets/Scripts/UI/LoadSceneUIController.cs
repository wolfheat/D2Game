using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneUIController : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("Loading MainScene");
        SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
    }

    public void PlayPressed()
    {
        Debug.Log("Play Pressed");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Debug.Log("Loading TownScene");
        SceneManager.LoadScene("TownScene",LoadSceneMode.Additive);

    }
}
