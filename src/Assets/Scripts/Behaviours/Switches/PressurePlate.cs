using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, iSenderObject
{
    public GameObject buttonPressSound;
    public bool PressOnce = true;
    public Sprite[] sprites;
    private bool currentState;
    public bool desiredState = true;
    public bool currentStateUpdate_ {get => currentState; set { currentState_ = value; UpdateReciever(); } }
    public bool currentState_ { get { return currentState; } set { currentState = value; UpdateSprite(); }
    }
    private List<iRecieverObject> targetObjects = new List<iRecieverObject>();
    private bool resetState;

    public List<iRecieverObject> targetObjects_ { get => targetObjects; set => targetObjects = value; }

    public void UpdateReciever() {
        foreach (iRecieverObject target in targetObjects_) {
            target.CheckSenders(this);
        }
    }
    // Start is called before the first frame update
    private void Start() {
        UpdateSprite();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (PressOnce && collision.transform.CompareTag("Player")) {
            Instantiate(buttonPressSound); 
            DebugBox.Instance.inputs.Add("Object.isActive(switch);");
            currentStateUpdate_ = true;
        }
        UpdateSprite();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!PressOnce && collision.transform.TryGetComponent<iPhysicsInterface>(out _) && !collision.isTrigger) {
            currentStateUpdate_ = true;
            UpdateSprite();            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!PressOnce && collision.transform.TryGetComponent<iPhysicsInterface>(out _) && !collision.isTrigger) {
            currentStateUpdate_ = false;
            UpdateSprite();
        }
        if(resetState && PressOnce && collision.transform.CompareTag("Player")) {
            currentState_ = false;
            resetState = false;
        }
    }

    private void UpdateSprite() {
        if (currentStateUpdate_) {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        } else {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
    }

    public void ResetSender() {
        resetState = true;
    }
}
