using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsAndDictionaries;



public class OpenDoor : AbstractDoor
{
    // Start is called before the first frame update
    void Start() {
        IsOpen = true;
    }

    public override void ValidateFunction() {
        IsOpen = true;
        UpdateSprite();
        InitExitDoor();
    }
}
