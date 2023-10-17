using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iRecieverObject
{
    public GameObject[] switchGameObjects_ { get; set; }
    public iSenderObject[] switchObjects_ { get; set; }
    bool currentState_ { get; set; }
    public void CheckSenders(iSenderObject iSenderObject);
}

public interface iSenderObject
{
    public List<iRecieverObject> targetObjects_ { get; set; }
    public bool currentState_ { get; set; }
    public void UpdateReciever();
    public void ResetSender();
}