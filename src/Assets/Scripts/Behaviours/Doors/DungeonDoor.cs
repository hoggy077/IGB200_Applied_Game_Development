using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsAndDictionaries;



public class DungeonDoor : AbstractDoor
{
    public int id;
    public bool isOpen;

    void Start()
    {
        IsOpen = true;
    }

    public override void ValidateFunction()
    {
        IsOpen = true;
        UpdateSprite();
        InitExitDoor();
    }

    public void Update()
    {
        if (isOpen == true)
        {
            if (id == 1) { transform.position = new Vector3 (14, 78.7f, 1); }
            if (id == 2) { transform.position = new Vector3(16.2f, 78.7f, 1); }
            if (id == 3) { transform.position = new Vector3(18.8f, 78.7f, 1); }
            if (id == 4) { transform.position = new Vector3(21, 78.7f, 1); }

            isOpen = false;
        }
    }

    
}
