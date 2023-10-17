using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public bool isSolved;
    public int roomId = -1;
    public GameObject solveSound;
    private bool summonSound;

    // Start is called before the first frame update
    void Start()
    {
        isSolved = false;
        summonSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSolved == true && summonSound == true)
        {
            Instantiate(solveSound);
            summonSound = false;
        }
    }
}
