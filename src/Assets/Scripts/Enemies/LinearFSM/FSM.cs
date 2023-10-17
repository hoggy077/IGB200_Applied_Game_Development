using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FSM_Struct
{
    public Dictionary<int, FSM_States> States;

    private FSM_States ActiveState;
    private FSM_States PreviousState;
    private Coroutine ActiveState_co;

    public FSM_States getState() => ActiveState;

    public void ChangeState(int StateID)
    {
        ActiveState = States[StateID];
    }
    public int UpdateState(FSM_Datapass data)
    {
        KeyValuePair<int, FSM_States> res = States.First((keypair) => { return keypair.Value.StatePredicate(data); });
        if (res.Equals(default))
            return -1;

        ActiveState = res.Value;
        return res.Key;
    }
    public void RunState(MonoBehaviour mono)
    {
        if (PreviousState != ActiveState)
        {
            PreviousState = ActiveState;
            //Debug.Log($"{(PreviousState == null ? "null" : PreviousState.Description)} - {ActiveState.Description}");
            if (ActiveState_co != null)
                mono.StopCoroutine(ActiveState_co);
            //Debug.Log(ActiveState.StateLogic);
            ActiveState_co = mono.StartCoroutine(ActiveState.StateLogic);
            
        }
    }
}

public class FSM_States
{
    public Predicate<FSM_Datapass> StatePredicate;
    public IEnumerator StateLogic;
    public string Description;
    public Action Callback;
}

public struct FSM_Datapass
{
    public GameObject Caller;
    public GameObject Target;
    public Vector3 TargetPos;
    public bool UsePos;
    public float Distance { get 
        {
            if (UsePos)
                return Vector3.Distance(Caller.transform.position, TargetPos);
            if (Target != null) 
                return Vector3.Distance(Caller.transform.position, Target.transform.position); 
            return Mathf.Infinity; 
        } 
    }
}