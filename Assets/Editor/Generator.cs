
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generator : EditorWindow
{
    private string objectName;
    private int objectValue;

    [MenuItem("Window/Generate Scriptable Object")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Generator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Scriptable Object Generator", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Name:", objectName);
        objectValue = EditorGUILayout.IntField("Value:", objectValue);

        if (GUILayout.Button("Create Scriptable Object"))
        {
            CreateScriptableObject();
        }
    }

    private void CreateScriptableObject()
    {
        PlayerSettingsSO newObject = ScriptableObject.CreateInstance<PlayerSettingsSO>();
        Vector3[] positions = new Vector3[2];
        positions[0] =new Vector3(10,0,20);
        positions[1] =new Vector3(50,0,30);
        newObject.TownSpotsPositions = positions;

        string path = EditorUtility.SaveFilePanelInProject("Save Scriptable Object", "NewScriptableObject", "asset", "Save Scriptable Object");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(newObject, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Scriptable Object created and saved: " + path);
        }
    }
}
