using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    public GameObject _player;
    public Transform deadCamTarget;
    private Camera camera;
    private float xTemp, targetDistance;
    	
    void Start()
    {
        xTemp = 0;
        targetDistance = 3.5f;
        camera = GetComponent<Camera>();
    }

	// Update is called once per frame
	void LateUpdate () {
        if (gameController.gc.gameState == 1)
        {
            transform.position = new Vector3(0, 27.5f, _player.transform.position.z - 3.5f);
        }
        else if(gameController.gc.gameState == 0)
        {
            /*if(transform.position.z > _player.transform.position.z - 10.75f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.05f);
            }*/
            if (camera.fieldOfView > 30)
            {
                camera.fieldOfView -= 22.5f * Time.deltaTime;
            }

            transform.position = Vector3.MoveTowards(transform.position, deadCamTarget.position, targetDistance * Time.deltaTime);

            //_player.transform.Rotate(transform.up * 60 * Time.deltaTime);
            
            if(_player.GetComponent<playerController>().isInWater)
            {
                if (_player.transform.eulerAngles.y < 180)
                {
                    _player.transform.eulerAngles = new Vector3(_player.transform.eulerAngles.x - (15 * Time.deltaTime), _player.transform.eulerAngles.y + (90 * Time.deltaTime), _player.transform.eulerAngles.z);
                }
                deadCamTarget.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 16.5f, _player.transform.position.z - 8f);
            }
            else
            {
                deadCamTarget.transform.position = new Vector3(_player.transform.position.x - 0.5f, _player.transform.position.y + 16.5f, _player.transform.position.z - 10.5f);
            }
        }
		
	}

    public void setTarget()
    {
        xTemp = Mathf.Abs(deadCamTarget.position.x);
        if(xTemp > 7)
        {
            targetDistance = xTemp / 2;
        }
    }
}
