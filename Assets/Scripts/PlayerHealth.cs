using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    //amount of times player can get hit by enemy before dying
    [SerializeField]
    int playerHealth = 3;

    //healthbar object
    public GameObject healthBar;

    //healthbar animation
    Animator healthAni;

    //Hit cool down
    [SerializeField]
    float coolDown=10;

    //bool for making the character invincible
    bool isInvincible;

    // Use this for initialization
    void Start () {
        //set health at start/restart of game/level
        if (StatManager.health <= 0)
            StatManager.health = playerHealth;

        //get animation component
        healthAni = healthBar.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        healthAni.SetInteger("Health", StatManager.health);

    }

    //decrease health if player is hit by enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //decrease health
        if (collision.gameObject.tag == "Ghost")
        {
            if(isInvincible==false)
            {
                StatManager.health -= 1;
                StartCoroutine(Invincible());
            }
            Debug.Log(StatManager.health);
            if (StatManager.health <= 0)
            {
                SceneManagement.GameOver();
            }
        }          
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

    //Makes the character invincible for a specific amount of seconds when called
    IEnumerator Invincible()
    {

        isInvincible = true;
        Debug.Log("True");
        yield return new WaitForSeconds(coolDown);
        Debug.Log("false");
        isInvincible = false;
    }

}
