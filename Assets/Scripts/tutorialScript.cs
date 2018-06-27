using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialScript : MonoBehaviour {

    public GameObject tutorialPanel, turn, jump, player;

    void Start()
    {
        tutorialPanel.SetActive(true);
        turn.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > 54)
        {
            if (turn)
                Destroy(turn);
            jump.SetActive(true);
        }

        if (player.transform.position.z > 110)
        {
            if (turn)
                Destroy(turn);
            Destroy(jump);
            Destroy(tutorialPanel);
            Destroy(gameObject);
        }
        if (player.GetComponent<playerController>().isDead)
        {
            if (turn)
                Destroy(turn);
            Destroy(jump);
            Destroy(tutorialPanel);
            Destroy(gameObject);
        }
    }
}
