using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SpriteManager;

public abstract class AbstractLockedDoor : AbstractDoor, iRecieverObject
{
    public GameObject[] switchGameObjects_ { get => switchGameObjects; set => switchGameObjects = value; }
    public GameObject[] switchGameObjects;
    public iSenderObject[] switchObjects_ { get => this.switchObjects; set => this.switchObjects = value; }
    private iSenderObject[] switchObjects;
    public bool currentState_ { get { return currentState; } set { OpenCloseDoor(value); currentState = value; } }
    private bool currentState;

    protected iSenderObject[] GetSwitches() {
        iSenderObject[] returnValue = new iSenderObject[switchGameObjects.Length];
        for (int i = 0 ; i < switchGameObjects.Length; i++) {
            
            try{
            returnValue[i] = switchGameObjects[i].GetComponent<iSenderObject>();
            }
            catch(Exception) {                
                switchGameObjects[i] = null;
                break;
            }
        }
        return returnValue;
    }

    protected void initSwitches() {
        foreach (iSenderObject iSender in switchObjects_) {
            iSender.targetObjects_.Add(this);
        }
    }

    public abstract void CheckSenders(iSenderObject iSenderObject);
}
