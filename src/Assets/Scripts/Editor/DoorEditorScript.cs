using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EnumsAndDictionaries;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(AbstractDoor), true)]
public class DoorEditorScript : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
    }
    void OnSceneGUI() {
        AbstractDoor abstractDoor = target as AbstractDoor;        
        if (abstractDoor.ExitDoor != null) {
            Handles.color = Color.yellow;
            Handles.DrawLine(abstractDoor.transform.position, abstractDoor.ExitDoor.transform.position);
        }
        if (abstractDoor.TryGetComponent(out iRecieverObject iReciever)) {
            foreach(GameObject iSender in iReciever.switchGameObjects_) {
                Handles.color = Color.blue;
                Handles.DrawLine(abstractDoor.transform.position, iSender.transform.position);
            }
        }

        abstractDoor.ValidateFunction();
    }

    //private void OnValidate() {
    //    AbstractDoor abstractDoor = target as AbstractDoor;
    //    Debug.Log(abstractDoor.transform.name + " Validate Called");
    //    abstractDoor.ValidateFunction();
    //}
}
