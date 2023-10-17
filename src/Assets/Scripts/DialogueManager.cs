using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogText;
    public Image portrait;
    public AudioClip clip;
    public AudioSource sound;
    #region Singleton Things
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    #endregion
    private Queue<string> sentences;
    public static bool IsOpen;

    private float textSpeed = 0.075f;
    private bool donePrinting = false;
    internal bool PAK = false;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        EndDialogue();
    }

    private void Update() {
        if(IsOpen && Input.GetKeyUp(KeyCode.Space) && donePrinting) {
            DisplayNextSentence();
        } else if (IsOpen && Input.GetKeyUp(KeyCode.Space) && !donePrinting) {
            textSpeed = 0f;
        }
    }

    internal void StartDialogue(Dialogue dialogue) {
        OpenDialogue();
        sentences.Clear();
        if (nameText) {
            nameText.text = dialogue.name;
        }
        if (portrait) {
            portrait.sprite = dialogue.CharacterPortrait;
        }
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    private void DisplayNextSentence() {
        //StopCoroutine("WriteSentence");
        Debug.Log("Display Next Dialogue");
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogText.text = "";
        StartCoroutine(WriteSentence(sentence));
    }
    string waitCmd = "/w=N";
    IEnumerator WriteSentence(string sentence) {
        donePrinting = false;
        textSpeed = 0.075f;
        for(int i = 0; i < sentence.Length; i++) {
            if (sound != null)
                sound.Play();
            if (i - sentence.Length > 0 && sentence.Substring(i, waitCmd.Length).StartsWith("/w=")) {
                string str = sentence.Substring(i, waitCmd.Length).Replace("/w=", "");
                int waitTime = int.Parse(str);
                i += waitCmd.Length;
                dialogText.text += "\n";
                
                yield return new WaitForSecondsRealtime(waitTime);

            } else {
                char Char = sentence.ToCharArray()[i];
                dialogText.text += Char;
                if (sound != null)
                    sound.Play();
                if (Char == ' ') {
                    yield return null;
                } else {
                    yield return new WaitForSecondsRealtime(textSpeed);
                }
            }
            if(sound != null)
                sound.Stop();

        }
        if (PAK) {
            PrintPAK();
        }
        donePrinting = true;
    }

    private void PrintPAK() {
        dialogText.text += "<color=#f77622> PRESS SPACE </color>";
    }

    private void OpenDialogue() {
        Time.timeScale = 0;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("Open Dialogue");
        //GetComponent<Animator>().SetTrigger("Open");
        IsOpen = true;
    }

    private void EndDialogue() {
        if (PAK) {
            MainMenuScript.MAINMENU();
            return;
        }
        Time.timeScale = 1;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Debug.Log("End Dialogue"); 
        Time.timeScale = 1;
        //GetComponent<Animator>().SetTrigger("Close");
        IsOpen = false;
    }
}
