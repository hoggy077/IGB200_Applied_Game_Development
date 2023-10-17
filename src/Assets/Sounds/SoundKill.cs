using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundKill : MonoBehaviour
{
    public float lifeTime = 3;
    public int bgmType;
    private bool playing;

    // Use this for initialization
    void Start()
    {
        if (lifeTime != 0)
        {
            Destroy(this.gameObject, lifeTime);
        }

        if(bgmType == 1) { playing = true; GetComponent<AudioSource>().Play(); }
        else { playing = false; }
    }

    private void Update()
    {
        if(bgmType == 1 && playing) // Overworld BGM
        {
            if (GameObject.FindGameObjectWithTag("Player").transform.position.x > 5 && GameObject.FindGameObjectWithTag("Player").transform.position.y < 75
                && GameObject.FindGameObjectWithTag("Player").transform.position.y > 60)
            {
                foreach (GameObject bgm in GameObject.FindGameObjectsWithTag("BGM"))
                {
                    if (bgm.GetComponent<SoundKill>().bgmType == 2)
                    {
                        bgm.GetComponent<AudioSource>().Play();
                        bgm.GetComponent<SoundKill>().playing = true;
                        GetComponent<AudioSource>().Stop();
                        playing = false;
                    }
                }
            }
        }
        else if(bgmType == 2 && playing) // Dungeon BGM
        {
            if (GameObject.FindGameObjectWithTag("Player").transform.position.x < 5)
            {
                foreach (GameObject bgm in GameObject.FindGameObjectsWithTag("BGM"))
                {
                    if (bgm.GetComponent<SoundKill>().bgmType == 1)
                    {
                        bgm.GetComponent<AudioSource>().Play();
                        bgm.GetComponent<SoundKill>().playing = true;
                        GetComponent<AudioSource>().Stop();
                        playing = false;
                    }
                }
            }
        }
    }
}
