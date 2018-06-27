using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class equipmentScript : MonoBehaviour {

    public GameObject camCanvas;
    public Canvas canvas;
    public RawImage eye, mouth, head, etc, frame;
    public Button eyeBtn, mouthBtn, headBtn, etcBtn, finishBtn, editEyeBtn, editHeadBtn, editMouthBtn, editEtcBtn;
    public Texture[] eyeImg, mouthImg, headImg, etcImg;
    public Sprite transformFrame, fullTransparent;
    private int[] imgPointer = new int[4] {0,0,0,0};
    private GameObject editingObj, editingFrame;
    private bool isRotating, isMovable;
    Vector2 startVector;
    float zRotation;

    public cameraController _cameraController;
    public mainMenuScript _main;

    // Use this for initialization
    void Start () {
        eyeBtn.onClick.AddListener(changeEye);
        mouthBtn.onClick.AddListener(changeMouth);
        headBtn.onClick.AddListener(changeHead);
        etcBtn.onClick.AddListener(changeEtc);
        finishBtn.onClick.AddListener(finishCustomize);
        editEyeBtn.onClick.AddListener(editEye);
        editHeadBtn.onClick.AddListener(editHead);
        editMouthBtn.onClick.AddListener(editMouth);
        editEtcBtn.onClick.AddListener(editEtc);

        editEyeBtn.gameObject.SetActive(false);
        editHeadBtn.gameObject.SetActive(false);
        editMouthBtn.gameObject.SetActive(false);
        editEtcBtn.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        editObj();
        Debug.Log(editHeadBtn.gameObject.transform.position.y);
	}

    private void editObj()
    {
        if (editingObj == null)
            return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                if(touch.position.x > editingFrame.transform.position.x && touch.position.x < editingFrame.transform.position.x + (editingFrame.GetComponent<Image>().rectTransform.rect.width * canvas.scaleFactor) && touch.position.y > editingFrame.transform.position.y && touch.position.y < editingFrame.transform.position.y + (editingFrame.GetComponent<Image>().rectTransform.rect.height * canvas.scaleFactor))
                {
                    isMovable = true;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (touch.position.y < frame.transform.position.y)
                {
                    exitEdit();
                }
            }          
            
            if (touch.phase == TouchPhase.Moved)
            {
                if (isMovable)
                {
                    editingObj.transform.position = touch.position;
                }
            }
        }
        else
        {
            isMovable = false;
        }
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


            if (!isRotating)
            {
                startVector = Input.GetTouch(1).position - Input.GetTouch(0).position;
                zRotation = editingObj.transform.eulerAngles.z;
                isRotating = true;
            }
            else
            {
                Vector2 currVector = Input.GetTouch(1).position - Input.GetTouch(0).position;
                float angleOffset = Vector2.Angle(startVector, currVector);
                Vector3 LR = Vector3.Cross(startVector, currVector);
                
                if (angleOffset > 5f)
                {
                    if (LR.z > 0)
                    {
                        editingObj.transform.eulerAngles = new Vector3(editingObj.transform.eulerAngles.x, editingObj.transform.eulerAngles.y, zRotation + angleOffset);
                        //editingObj.transform.Rotate(transform.forward * angleOffset);
                    }
                    else if (LR.z < 0)
                    {
                        //editingObj.transform.Rotate(transform.forward * -angleOffset);
                        editingObj.transform.eulerAngles = new Vector3(editingObj.transform.eulerAngles.x, editingObj.transform.eulerAngles.y, zRotation - angleOffset);
                    }
                }
                else
                {
                    editingObj.transform.localScale = new Vector3(editingObj.transform.localScale.x - deltaMagnitudeDiff * 0.01f, editingObj.transform.localScale.y - deltaMagnitudeDiff * 0.01f, editingObj.transform.localScale.z - deltaMagnitudeDiff * 0.01f);
                    editingObj.transform.localScale = new Vector3(Mathf.Clamp(editingObj.transform.localScale.x, 0.1f, 2f), Mathf.Clamp(editingObj.transform.localScale.y, 0.1f, 2f), Mathf.Clamp(editingObj.transform.localScale.z, 0.1f, 2f));
                }
            }
        }
        else isRotating = false;
    }

    void editEye()
    {
        exitEdit();
        if (editingObj == null)
        {
            editingObj = eye.gameObject;
            editingFrame = editEyeBtn.gameObject;
            editEyeBtn.image.sprite = transformFrame;
        }
    }

    void editHead()
    {
        exitEdit();
        if (editingObj == null)
        {
            editingObj = head.gameObject;
            editingFrame = editHeadBtn.gameObject;
            editHeadBtn.image.sprite = transformFrame;
        }
    }

    void editMouth()
    {
        exitEdit();
        if (editingObj == null)
        {
            editingObj = mouth.gameObject;
            editingFrame = editMouthBtn.gameObject;
            editMouthBtn.image.sprite = transformFrame;
        }
    }

    void editEtc()
    {
        exitEdit();
        if (editingObj == null)
        {
            editingObj = etc.gameObject;
            editingFrame = editEtcBtn.gameObject;
            editEtcBtn.image.sprite = transformFrame;
        }
    }

    void exitEdit()
    {
        editingObj = null;
        editingFrame = null;
        editEyeBtn.image.sprite = fullTransparent;
        editHeadBtn.image.sprite = fullTransparent;
        editMouthBtn.image.sprite = fullTransparent;
        editEtcBtn.image.sprite = fullTransparent;
    }

    void finishCustomize()
    {
        exitEdit();
        StartCoroutine("captureImg");
    }

    IEnumerator captureImg()
    {
        shutter();
        yield return new WaitForEndOfFrame();
        this.gameObject.SetActive(false);
        camCanvas.gameObject.SetActive(false);
    }

    void changeEye()
    {
        imgPointer[0] = changePointer(imgPointer[0], eyeImg.Length);
        doChange(eye, eyeImg[imgPointer[0]]);
        if (eye.texture != eyeImg[0])
        {
            editEyeBtn.gameObject.SetActive(true);
        }
        else
        {
            editEyeBtn.gameObject.SetActive(false);
        }

    }

    void changeMouth()
    {
        imgPointer[1] = changePointer(imgPointer[1], mouthImg.Length);
        doChange(mouth, mouthImg[imgPointer[1]]);
        if (mouth.texture != mouthImg[0])
        {
            editMouthBtn.gameObject.SetActive(true);
            editMouthBtn.onClick.AddListener(editMouth);
        }
        else
        {
            editMouthBtn.gameObject.SetActive(false);
        }
    }

    void changeHead()
    {
        imgPointer[2] = changePointer(imgPointer[2], headImg.Length);
        doChange(head, headImg[imgPointer[2]]);
        if (head.texture != headImg[0])
        {
            editHeadBtn.gameObject.SetActive(true);
            editHeadBtn.onClick.AddListener(editHead);
        }
        else
        {
            editHeadBtn.gameObject.SetActive(false);
        }
    }

    void changeEtc()
    {
        imgPointer[3] = changePointer(imgPointer[3], etcImg.Length);
        doChange(etc, etcImg[imgPointer[3]]);
        if (etc.texture != etcImg[0])
        {
            editEtcBtn.gameObject.SetActive(true);
            editEtcBtn.onClick.AddListener(editEtc);
        }
        else
        {
            editEtcBtn.gameObject.SetActive(false);
        }
    }

    int changePointer(int pointer, int length)
    {
        ++pointer;
        if (pointer >= length)
        {
            pointer = 0;
        }
        return pointer;
    }

    void doChange(RawImage _raw, Texture _Img)
    {
        _raw.texture = _Img;
    }

    public void shutter()
    {
        StartCoroutine(TakeSnapshot((int)(frame.rectTransform.rect.width * canvas.scaleFactor), (int)(frame.rectTransform.rect.height * canvas.scaleFactor)));
    }

    private Texture2D snapshot;
    public IEnumerator TakeSnapshot(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(frame.rectTransform.position.x, frame.rectTransform.position.y, width, height), 0, 0);
        texture.Apply();

        snapshot = texture;

        _main._data.faceMat[_main.charNum].SetTexture("_MainTex", snapshot);
        _main._audio.playAudio(_main._audio.uiAudio[0]);
    }
}
