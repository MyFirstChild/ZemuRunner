using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starModeGroundScript : MonoBehaviour {

    public GameObject _player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(_player.transform.position.x, 10.6f, _player.transform.position.z);
	}
}
