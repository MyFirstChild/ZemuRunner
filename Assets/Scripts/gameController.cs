using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

	// Use this for initialization
    public static gameController gc;
    public int coin, distance, gameState, tileCount,life ;
    public float time, timeMinute;
    public GameObject ui, dataPref;
    public GameObject sound;

    public uiController _uiCtrl;
    public playerController _playerCtrl;
    public dataController _data;
    public audioController _audio;

    void Awake () {
        gc = GetComponent<gameController>();
        if(!GameObject.Find("dataController"))
        {
            Instantiate(dataPref);
            _data = GameObject.Find("dataController(Clone)").GetComponent<dataController>();
        }
        else
            _data = GameObject.Find("dataController").GetComponent<dataController>();
        _audio = GameObject.Find("audio").GetComponent<audioController>();
        coin = 0;
        time = 0;
        timeMinute = 0;
        distance = 0;
        gameState = 1;
        life = 1;
        tileCount = 0;
        distance = 0;
        //StartCoroutine(wait());
        sound = GameObject.Find("sound");
        checkMuted();
    }
	
	// Update is called once per frame
	void Update () {
        if (life <= 0 && gameState == 1)
        {
            StartCoroutine(gameOver());
            
            //StartCoroutine(iniStartScene());
        }

        if (gameState == 1)
        {
            time += Time.deltaTime;
            if(time >= 60)
            {
                time = 0;
                timeMinute++;
            }
        }  
	}

    IEnumerator gameOver()
    {
        gameState = 0;
        GameObject.Find("Main Camera").GetComponent<cameraScript>().setTarget();
        _data.saveData(distance);
        _data.saveCoin(coin);        
        yield return new WaitForSeconds(3f);
        StartCoroutine(captureDead(Screen.width, Screen.width));

        gameController.gc._audio.playAudio(gameController.gc._audio.gameAudio[3]);
    }    

    IEnumerator iniStartScene()
    {
        sound.GetComponent<soundController>().playAudio(sound.GetComponent<soundController>().endAudio);
        yield return new WaitForSeconds(90f);

        SceneManager.LoadScene(0);
    }
    public void restart()
    {
        SceneManager.LoadScene(1);
    }

    public IEnumerator captureDead(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        _uiCtrl.deadCam.texture = texture;
        _uiCtrl.gameOverBoard();
    }

    void checkMuted()
    {
        if (_data._data.isMute == 0)
        {
            GameObject.Find("bgm").GetComponent<AudioSource>().volume = 0.5f;
            GameObject.Find("audio").GetComponent<AudioSource>().volume = 0.8f;
        }
        else
        {
            GameObject.Find("bgm").GetComponent<AudioSource>().volume = 0;
            GameObject.Find("audio").GetComponent<AudioSource>().volume = 0f;
        }
    }
}
