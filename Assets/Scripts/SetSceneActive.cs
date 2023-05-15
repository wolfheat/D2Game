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
    }
}
