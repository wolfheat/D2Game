using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCreator),true)]
[InitializeOnLoad]
public class RandomDungeonGeneratorEditor : Editor
{
	LevelCreator generator;
    private int currentViewPoint = 20;
	
	private void Awake()
    {
        generator = (LevelCreator)target;
    }

    public override void OnInspectorGUI()
	{        
        // Draw the default inspector
		DrawDefaultInspector();

		if(GUILayout.Button("Clear"))
        {
            generator.ClearLevel();
		}
        if(GUILayout.Button("Bird View"))
        {
            currentViewPoint += 30;
            if (currentViewPoint > 110) currentViewPoint = 4;
            Vector3 playerPos = FindObjectOfType<PlayerController>().transform.position;

            SceneView.lastActiveSceneView.pivot = new Vector3(playerPos.x,currentViewPoint,playerPos.z);
            SceneView.lastActiveSceneView.rotation = Quaternion.LookRotation(Vector3.down,Vector3.forward);
		}
        if(GUILayout.Button("Create Dungeon With RoomDispersion"))
        {
            generator.CreateRoomDispersionDungeon();
		}
    }
}
