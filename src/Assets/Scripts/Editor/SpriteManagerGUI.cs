using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SpriteManager))]
public class SpriteManagerGUI : Editor
{
    public override void OnInspectorGUI() {
        SpriteManager _ = (SpriteManager)target;

        if (DrawDefaultInspector()) {

        }
    }
}
