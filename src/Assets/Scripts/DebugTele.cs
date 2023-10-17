using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTele : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newpos.z = 0;
            GameObject.FindGameObjectWithTag("Player").transform.position = newpos;
        }
    }
}
