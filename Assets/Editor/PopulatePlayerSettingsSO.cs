using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TownPositionsController), true)]
[InitializeOnLoad]
public class PopulatePlayerSettingsSO : Editor
{

    TownPositionsController townPositionsController;
    
        private void Awake()
        {
             townPositionsController = (TownPositionsController)target;
        }

        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            GUILayout.Space(10);
        if (GUILayout.Button("Update Positions"))
        {
            townPositionsController.Populate();
        }        
    }
}