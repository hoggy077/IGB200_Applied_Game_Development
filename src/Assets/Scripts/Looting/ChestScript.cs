using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpriteManager;
using static SoundManager;

public class ChestScript : MonoBehaviour, iSenderObject
{
    public bool currentState_ { get { return currentState; } set { _ = value; } }
    private bool currentState = false;   
    public List<iRecieverObject> targetObjects_ { get => targetObjects; set => targetObjects = value; }
    private List<iRecieverObject> targetObjects = new List<iRecieverObject>();

    public bool debug = false;
    public void UpdateReciever() {
        foreach (iRecieverObject target in targetObjects_) {
            target.CheckSenders(this);
        }
    }
    private void Awake() {
        GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
        GetComponent<Renderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y);
    }

    private void UpdateSprite(bool value) {
        if (!value) {
            GetComponent<SpriteRenderer>().sprite = SpriteDict["ChestSprites"][0];
        } else {
            GetComponent<SpriteRenderer>().sprite = SpriteDict["ChestSprites"][1];
        }
    }

    public string[] UnlockNames;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !currentState){
            if (debug) {
                UnlockNames = UnlockManager.Instance.Registry.AllKeys();
            }

            foreach (string UnlockName in UnlockNames) {
                if (Array.Exists(UnlockManager.Instance.Registry.AllKeys(), (e) => { return e == UnlockName; })) {
                    UnlockManager.Instance.Registry.UnlockItem(UnlockName);
                    DebugBox.Instance.inputs.Add("Spell.unlock(" + UnlockName + ");");
                } else {
                    throw new Exception($"Item by the name {UnlockName} does not exist within the unlock manager");
                }
            }
            Instantiate(SoundDict["ChestOpenSound"]);
            currentState = true; 
            UpdateSprite(true); UpdateReciever();
        }
    }

    public void ResetSender() {
        currentState_ = false;
    }
}
