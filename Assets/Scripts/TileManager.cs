using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

	public GameObject[] tilePrefabs, tileLeft, tileMid, tileRight;
    private GameObject tileTmp;
    public GameObject tileControllerPref;

    private Transform playerTransform;
	private float SpawnZ = 150.0f;
	private float tileLength = 30f;
	private float safeZone = 30.0f;
	private int amnTilesonScreen = 4;
	private int lastPrefabIndex = 0;
    private int lastTileType;
    private int lastTileObstacleType;
    private int lastTileSpaceType;

    private List<GameObject> activeTiles;

	
	// Use this for initialization
	private void Start () {

        lastTileType = 1;

		activeTiles = new List<GameObject>();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

		for (int i = 0; i < amnTilesonScreen; i++)
		{
			if (i < 2){
				SpawnTile (0);
			}
			else {
				SpawnTile ();
			}
		}

	}
    

	// Update is called once per frame
	private void Update () {
		if (playerTransform.position.z - safeZone > (SpawnZ - amnTilesonScreen * tileLength))
		{
			SpawnTile();
			DeleteTile ();
		}
	}

	private void SpawnTile(int prefabIndex = -1) {
		GameObject go, tileController;

		if (prefabIndex == -1)
			go = Instantiate (randomTile())as GameObject;

		else
			go = Instantiate (tilePrefabs [prefabIndex])as GameObject;

		go.transform.SetParent (transform);
		go.transform.position = Vector3.forward * SpawnZ;

        tileController = Instantiate(tileControllerPref) as GameObject;
        tileController.transform.SetParent(go.transform);
        tileController.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 10.5f, go.transform.position.z - 5f);
        tileController.GetComponent<tileScript>().tileObstacleType = lastTileObstacleType;
        tileController.GetComponent<tileScript>().spaceType = lastTileSpaceType;

        SpawnZ += tileLength;
		activeTiles.Add (go);
        
	}

    private GameObject randomTile()
    {
        int rand = 0;
        if(lastTileType == 1)
        {
            rand = Random.Range(0, 3);
            if(rand == 0)
            {
                tileTmp = tileMid[0];
                lastTileType = 1;
            }
            else
            {
                rand = Random.Range(0, 3);
                tileTmp = getTile(rand);
            }
        }
        else if(lastTileType == 0)
        {
            rand = Random.Range(0, 3);
            if (rand == 0)
            {
                tileTmp = tileMid[0];
                lastTileType = 1;
            }
            else
            {
                tileTmp = getTile(0);
            }
        }
        else if (lastTileType == 2)
        {
            rand = Random.Range(0, 3);
            if (rand == 0)
            {
                tileTmp = tileMid[0];
                lastTileType = 1;
            }
            else
            {
                tileTmp = getTile(2);
            }
        }

        checkTileObsatacleType();

        return tileTmp;
        //return tileMid[0];

    }

    private GameObject getTile(int rand)
    {
        if(rand == 1)
        {
            rand = Random.Range(0, tileMid.Length);
            lastTileType = 1;
            return tileMid[rand];
        }
        else if(rand == 0)
        {
            rand = Random.Range(0, tileLeft.Length);
            if(rand <= 1) { lastTileType = 0; }
            else { lastTileType = 1; }
            return tileLeft[rand];
        }
        else if (rand == 2)
        {
            rand = Random.Range(0, tileRight.Length);
            if (rand <= 1) { lastTileType = 2; }
            else { lastTileType = 1; }
            return tileRight[rand];
        }

        else return tileMid[0];
    }

    private void checkTileObsatacleType()
    {
        if (tileTmp == tileMid[0])
        {
            lastTileObstacleType = 0;
        }
        else if(tileTmp == tileMid[2] || tileTmp == tileMid[3] || tileTmp == tileMid[4] || tileTmp == tileMid[5] || tileTmp == tileLeft[3] || tileTmp == tileRight[3])
        {
            lastTileObstacleType = 1;
        }
        else
        {
            lastTileObstacleType = -1;
            for (int i = 0; i < 3; i++)
            {
                if (tileTmp == tileLeft[i])
                {
                    lastTileSpaceType = 0;
                }
                else if(tileTmp == tileRight[i])
                {
                    lastTileSpaceType = 2;
                }
            }
        }
    }

	private void DeleteTile() {
		Destroy (activeTiles [0]);
		activeTiles.RemoveAt (0);
	}

	private int RandomPrefabIndex() {
		if (tilePrefabs.Length <= 1)
			return 0;

		int randomIndex = lastPrefabIndex;
		while (randomIndex == lastPrefabIndex) {
			randomIndex = Random.Range (0,tilePrefabs.Length);
		}

		lastPrefabIndex = randomIndex;
		return randomIndex;
	}
    
}
 