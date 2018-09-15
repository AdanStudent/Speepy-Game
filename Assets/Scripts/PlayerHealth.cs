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
        //set health at start/restart of game/level
        if (StatManager.health <= 0)
            StatManager.health = playerHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    //decrease health if player is hit by enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //decrease health
        if (collision.gameObject.tag == "ghost")
            StatManager.health -= 1;
        //pickup key object
        else if (collision.gameObject.tag == "key")
        {
            StatManager.hasKey = true;
            Destroy(collision.gameObject); 
        }
        //go to next level if player has key
        else if (collision.gameObject.tag == "stair")
        {
            if (StatManager.hasKey)
                SceneManagement.LevelChange();
        }
    }
}
