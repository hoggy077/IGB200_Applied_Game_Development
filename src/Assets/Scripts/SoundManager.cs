using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundCollections[] SoundCollections;
    public static Dictionary<string, GameObject> SoundDict;
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    [ExecuteInEditMode]
    private void Awake() {

        StoreVariables();

        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void OnValidate() {
        StoreVariables();
    }

    private void StoreVariables() {
        SoundDict = new Dictionary<string, GameObject>();
        foreach(SoundCollections Collection in SoundCollections) {
            foreach (SoundCollection collection in Collection.soundCollections) {
                if (!SoundDict.ContainsKey(collection.name)) {
                    SoundDict.Add(collection.name, collection.soundPrefab);
                }
            }
        }
    }
}
[System.Serializable]
public struct SoundCollection
{
    public string name;
    public GameObject soundPrefab;
}
[System.Serializable]
public struct SoundCollections
{
    public string name;
    public SoundCollection[] soundCollections;
}
