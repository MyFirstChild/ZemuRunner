using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour {

    public GameObject camPanel, customizePanel, frameMask;
    public Material faceMat;
    public Button shutterBtn, swapBtn, skipBtn;
    public mainMenuScript _main;
    public Canvas canvas;

    WebCamTexture camTexture, backCamTexture;
    private bool isCam, isFronting;
    public RawImage background, frame, customizePhoto;
   

    // Use this for initialization
    void Start () {

        shutterBtn.onClick.AddListener(shutter);
        swapBtn.onClick.AddListener(swapCam);
        skipBtn.onClick.AddListener(skip);
        customizePanel.SetActive(false);

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            isCam = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                camTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            if (!devices[i].isFrontFacing)
            {
                backCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (camTexture == null && backCamTexture == null)
        {
            return;
        }

        if (camTexture != null)
        {
            camTexture.Play();
            background.texture = camTexture;
            isFronting = true;
        }
        else if(backCamTexture != null)
        {
            backCamTexture.Play();
            background.texture = backCamTexture;
            isFronting = false;
        }
        else
        {
            return;
        }
        
        isCam = true;
                        
        background.rectTransform.localScale = new Vector3(1f, -1, 1f);

    }
	
	// Update is called once per frame
	/*void Update () {
        if(isFronting)
        {
            background.texture = camTexture;
        }
        else if(!isFronting)
        background.texture = backCamTexture;

        //float ratio = (float)camTexture.width / (float)camTexture.height;
        //fit.aspectRatio = ratio;

        //float scaleY = camTexture.videoVerticallyMirrored ? -1f : 1f;

        //int orient = -camTexture.videoRotationAngle;
        //background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        //background.transform.rotation = baseRotation* Quaternion.AngleAxis(camTexture.videoRotationAngle, Vector3.up);
        //Debug.Log(orient);
    }*/

    void skip()
    {
        customizePanel.SetActive(true);
        if(!_main.isUsingPhoto[_main.charNum])
            customizePhoto.texture = _main._data.faceOriginal[_main.charNum];
        else
        {
            customizePhoto.texture = snapshot;
        }
    }

    public void shutter()
    {
        StartCoroutine(TakeSnapshot((int)(frame.rectTransform.rect.width * canvas.scaleFactor), (int)(frame.rectTransform.rect.height * canvas.scaleFactor)));
    }

    private Texture2D snapshot;
    public IEnumerator TakeSnapshot(int width, int height)
    {
        frameMask.SetActive(false);
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(frame.rectTransform.position.x, frame.rectTransform.position.y, width, height), 0, 0);
        texture.Apply();

        snapshot = texture;

        _main._data.faceMat[_main.charNum].SetTexture("_MainTex",snapshot);
        _main._audio.playAudio(_main._audio.uiAudio[2]);
        _main.isUsingPhoto[_main.charNum] = true;

        frameMask.SetActive(true);
        customizePanel.SetActive(true);
        customizePhoto.texture = snapshot;
    }

    void swapCam()
    {
        if (background.texture == camTexture)
        {
            if(backCamTexture == null)
            {
                return;
            }
            camTexture.Stop();
            if (backCamTexture != null)
                backCamTexture.Play();
            background.texture = backCamTexture;
            isFronting = false;
            background.rectTransform.localScale = new Vector3(-1f, -1, 1f);
        }
        else if(background.texture == backCamTexture)
        {
            if (camTexture == null)
            {
                return;
            }
            backCamTexture.Stop();
            if (camTexture != null)
                camTexture.Play();
            background.texture = camTexture;
            isFronting = true;
            background.rectTransform.localScale = new Vector3(1f, -1, 1f);
        }
    }
}
