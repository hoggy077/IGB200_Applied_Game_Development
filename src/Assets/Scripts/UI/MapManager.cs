using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpriteManager;

public class MapManager : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject mapScreen;
    // Start is called before the first frame update
    RoomData[] AllRooms;
    private bool isLoaded;

    void Start()
    {
        AllRooms = FindObjectsOfType<RoomData>();
        UnloadMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !DialogueManager.IsOpen) {
            if (isLoaded) {
                UnloadMenu();
            } else {
                LoadMenu();
            }
        }
    }

    void UnloadMenu() {
        Time.timeScale = 1;
        isLoaded = false;
        mapCamera.enabled = isLoaded;
        mapScreen.SetActive(isLoaded);
        foreach (RoomData Room in AllRooms) {
            Room.gameObject.layer = 2;
            Room.fill.SetActive(isLoaded);
            Room.spriteRenderer.enabled = isLoaded;
            if (Room.Icon)  Room.Icon.SetActive(isLoaded);
        }

    }
    void LoadMenu() {
        Time.timeScale = 0;
        isLoaded = true;
        mapCamera.enabled = isLoaded;
        mapScreen.SetActive(isLoaded);
        foreach (RoomData Room in AllRooms) {
            Room.gameObject.layer = 5;
            Room.spriteRenderer.enabled = isLoaded;
            Room.fill.SetActive(isLoaded);
            if (Room.isLoaded_) {
                Room.fill.GetComponent<SpriteRenderer>().color = Color.white;
            } else if (Room.hasVisited) {
                Room.fill.GetComponent<SpriteRenderer>().color = Color.red;
            } else {
                Room.fill.GetComponent<SpriteRenderer>().color = Color.green; 
            }

            if(Room.hasChest && Room.Icon) {
                Room.Icon.SetActive(isLoaded);
                if (Room.hasVisited) {
                    Room.Icon.GetComponent<SpriteRenderer>().sprite = SpriteDict["ChestSprites"][1];
                } else {
                    Room.Icon.GetComponent<SpriteRenderer>().sprite = SpriteDict["ChestSprites"][0];
                }                
            } else if (Room.hasFire && Room.Icon) {
                Room.Icon.SetActive(isLoaded);
                if (Room.bonfire.currentState_) {
                    Room.Icon.GetComponent<SpriteRenderer>().sprite = SpriteDict["BonfireSprites"][1];
                } else {
                    Room.Icon.GetComponent<SpriteRenderer>().sprite = SpriteDict["BonfireSprites"][0];
                }
            } else {

            }
        }
    }
}
