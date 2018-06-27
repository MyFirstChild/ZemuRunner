using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boyScript : MonoBehaviour {

    Animator anim;
    playerController _player;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        if (GameObject.Find("player"))
        {
            _player = transform.parent.GetComponent<playerController>();
            _player.anim = anim;
        }
	}
	
}
