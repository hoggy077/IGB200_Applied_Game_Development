using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour, iSenderObject
{
    public bool currentState_ {
        get {
            return currentState;
        }
        set {
            currentState = value;
            UpdateReciever();
        }
    }
    public bool currentState = false;
    public List<iRecieverObject> targetObjects_ { get => targetObjects; set => targetObjects = value; }
    private List<iRecieverObject> targetObjects = new List<iRecieverObject>();
    public void UpdateReciever() {
        foreach (iRecieverObject target in targetObjects_) {
            target.CheckSenders(this);
        }
    }

    public void Start() {
        Destroy(this.GetComponent<SpriteRenderer>());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out PlayerEntity _) && collision.isTrigger && !currentState_) {
            currentState_ = true;
            TriggerDialogue();
            Destroy(this.GetComponent<Collider2D>());
        }
    }


    public Dialogue dialogue;

    public void TriggerDialogue() {
        if(dialogue.sentences.Length > 0) {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }

    public void ResetSender() {
        currentState_ = false;
    }
}
