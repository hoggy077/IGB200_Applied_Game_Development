using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingScript : MonoBehaviour
{
    private bool isActive = false;
    public float Duration = 1;
    public bool ToggleButton;

    // Start is called before the first frame update
    void Start()
    {
        FadeFromBlack(Duration);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            ToggleFade();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (ToggleButton) {
            ToggleFade();
            Debug.LogWarning("FadeToggled");
            ToggleButton = false;
        }
    }

    public void ToggleFade() {
        FadeScreen();
        isActive = !isActive;
    }
    public void FadeScreen() {
        if (isActive) {
            StartCoroutine(FadeToBlack(Duration));
        } else {
            StartCoroutine(FadeFromBlack(Duration));
        }
    }

    public IEnumerator FadeToBlack(float duration) {
        
        Image sr = GetComponent<Image>();
        float t = 0;
        Color color = sr.color;
        sr.enabled = true;
        while (t < duration) {
            color.a = Mathf.Clamp01(t / duration );
            sr.color = color;
            t += Time.deltaTime;
            yield return null;
        }
        color.a = 1;
        sr.color = color;
        sr.enabled = true;
    }
    
    public IEnumerator FadeFromBlack (float duration) {
        Image sr = GetComponent<Image>();
        float t = 0;
        Color color = sr.color;
        sr.enabled = true;
        while (t < Duration) {
            color.a = 1 - Mathf.Clamp01(t / Duration);
            sr.color = color;
            t += Time.deltaTime;
            yield return null;
        }
        color.a = 0;
        sr.color = color;
        sr.enabled = false;
    }
     
}
