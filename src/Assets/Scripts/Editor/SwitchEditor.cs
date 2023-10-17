
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwitchObject))]
public class SwitchEditor : Editor
{
    void OnSceneGUI() {
        SwitchObject abstractDoor = target as SwitchObject;
        abstractDoor.ValidateFunction();
    }


}

