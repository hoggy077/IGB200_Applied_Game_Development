using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScroll : MonoBehaviour
{
    RectTransform rtrans;
    public float rate = 0.1f;
    private void Start()
    {
        rtrans = gameObject.GetComponent<RectTransform>();
    }
    public void ScrollUP()
    {
        if (rtrans.offsetMin.y < 0)
        {
            transform.position += Vector3.up * rate;
        }
    }

    public void ScrollDOWN()
    {
        if (rtrans.offsetMax.y > 0)
        {
            transform.position += -Vector3.up * rate;
        }
    }
}
