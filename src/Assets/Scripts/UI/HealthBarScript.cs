using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public Image[] Hearts;
    public GameObject target;
    //Start is called before the first frame update
    void Start() {
        target = GameObject.FindGameObjectWithTag("Player");
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update() {
        UpdateHealthBar();
    }

    void UpdateHealthBar() {
        for (int i = 0; i < Hearts.Length; i++) {
            if (i < PlayerEntity.Instance.Health_) {
                Hearts[i].sprite = FullHeart;
            } else {
                Hearts[i].sprite = EmptyHeart;
            }

            if (i < PlayerEntity.Instance.MaxHealth_) {
                Hearts[i].enabled = true;
            } else {
                Hearts[i].enabled = false;
            }
        }
    }
}
