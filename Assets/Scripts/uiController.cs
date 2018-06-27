using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class uiController : MonoBehaviour {

    public Text distance, coin;
    public Text gameOver_set1, gameOver_set2, gameOver_set3, endDistant, endCoin;
    public Button pauseBtn, resumeBtn, restartBtn, replayBtn, homeBtn, homeBtn2, shareBtn;
    public GameObject gamePanel,pausePanel, endPanel, tutorialPanel;
    public RawImage deadCam, catchphrases;
    public Texture[] catchphrasesList, catchphrasesWaterList;

    private string[,] text_set1;
    private string[] text_set2, text_set3;

	// Use this for initialization
	void Start () {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        pauseBtn.onClick.AddListener(pauseGame);
        resumeBtn.onClick.AddListener(resumeGame);
        restartBtn.onClick.AddListener(restartGame);
        replayBtn.onClick.AddListener(restartGame);
        homeBtn.onClick.AddListener(mainMenu);
        homeBtn2.onClick.AddListener(mainMenu);
        
    }
	
	// Update is called once per frame
	void Update () {

        distance.text = gameController.gc.distance.ToString();
        coin.text = gameController.gc.coin.ToString();
    }

    public void gameOverBoard()
    {
        endPanel.SetActive(true);
        gamePanel.SetActive(false);
        if(gameController.gc._playerCtrl.isInWater)
        {
            catchphrases.texture = catchphrasesWaterList[(Random.Range(0, 97) * gameController.gc.distance) %  catchphrasesWaterList.Length];
        }
        else
            catchphrases.texture = catchphrasesList[(Random.Range(0, 97) * gameController.gc.distance) % catchphrasesList.Length];
        if(gameController.gc._playerCtrl.isInWater)
        {
            deadCam.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        }
        endDistant.text = gameController.gc.distance.ToString();
        endCoin.text = gameController.gc.coin.ToString();

    }

    void pauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //if()
        gameController.gc._audio.playAudio(gameController.gc._audio.uiAudio[0]);
    }

    void resumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        gameController.gc._audio.playAudio(gameController.gc._audio.uiAudio[0]);
    }

    void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        gameController.gc._audio.playAudio(gameController.gc._audio.uiAudio[0]);
    }
    void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        gameController.gc._audio.playAudio(gameController.gc._audio.uiAudio[0]);
    }

    
    /*
    public void setGameOverText(int set1_1, int set1_2, int set2, int set3)
    {
        text_set1 = new string[9, 11] 
        { 
            { "เป็ดน้อย", "น้องอ้วน", "ขุนแฉะ", "อีแฉะ", "แปะลื่นน้ำ", "อีเปียก", "น้องหวานเย็น", "เบรกแตก", "แดรกตะไคร่", "โด้สายเย็น", "เตี้ยเปียกเเคระ" },        
            { "ขี้ตาแหล้", "แมงตั๊กคำ", "อี่ฮวกกบ", "แมงแซ๊บ", "แมงอี่มิบ", "จั๊กกะเล้อเกลี้ยง", "แมงเวา", "จั๊กเข็บคือ", "อี่นิ่ว", "แมงเวา", "แมงก๋ำบี้" },
            { "ขี้ซุย", "เตี้ยวอนตีน", "ป้าย่น", "น้องกะหล่ำ", "งอกกระโหลก", "หน่อมแน้ม", "ป้อแป", "หยอง", "เหลืองสายแบ๋ว", "ปลวกคันหู", "ลูกชิ้นถลาลม" },
            { "กระโจกหน้าโอ่ง", "พะยูน", "นางอ้วน", "ซิ้มน้อย", "นางฟ้าข้างโอ่ง", "นังงูพิษ", "ไอ้ดื้อ", "เหม่งห้อย", "โจ๋หลังโอ่ง", "ถังแก๊ส", "ยูนคุง" },
            { "โต๊ะชาย", "หวา", "น้องพ่าย", "สาวนุ้ย", "นายหญิง", "เติ่น", "พี่บ่าว", "โหม๋เรา", "หม่อง", "คะยองโอ", "เสี่ย" },
            { "มนุษย์หิน", "ธรณีวิทยา", "ก้องภพ", "ธุรีดิน", "ละอองฝุ่น", "น้องดินร่วน", "น้องทราย", "ขี้โคลน", "ขี้เลน", "ดินเปียก", "ก้อนกรวด" },
            { "บอระเพ็ด", "ลั่นทม", "หูกระจง", "ตรีนเป็ด", "แคนา บ้านไร่", "มะฮวกกานี", "หลิว", "จั๋งจีน", "ทองอุไร", "ซองออฟ", "ยี่โถ" },
            { "อาโกว", "ฮัวกง", "อาม่า", "อาเตี๋ย", "อาซิ้ม", "อากิ๋ม", "อาอี๊", "อากู๋ ", "อาอึ้ม", "อาเจ่ก", "อาแปะ" },
            { "เด็กแว๊น", "นักซิ่ง", "สก๊อย", "โชเฟอร์", "สารถี", "สิบล้อ", "หกล้อ", "รถเมล์", "กระเป๋า", "พี่วิน", "สามล้อรอรัก" }
        };

        text_set2 = new string[15] { "อยากบอกว่า", "ตะโกนว่า ", "กระซิบว่า ", "ลือกันว่า", "ปิดกันให้", "โดนนินทาว่า", "โดนล้อว่า", "ยืนหน้าผา แล้วพูดว่า", "ขึ้นดาดฟ้าตะโกนว่า", "ให้เพื่อนพูดแทนว่า ", "พิมพ์ความในใจว่า", "แถลงข่าวว่า", "ยืนโพเดียม แล้วกล่าวว่า", "ยืนกลางแดด กางแขน ตะโกนว่า", "ให้ตายเถอะ " };
        text_set3 = new string[19] { "หมูหยอง", "แชร์แหม", "อ่อนวะ", "โดน", "ไอ้กากเอ้ย", "แว้นวะ",
            "nodไหมพี่", "เก่งป่ะละ", "ไปกับพี่ไหมน้อง", "ยิ้มสุดท้าย", "ขอสามร้อย", "แจ็คโคตรหล่อ", "เกมกูเอง", "ไฝว้กูป่าว", "พี่ชอบหอยลาย", "ลิ้นพี่รัวนะ", "ไส้กรอกค็อกเทล", "หัวเห็ด", "อีงูพิษ"};

        gameOver_set1.text = text_set1[set1_1, set1_2];
        gameOver_set2.text = text_set2[set2];
        gameOver_set3.text = text_set3[set3];


        //gameOver_set3.text = text_set3[set3];

    }

    public void setGameOverTextLayout()
    {
        gameOver_set2.rectTransform.position = new Vector3(40 + gameOver_set1.rectTransform.position.x + gameOver_set1.rectTransform.rect.width, gameOver_set2.rectTransform.position.y, gameOver_set2.rectTransform.position.z);
        gameOver_set3.rectTransform.position = new Vector3(100 + gameOver_set1.rectTransform.position.x + gameOver_set1.rectTransform.rect.width + gameOver_set2.rectTransform.rect.width, gameOver_set3.rectTransform.position.y, gameOver_set3.rectTransform.position.z);
        Debug.Log(gameOver_set1.rectTransform.position.x);

    }*/
}