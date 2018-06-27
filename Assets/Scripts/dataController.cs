using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataController : MonoBehaviour
{

    public dataStorage _data;

    public GameObject[] playerChar;
    public Material[] faceMat;
    public Texture[] faceOriginal;

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _data = new dataStorage();
        loadData();
        faceCheck();
    }
    private void loadData()
    {


        if (PlayerPrefs.HasKey("highestScore"))
        {
            _data.highestScore = PlayerPrefs.GetInt("highestScore");
        }
        if (PlayerPrefs.HasKey("charChoice"))
        {
            _data.charChoice = PlayerPrefs.GetInt("charChoice");
        }
        if (PlayerPrefs.HasKey("coin"))
        {
            _data.coin = PlayerPrefs.GetInt("coin");
        }
        if (PlayerPrefs.HasKey("isMute"))
        {
            _data.isMute = PlayerPrefs.GetInt("isMute");
        }
    }

    private void faceCheck()
    {
        for(int i = 0; i < playerChar.Length; i++)
        {
            if(faceMat[i].mainTexture == null)
            {
                faceMat[i].SetTexture("_MainTex", faceOriginal[i]);
            }

        }


    }

    public void saveData(int score)
    {
        if (_data.highestScore < score)
        {
            _data.highestScore = score;
            PlayerPrefs.SetInt("highestScore", _data.highestScore);
        }
    }
    public void saveCharChoice(int choice)
    {
        _data.charChoice = choice;
        PlayerPrefs.SetInt("charChoice", _data.charChoice);
    }
    public void saveCoin(int coin)
    {
        _data.coin += coin;
        PlayerPrefs.SetInt("coin", _data.coin);
    }

    public void saveIsMute(int choice)
    {
        _data.isMute += choice;
        PlayerPrefs.SetInt("isMute", _data.isMute);
    }
}
