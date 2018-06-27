using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour {

    public GameObject[] obstaclePref1, obstaclePref2, obstaclePref3, obstaclePref4, environmentPref;
    GameObject obsTemp, coinTemp, environmentTemp;
    public GameObject coinPref;
    public int tileObstacleType, spaceType;
    // spacetype = 0left 1center 2right, tileobstype 0 = normaltile 1 = logtile else = noneObs;
    private float xValue;

    // Use this for initialization
    void Start () {

        xValue = 3.5f;

        if (tileObstacleType == 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 15f);
        }
        else if (tileObstacleType == 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 15f);
        }

        randomObs();
        randomCoin();
        randomEnvi();
    }

    private void randomObs()
    {
        int rand;
        if(tileObstacleType == 0)
        {
            rand = Random.Range(0, 5);
            getObs(rand);
        }
        else if (tileObstacleType == 1)
        {
            rand = Random.Range(2, 5);
            getObs(rand);
        }
    }

    private void getObs(int rand)
    {
        
        if(rand == 0)
        {
            obsTemp = Instantiate(obstaclePref1[0],transform.position,transform.rotation);
            spaceType = -1;
        }
        else if (rand == 1)
        {
            obsTemp = Instantiate(obstaclePref1[(int)Random.Range(1,4)], transform.position, transform.rotation);
            if ((int)(Random.Range(0, 2)) == 0)
            {
                obsTemp.transform.position = new Vector3(obsTemp.transform.position.x + 4, obsTemp.transform.position.y, obsTemp.transform.position.z);
                spaceType = 0;
            }
            else spaceType = 2;
        }
        else if (rand == 2)
        {
            rand = Random.Range(0, obstaclePref2.Length);
            obsTemp = Instantiate(obstaclePref2[rand], transform.position, transform.rotation);
            rand = Random.Range(-1, 2);
            obsTemp.transform.position = new Vector3(obsTemp.transform.position.x + (3.5f * rand), obsTemp.transform.position.y, obsTemp.transform.position.z);
            spaceType = (rand*-1) + 1;
        }
        else if (rand == 3)
        {
            rand = Random.Range(0, obstaclePref3.Length);
            obsTemp = Instantiate(obstaclePref3[rand], transform.position, transform.rotation);
            rand = Random.Range(0, 2);
            if(rand == 0) { rand = -1; }
            obsTemp.transform.position = new Vector3(obsTemp.transform.position.x + (3f * rand), obsTemp.transform.position.y, obsTemp.transform.position.z);
            if(rand == 1)
            {
                spaceType = 0;
            }
            else spaceType = 2;

        }
        else if (rand == 4)
        {
            rand = Random.Range(0, obstaclePref4.Length);
            obsTemp = Instantiate(obstaclePref4[rand], transform.position, transform.rotation);
            rand = Random.Range(-1, 2);
            obsTemp.transform.position = new Vector3(obsTemp.transform.position.x + (3.5f * rand), obsTemp.transform.position.y, obsTemp.transform.position.z);
            spaceType = (rand*-1) + 1;
        }
        else spaceType = -1;

        if (obsTemp)
            obsTemp.transform.SetParent(transform);
    }

    void randomCoin()
    {
        int rand = Random.Range(0, 4);
        float xPos = 0;
        if(rand != 0)
        {
            if(spaceType == -1)
            {
                xPos = getXPos((int)Random.Range(0,3));
                insCoin(xPos, 4.5f, 0);
            }
            else if (spaceType == 0)
            {
                xPos = -3.5f;
                insCoin(xPos, 1.5f, 0);
            }
            else if (spaceType == 1)
            {
                xPos = getXPos((int)Random.Range(0, 2));
                insCoin(xPos, 1.5f, 0);
            }
            else if (spaceType == 2)
            {
                xPos = 3.5f;
                insCoin(xPos, 1.5f, 0);
            }

            if (spaceType != -1 && tileObstacleType != -1)
            {
                if ((int)Random.Range(0, 4) != 0)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        insCoin(xPos, 1.5f, -5f);
                    }
                    else
                    {
                        insCoin(0, 1.5f, -5f);
                    }
                }

                if ((int)Random.Range(0, 4) != 0)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        insCoin(xPos, 1.5f, 5f);
                    }
                    else
                    {
                        insCoin(0, 1.5f, 5f);
                    }
                }
            }
        }
    }

    void insCoin(float x,float y,float z)
    {
        coinTemp = Instantiate(coinPref, transform.position, transform.rotation);
        coinTemp.transform.SetParent(transform);
        coinTemp.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
    }

    void randomEnvi()
    {
        if((int)(Random.Range(0, 3)) == 0)
        {
            environmentTemp = Instantiate(environmentPref[Random.Range(0, environmentPref.Length)]);
            environmentTemp.transform.SetParent(transform);
            environmentTemp.transform.position = new Vector3(transform.position.x + Random.Range(12f,15f), transform.position.y - 4.5f, transform.position.z + Random.Range(-12f, 12f));
        }
        if ((int)(Random.Range(0,3)) == 0)
        {
            environmentTemp = Instantiate(environmentPref[Random.Range(0, environmentPref.Length)]);
            environmentTemp.transform.SetParent(transform);
            environmentTemp.transform.position = new Vector3(transform.position.x + Random.Range(-12f, -15f), transform.position.y - 4.5f, transform.position.z + Random.Range(-12f, 12f));
        }
    }

    float getXPos(int rand)
    {
        float xPos = 0;
        if(rand == 0)
        {
            xPos = -3.5f;
        }
        else if(rand == 1)
        {
            xPos = 3.5f;
        }
        else if (rand == 2)
        {
            xPos = 0;
        }
        return xPos;
    }


}
