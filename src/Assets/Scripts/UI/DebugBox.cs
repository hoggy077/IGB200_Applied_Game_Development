using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class DebugBox : MonoBehaviour
{
    public GameObject textBox;
    private TextMeshProUGUI text;
    public string input;

    public List<string> inputs;
    private Image image;

    private static DebugBox _instance;
    public static DebugBox Instance { get { return _instance; } }
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start()
    {
        text = textBox.GetComponent<TextMeshProUGUI>();
        input = "";
        inputs.Clear();

        image = gameObject.GetComponent<Image>();

        textBox.SetActive(false);
    }


    private Coroutine Grad_remove = null;
    //private Coroutine Fade = null;

    private float t_time { get => t_time_; set { t_time_ = Time.timeSinceLevelLoad + value; } }
    private float t_time_;
    public int MaxLines = 8;
    private int CurrentLines = 0;
    Regex rgx = new Regex(@"(.*\n)", RegexOptions.IgnoreCase);
    void Update()
    {
        if(inputs.Count != 0)
        {
            if(CurrentLines >= MaxLines)
            {
                Match match = rgx.Match(text.text);//Regex.Match(text.text, @"(.*\n)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if(match != null)
                {
                    text.text = rgx.Replace(text.text, "", 1);
                    CurrentLines--;
                }
                
            }

            text.text += text.text == "" ? $"//Debug.Log: {inputs[0]}" : $"\n//Debug.Log: {inputs[0]}";
            CurrentLines++;
            inputs.RemoveAt(0);

            //timer = Time.time + 3f;
            //tryTimer = true;
            textBox.SetActive(true);
            text.color = new Color(1f, 1f, 1f, 0.725f);
            image.color = new Color(0.1981f, 0.1981f, 0.1981f, 0.725f);
            t_time = 5f;
            //transSet = 0.725f;
            //addition = 0f;
        }

        if (text.text != "" && inputs.Count == 0 && t_time <= Time.timeSinceLevelLoad && Grad_remove == null)
        {
            //Debug.Log("fuck");
            Grad_remove = StartCoroutine(Rem());
        }
        else if (t_time >= Time.timeSinceLevelLoad && Grad_remove != null)
        {
            //Debug.Log("kcuf");
            StopCoroutine(Grad_remove);
            Grad_remove = null;
        }
        else if (Grad_remove != null && text.text == "")
        {
            //Debug.Log("kfuc");
            StopCoroutine(Grad_remove);
            Grad_remove = null;
            image.color = new Color(0.1981f, 0.1981f, 0.1981f, 0f);

        }


        
        

        #region Felix
        //if (inputs.Count != 0)
        //{
        //    input = inputs[0];
        //    inputs.RemoveAt(0);

        //    timer = Time.time + 3f;
        //    tryTimer = true;
        //    textBox.SetActive(true);
        //    text.color = new Color(1f, 1f, 1f, 0.725f);
        //    bg1Img.color = new Color(0.1981f, 0.1981f, 0.1981f, 0.725f);
        //    bg2Img.color = new Color(0.1981f, 0.1981f, 0.1981f, 0.725f);
        //    bg3Img.color = new Color(0.1981f, 0.1981f, 0.1981f, 0.725f);
        //    bg4Img.color = new Color(0.1981f, 0.1981f, 0.1981f, 0.725f);
        //    bg5Img.color = new Color(0.1981f, 0.1981f, 0.1981f, 0.725f);
        //    transSet = 0.725f;
        //    addition = 0f;

        //    if (bg1.activeSelf == false)
        //    {
        //        bg1.SetActive(true);
        //        text.text = "//Debug.Log: " + input;
        //        charsInFirstLine = input.Length + 15;

        //        input = "";
        //    }
        //    else if(bg2.activeSelf == false)
        //    {
        //        bg2.SetActive(true);
        //        text.SetText(text.text + System.Environment.NewLine + "//Debug.Log: " + input);

        //        input = "";
        //    }
        //    else if (bg3.activeSelf == false)
        //    {
        //        bg3.SetActive(true);
        //        text.text = text.text + System.Environment.NewLine + "//Debug.Log: " + input;

        //        input = "";
        //    }
        //    else if (bg4.activeSelf == false)
        //    {
        //        bg4.SetActive(true);
        //        text.text = text.text + System.Environment.NewLine + "//Debug.Log: " + input;

        //        input = "";
        //    }
        //    else if (bg5.activeSelf == false)
        //    {
        //        bg5.SetActive(true);
        //        text.text = text.text + System.Environment.NewLine + "//Debug.Log: " + input;

        //        input = "";
        //    }
        //    else
        //    {
        //        text.text = text.text.Remove(0, charsInFirstLine);
        //        charsInFirstLine = text.text.Split(':')[0].Length + text.text.Split(':')[1].Length - 10;

        //        text.text = text.text + System.Environment.NewLine + "//Debug.Log: " + input;
        //        input = "";
        //    }
        //}

        //if (Time.time > timer & tryTimer == true)
        //{
        //    if (Time.time > timer && Time.time <= timer + 0.1f) { transSet = 0.725f - 0.0725f; addition = 0.1f; }
        //    else if (Time.time > timer + addition && Time.time <= timer + addition + 0.1f) { transSet -= 0.0725f; addition += 0.1f; }

        //    //if (Time.time > timer && Time.time <= timer + 0.05f) { transSet = 0.725f - 0.03625f; addition = 0.05f; }
        //    //else if (Time.time > timer + addition && Time.time <= timer + addition + 0.05f) { transSet -= 0.03625f; addition += 0.5f; }

        //    bg1Img.color = new Color(0.1981f, 0.1981f, 0.1981f, transSet);
        //    bg2Img.color = new Color(0.1981f, 0.1981f, 0.1981f, transSet);
        //    bg3Img.color = new Color(0.1981f, 0.1981f, 0.1981f, transSet);
        //    bg4Img.color = new Color(0.1981f, 0.1981f, 0.1981f, transSet);
        //    bg5Img.color = new Color(0.1981f, 0.1981f, 0.1981f, transSet);
        //    text.color = new Color(1, 1, 1, transSet);

        //    if (Time.time > timer + 1)
        //    {
        //        text.text = "";

        //        textBox.SetActive(false);
        //        bg1.SetActive(false);
        //        bg2.SetActive(false);
        //        bg3.SetActive(false);
        //        bg4.SetActive(false);
        //        bg5.SetActive(false);

        //        tryTimer = false;
        //    }
        //}
        #endregion
    }

    IEnumerator Rem()
    {
        //yield return new WaitForSecondsRealtime(1f);

        while (CurrentLines > 0)
        {
            if (CurrentLines > 1)
            {
                text.text = rgx.Replace(text.text, "", 1);
                CurrentLines--;
            }
            else if(CurrentLines == 1)//single lines wont end with \n which is what the regex pattern uses
            {
                Regex regex = new Regex(@"(.*)");
                text.text = regex.Replace(text.text, "", 1);
                CurrentLines--;
            }
            yield return new WaitForSecondsRealtime(3f);
        }
        Grad_remove = null;
        yield break;
    }

    //IEnumerator Fade()
    //{

    //}
}
