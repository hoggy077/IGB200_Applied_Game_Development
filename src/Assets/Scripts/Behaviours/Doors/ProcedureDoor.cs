using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;

public class ProcedureDoor : AbstractLockedDoor, iRecieverObject
{
    private bool[] InputBools;
    private iSenderObject[] InputSenders;
    private iSenderObject[] SolutionSenders;
    // Start is called before the first frame update
    void Start() {
        isInvulnerable = true;
        if (switchGameObjects.Length > 0) {
            switchObjects_ = GetSwitches();
            foreach (iSenderObject iSender in switchObjects_) {
                iSender.targetObjects_.Add(this);
            }
        }
        isSolved = new bool[switchGameObjects.Length];
        InputBools = new bool[switchGameObjects.Length];
        InputSenders = new iSenderObject[switchGameObjects.Length];
        SolutionSenders = new iSenderObject[switchGameObjects.Length];

        for (int i = 0; i < switchGameObjects.Length; i++) {
            SolutionSenders[i] = switchGameObjects[i].GetComponent<iSenderObject>();
        }
    }
    public override void CheckSenders(iSenderObject iSender) {
        UpdateInput(iSender);
        UpdateStates(iSender);
        ResetSwitches();
        if (!isSolved.Contains(false)) {
            currentState_ = true;
        }
    }

    private void ResetSwitches() {
        for (int i = 0; i < switchGameObjects.Length; i++) {
            if (!isSolved[i] && InputSenders[i] != null) {
                InputSenders[i].ResetSender();
            }
        }
    }

    bool[] isSolved;
    public void UpdateInput(iSenderObject iSender) {
        for (int i = 0; i < switchGameObjects.Length; i++) {
            if (InputSenders[i] == iSender) {
                return;
            }
            if (!isSolved[i]) {
                InputBools[i] = iSender.currentState_;
                InputSenders[i] = iSender;
                return;
            }
        }
    }
    public void UpdateStates(iSenderObject iSender) {
        for (int i = 0; i < switchGameObjects.Length; i++) {
            bool SameState = InputBools[i];
            bool SameObject = InputSenders[i] == SolutionSenders[i];
            isSolved[i] = SameState && SameObject;
            if (SolutionSenders[i] == iSender) {
                return;
            }         
        }
    }
    protected new iSenderObject[] GetSwitches() {
        iSenderObject[] returnValue = new iSenderObject[switchGameObjects.Length];
        for (int i = 0; i < switchGameObjects.Length; i++) {
            if(switchGameObjects[i].TryGetComponent(out iSenderObject si))
            returnValue[i] = si;
        }
        return returnValue;
    }
    public override void ValidateFunction() {
        isInvulnerable = true;
        UpdateSprite();
        InitExitDoor(); 
    }
}

[System.Serializable]
public struct SwitchObjects
{
    public GameObject SwitchObject;
    public bool DesiredState;
}

public struct SwitchOrders
{
    public iSenderObject SwitchObject;
    public bool DesiredState;
    public SwitchOrders(iSenderObject iSenderObject, bool State) {
        this.SwitchObject = iSenderObject;
        this.DesiredState = State;
    }
}
