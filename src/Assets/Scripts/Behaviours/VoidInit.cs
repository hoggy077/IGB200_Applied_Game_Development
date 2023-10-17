using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static EnumsAndDictionaries;
using static SpriteManager;

public class VoidInit : MonoBehaviour
{
    public List<TileAssetArrays> TileAssets;
    private List<GameObject> VoidTiles = new List<GameObject>();
    //private List<TileAsset> TileAsset = new List<TileAsset>();
    private Dictionary<Vector3Int, VoidType> TileDict = new Dictionary<Vector3Int, VoidType>();
    private Dictionary<Sprite, VoidType> TileSpriteDict = new Dictionary<Sprite, VoidType>();
    public Tilemap Tile_map;
    // Start is called before the first frame update
    void Awake()
    {
        if(VoidTiles.Count == 0) {
            GenerateVoid();
        }
    }

    public void GenerateVoid() {
        DeleteVoid();

        TileSpriteDict = new Dictionary<Sprite, VoidType>();

        foreach (TileAssetArrays TAA in TileAssets.ToArray()) {
            foreach (Sprite sprite in TAA.Tiles) {
                if (!TileSpriteDict.ContainsKey(sprite)) {
                    TileSpriteDict.Add(sprite, TAA.voidType);
                }
            }
        }
        for (int x = Tile_map.cellBounds.xMin; x < Tile_map.cellBounds.xMax; x++) {
            for (int y = Tile_map.cellBounds.yMin; y < Tile_map.cellBounds.yMax; y++) {
                Vector3Int TilePosRaw = new Vector3Int(x, y, Mathf.RoundToInt(Tile_map.transform.position.z));
                Vector3 TilePos = Tile_map.CellToWorld(TilePosRaw);
                if (Tile_map.HasTile(TilePosRaw)) {
                    if (TileSpriteDict.ContainsKey(Tile_map.GetSprite(TilePosRaw)) && !TileDict.ContainsKey(TilePosRaw)) {
                        TileDict.Add(TilePosRaw, TileSpriteDict[Tile_map.GetSprite(TilePosRaw)]);
                    }
                }
            }
        }

        foreach (Vector3Int vector3Int in TileDict.Keys.ToArray()) {
            GameObject Tile = CreateVoidObject(TileDict[vector3Int]);
            Tile.transform.position = Tile_map.CellToWorld(vector3Int) + 0.5f * new Vector3(1, 1, 0);
            Tile.transform.parent = this.transform;
            VoidTiles.Add(Tile);
        }
    }

    public void DeleteVoid() {
        foreach (GameObject gameObject in VoidTiles.ToArray()) {
            VoidTiles.Remove(gameObject);
            DestroyImmediate(gameObject);
        }
        while (transform.childCount > 0){
            DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
        }
        EmptySpaceScript[] AllVoids = FindObjectsOfType<EmptySpaceScript>();
        foreach (EmptySpaceScript gameObject in AllVoids) {
            VoidTiles.Remove(gameObject.gameObject);
            DestroyImmediate(gameObject.gameObject);
        }
        VoidTiles = new List<GameObject>();
    }

    private GameObject CreateVoidObject(VoidType voidType) {
        GameObject GO = new GameObject(voidType.ToString() + "Tile");
        GO.AddComponent<EmptySpaceScript>().VoidType_ = voidType;
        GO.AddComponent<BoxCollider2D>();
        return GO;
    }
}

[System.Serializable]
public class TileAssetArrays {
    public Sprite[] Tiles;
    public VoidType voidType;

}
//[System.Serializable]
//public class TileAsset{
//    public Sprite Tile;
//    public VoidType voidType;

//    public TileAsset(Sprite Tile_, VoidType voidType_) {
//        Tile = Tile_;
//        voidType = voidType_;
//    }
//}

