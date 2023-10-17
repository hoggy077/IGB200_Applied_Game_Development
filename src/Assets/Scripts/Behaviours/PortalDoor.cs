using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDoor : MonoBehaviour, iRecieverObject
{
    public GameObject[] switchGameObjects_ { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public iSenderObject[] switchObjects_ { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool currentState_ { get => currentState; set => currentState = value; }
    private bool currentState;
    public AbstractDoor ExitDoor;

    public void CheckSenders(iSenderObject iSenderObject) {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) {
        if(!collision.isTrigger && collision.transform.TryGetComponent(out PlayerEntity playerEntity)) {

        }
    }
}
