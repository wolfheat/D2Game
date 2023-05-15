using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyboxController : MonoBehaviour
{
    [SerializeField] private Material skyMaterial;

    [SerializeField] private Color color;

	private void Start()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TownScene"));
        if (skyMaterial!=null) RenderSettings.skybox = skyMaterial;
        RenderSettings.ambientSkyColor = color;
        //RenderSettings.ambientLight = color;

    }
}
