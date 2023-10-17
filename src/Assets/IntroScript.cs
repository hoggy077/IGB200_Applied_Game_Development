using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public Dialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        DialogueManager.Instance.PAK = true;
    }
}
