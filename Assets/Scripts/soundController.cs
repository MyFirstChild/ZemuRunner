using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundController : MonoBehaviour {

    // Use this for initialization
    public AudioClip hitAudio, coinAudio, countAuido, goAudio, endAudio, starAudio, bombAudio, powerAuido, lifeAudio;
    public AudioSource soundEffect;
	void Start () {
        soundEffect = GameObject.Find("sound effect").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void hitPlay()
    {
        soundEffect.PlayOneShot(hitAudio);
    }

    public void coinPlay()
    {
        soundEffect.PlayOneShot(coinAudio);
    }
    public void playAudio(AudioClip clip)
    {
        soundEffect.PlayOneShot(clip);
    }
}
