using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuScript : MonoBehaviour {

    public Button startBtn;
    public Button nextCharBtn, prevCharBtn, characterBtn, homeBtn, cameraBtn, resetBtn, customizeBtn, mainBtn, audioBtn;
    public Text highScore, coin;
    public GameObject characterPanel, camPanel, customizePanel, mainPanel, video;

    public int charNum;
    public bool[] isUsingPhoto = new bool[3];
    public GameObject avatarTemp, avatar, dataObj;
    public dataController _data;
    public audioController _audio;
    public AudioSource _bgm;
    public Sprite audioOn, audioOff;

    public Camera camera;
    private float perspectiveZoomSpeed = 0.2f;


    // Use this for initialization
    void Start () {
        if (!GameObject.Find("dataController"))
        {
            dataObj = Instantiate(dataObj);
            dataObj.name = "dataController";            
        }
        else
        {
            dataObj = GameObject.Find("dataController");
        }
        _audio = GameObject.Find("audio").GetComponent<audioController>();
        _data = dataObj.GetComponent<dataController>();
        createPlayerChar();        

        highScore.text = _data._data.highestScore.ToString(); 
        coin.text = _data._data.coin.ToString();        

        startBtn.onClick.AddListener(startGame);
        customizeBtn.onClick.AddListener(customize);
        mainBtn.onClick.AddListener(mainMenu);
        characterBtn.onClick.AddListener(changeCharacter);
        homeBtn.onClick.AddListener(changeCharacter);
        nextCharBtn.onClick.AddListener(nextChar);
        prevCharBtn.onClick.AddListener(prevChar);
        cameraBtn.onClick.AddListener(openCam);
        resetBtn.onClick.AddListener(resetFace);
        audioBtn.onClick.AddListener(muteAudio);

        checkMuted();
    }

    void Update()
    {
        zoom();
    }
	
    private void createPlayerChar()
    {
        charNum = _data._data.charChoice;
        avatar.transform.localScale = new Vector3(1, 1, 1);
        avatarTemp = Instantiate(_data.playerChar[charNum]);
        avatarTemp.transform.rotation = Quaternion.Euler(0, 180f, 0);
        avatarTemp.transform.SetParent(avatar.transform);
    }

    void checkMuted()
    {
        if (_data._data.isMute == 0)
        {
            _bgm.volume = 0.8f;
            GameObject.Find("audio").GetComponent<AudioSource>().volume = 0.5f;
            audioBtn.GetComponent<Image>().sprite = audioOn;
        }
        else
        {
            _bgm.volume = 0;
            GameObject.Find("audio").GetComponent<AudioSource>().volume = 0f;
            audioBtn.GetComponent<Image>().sprite = audioOff;
        }
    }

    void startGame()
    {
        SceneManager.LoadScene(1);
        _audio.playAudio(_audio.uiAudio[0]);
    }

    void muteAudio()
    {
        if(_data._data.isMute == 0)
        {
            _data._data.isMute = 1;
        }
        else
        {
            _data._data.isMute = 0;
        }
        checkMuted();
        _data.saveIsMute(_data._data.isMute);
    }

    void mainMenu()
    {
        mainPanel.SetActive(true);
        video.SetActive(true);
        customizePanel.SetActive(false);
    }

    void customize()
    {
        mainPanel.SetActive(false);
        video.SetActive(false);
        customizePanel.SetActive(true);
    }


    void openCam()
    {
        camPanel.SetActive(true);
        _audio.playAudio(_audio.uiAudio[0]);
    }

    void resetFace()
    {
        _data.faceMat[charNum].SetTexture("_MainTex", _data.faceOriginal[charNum]);
        _audio.playAudio(_audio.uiAudio[3]);
        isUsingPhoto[charNum] = false;
    }

    void changeCharacter()
    {
        characterPanel.SetActive(!characterPanel.activeSelf);

    }

    void nextChar()
    {
        ++charNum;
        if(charNum > _data.playerChar.Length -1)
        {
            charNum = 0;
        }
        changeChar();
    }

    void prevChar()
    {
        --charNum;
        if (charNum < 0)
        {
            charNum = _data.playerChar.Length -1;
        }
        changeChar();
    }

    void changeChar()
    {
        foreach (Transform child in avatar.transform)
        {
            Destroy(child.gameObject);
        }
        avatar.transform.localScale = new Vector3(1, 1, 1);
        avatarTemp = Instantiate(_data.playerChar[charNum]);
        avatarTemp.transform.rotation = Quaternion.Euler(0, 180f, 0);
        avatarTemp.transform.SetParent(avatar.transform);
        _data.saveCharChoice(charNum);
        _audio.playAudio(_audio.uiAudio[4]);
    } 


    private void zoom()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            avatar.transform.localScale = new Vector3(avatar.transform.localScale.x - (deltaMagnitudeDiff * 0.01f), avatar.transform.localScale.y - (deltaMagnitudeDiff * 0.01f), avatar.transform.localScale.z - (deltaMagnitudeDiff * 0.01f));
            avatar.transform.localScale = new Vector3(Mathf.Clamp(avatar.transform.localScale.x, 0.5f, 2f), Mathf.Clamp(avatar.transform.localScale.y, 0.5f, 2f), Mathf.Clamp(avatar.transform.localScale.z, 0.5f, 2f));

            /*camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 30f, 90f);*/
        }


    }
}
