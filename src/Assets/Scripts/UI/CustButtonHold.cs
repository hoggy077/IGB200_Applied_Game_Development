using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustButtonHold : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    bool Held = false;
    public UnityEvent FireEvent;

    public void OnPointerDown(PointerEventData eventData) => Held = true;

    public void OnPointerUp(PointerEventData eventData) => Held = false;

    void Update()
    {
        if (Held)
            FireEvent.Invoke();
    }
}
