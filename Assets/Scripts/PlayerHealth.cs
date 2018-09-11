using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    //amount of times player can get hit by enemy before dying
    [SerializeField]
    int playerHealth = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //gameover condition, if health is lower than 0
        if (playerHealth <= 0)
            SceneManager.LoadScene("GameOver");
	}

    //decrease health if player is hit by enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ghost")
            playerHealth -= 1;
    }
}
