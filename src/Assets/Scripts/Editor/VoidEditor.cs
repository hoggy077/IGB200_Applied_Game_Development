using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoidInit))]
public class VoidEditor : Editor
{
    public override void OnInspectorGUI() {
        VoidInit myScript = (VoidInit)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Build Void")) {
            myScript.GenerateVoid();
        }
        if (GUILayout.Button("Destroy Void")) {
            myScript.DeleteVoid();
        }
    }
}
