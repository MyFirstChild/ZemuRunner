using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour {

    public AudioSource _audioSource;
    public AudioClip[] uiAudio, gameAudio;

    private static audioController instance = null;

    public static audioController Instance
    {
        get { return instance; }
    }

    // Use this for initialization
    void Awake () {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playAudio(AudioClip _audio)
    {

        _audioSource.PlayOneShot(_audio);

    }
    
}
