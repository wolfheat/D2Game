using UnityEngine;
using UnityEngine.SceneManagement;
using System;


enum SceneName{TownScene,DungeonSceneA}
public class SetSceneActive : MonoBehaviour
{
    [SerializeField] SceneName sceneToSet;
	private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToSet.ToString()));

        AddMainIfNotActive();
    }

    private void AddMainIfNotActive()
    {
        if (!SceneManager.GetSceneByName("MainScene").IsValid()) SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
    }
}
