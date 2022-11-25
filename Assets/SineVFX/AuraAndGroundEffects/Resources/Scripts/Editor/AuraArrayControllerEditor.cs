using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AuraArrayController))]
public class AuraArrayControllerEditor : Editor
{

    private GUIStyle style;

    public override void OnInspectorGUI()
    {
        style = new GUIStyle(GUI.skin.textArea);
        //style.fontStyle = FontStyle.Bold;
        //style.border = GUI.skin.label.border;
        //style.normal = GUI.skin.label.normal;
        style.margin.top = 12;
        style.margin.bottom = 6;

        //DrawDefaultInspector();
        base.OnInspectorGUI();

        GUILayout.TextArea("In order to edit your material properly, please follow the two steps below. First, assign material to the projector. Edit your material as you want, and then update all material instances in the Scene.", style);

        AuraArrayController myScript = (AuraArrayController)target;
        if (GUILayout.Button("Step 1: Assign Source Material"))
        {
            myScript.AssignSourceMaterial();
        }

        if (GUILayout.Button("Step 2: Update all Material Instances"))
        {
            myScript.UpdateAllMaterialInstances();
        }
    }
}