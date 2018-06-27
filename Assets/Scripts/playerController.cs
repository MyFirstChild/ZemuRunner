using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerController : MonoBehaviour {
	
	private CharacterController controller;
	private Vector3 moveVector, movementX;
    public Animator anim;
    public GameObject character, starModeGround, fxStar, fxWater;
    public GameObject[] fxCharacter;
    //public SkinnedMeshRenderer[] skin;
    //public Texture white, normal,headNormal;
    //public Material blue, shirt, hair;

    public float Speed, speedX, floatingTime;
	private float verticalVelocity;
	private float gravity, floor;
    public float distance, startPoint;
    private int mode;
    public bool isFiring = false, isJumping, isInWater = false, isDead = false;
    

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
        startPoint = transform.position.z;
        distance = startPoint;
        speedX = 25f;
        Speed = 25.0f;
        gravity = 50.0f;
        floor = 7f;
        mode = 0;

        verticalVelocity = 0.0f;
        floatingTime = 0;
        
        character = Instantiate(gameController.gc._data.playerChar[gameController.gc._data._data.charChoice], transform.position, transform.rotation);
        character.transform.parent = transform;
                
	}
	
	// Update is called once per frame
	void Update () {      

        
        if (gameController.gc.gameState == 1)
        {
            if(transform.position.y < floor)
            {
                //genGameOverText(null);
                isInWater = true;
                fxWater.SetActive(true);
                dead();
                gameController.gc._audio.playAudio(gameController.gc._audio.gameAudio[5]);
            }
            distance = (transform.position.z - startPoint) / 10f;
            gameController.gc.distance = (int)distance;

            Speed += Time.deltaTime / 5f;
            speedX += Time.deltaTime / 5f;
            moveVector = Vector3.zero;

            if (controller.isGrounded)
            {
                verticalVelocity = -0.5f;
            }

            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }


            // X - L and R


            moveVector.x = Input.GetAxisRaw("Horizontal") * (speedX / 1.5f);



#if UNITY_EDITOR || UNITY_STANDALONE

            if (!isFiring)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    jump();
                }
            }
#else
            if (controller.isGrounded)
            {
                swipe();
            }
                
            moveVector.x = Input.acceleration.x * speedX;
#endif

            // Y - Up and Down
            moveVector.y = verticalVelocity;
            
            // Z - Forward and Backward
            moveVector.z = Speed;            
            controller.Move(moveVector * Time.deltaTime);

            


        }
        else if (gameController.gc.gameState == 0)
        {
            moveVector = Vector3.zero;
        }

        animControl();


    }

    /*private void LateUpdate()
    {
        if (mode == 1)
        {
            if (transform.position.y < 10.6f)
                transform.position = new Vector3(transform.position.x, 10.6f, transform.position.z);
        }
    }*/

    protected Vector2 m_StartingTouch;
    protected bool m_IsSwiping = false;
    void swipe()
    {
        if (Input.touchCount == 1)
        {
            if (m_IsSwiping)
            {
                Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

                // Put difference in Screen ratio, but using only width, so the ratio is the same on both
                // axes (otherwise we would have to swipe more vertically...)
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);

                if (diff.magnitude > 0.05f) //we set the swip distance to trigger movement to 1% of the screen width
                {
                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        if (diff.y > 0)
                        {
                            /*jump();
                            if (!isJumping)
                            {
                                isJumping = true;
                                jump();
                            }*/
                        }
                    }

                    m_IsSwiping = false;
                }
            }

            // Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
            // a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                jump();
                m_StartingTouch = Input.GetTouch(0).position;
                m_IsSwiping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_IsSwiping = false;
            }
            

        }

    }
    void jump()
    {
        if (controller.isGrounded)
        {
            gameController.gc._audio.playAudio(gameController.gc._audio.gameAudio[0]);
            verticalVelocity = 20f;
            anim.Play("Jump");
            StartCoroutine(createFX(fxCharacter[0], transform, 0));
        }
    }

    IEnumerator starMode()
    {
        float speedTmp = Speed;
        Speed *= 1.5f;
        starModeGround.SetActive(true);
        mode = 1;
        gameObject.layer = 10;
        yield return new WaitForSeconds(5f);
        Speed = speedTmp;
        starModeGround.SetActive(false);
        mode = 0;
        gameObject.layer = 8;
    }
    IEnumerator magnetMode()
    {
        mode = 2;
        yield return new WaitForSeconds(10f);
        mode = 0;
    }
    private IEnumerator createFX(GameObject fx, Transform fxPos, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Instantiate(fx, fxPos.position, transform.rotation);
    }
    
    void dead()
    {
        isDead = true;
        gameController.gc.life -= 1;
        Instantiate(fxCharacter[1], transform.position, transform.rotation);
    }

    void animControl()
    {
        if (gameController.gc.gameState == 1)
        {
            if (controller.isGrounded || mode == 1)
            {
                anim.Play("Run");
                floatingTime = 0;
            }
            else
            {
                floatingTime += Time.deltaTime;
                if (floatingTime > 0.2f)
                {
                    anim.Play("Jump");
                }
            }
        }
        else
        {
            if(gameController.gc.life <= 0 && transform.position.y > 10f)
            {
                anim.Play("Dead");
            }
            else
            {
                anim.Play("DeadSwim");
            }
        }
    }

    /*private void genGameOverText(Collider hit)
    {
        int set1_1, set1_2, set2, set3;
        if (hit != null)
        {
            set1_1 = hit.GetComponent<obstacleScript>().type;
        }
        else set1_1 = 0;
        
        set1_2 = (gameController.gc.distance % 110) / 10;
        set2 = gameController.gc.coin % 15;
        set3 = Random.Range(0,19);
        gameController.gc._uiCtrl.setGameOverText(set1_1, set1_2, set2, set3);
    }*/

	//Get Coin
	void OnTriggerEnter(Collider hit) {
        
        if(hit.gameObject.tag == "coin")
        {
            Destroy(hit.gameObject);
            ++gameController.gc.coin;
            gameController.gc._audio.playAudio(gameController.gc._audio.gameAudio[2]);
        }

        if (hit.gameObject.tag == "obstacle")
        {
            //genGameOverText(hit);
            dead();
            gameController.gc._audio.playAudio(gameController.gc._audio.gameAudio[4]);
            gameController.gc._audio.playAudio(gameController.gc._audio.gameAudio[6]);
            fxStar.SetActive(true);
        }

        if (hit.gameObject.tag == "itemStar")
        {
            Destroy(hit.gameObject);
            StartCoroutine("starMode");
        }

    }
		
}
