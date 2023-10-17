using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomData))]
public class RoomEditor : Editor
{
    public override void OnInspectorGUI() {
        RoomData myScript = (RoomData)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Sort All Rooms")) {
            RoomData[] AllRooms = FindObjectsOfType<RoomData>();
            foreach(RoomData room in AllRooms) {
                room.GetComponent<Collider2D>().enabled = true;
                room.SortRoom();
                room.GetComponent<Collider2D>().enabled = false;
            }
        }
        if (GUILayout.Button("Unsort All Rooms")) {
            RoomData[] AllRooms = FindObjectsOfType<RoomData>();
            foreach (RoomData room in AllRooms) {
                room.GetComponent<Collider2D>().enabled = true;
                room.UnSortRoom();
                room.GetComponent<Collider2D>().enabled = false;
            }
        }
        if (GUILayout.Button("Load Room")) {
            myScript.Load();
        }
        if (GUILayout.Button("Unload Room")) {
            myScript.Unload();
        }
    }
}
