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

    //keyUI object
    public GameObject keyUI;

    //KeyUI animation
    Animator keyAni;

    //Hit cool down
    [SerializeField]
    float coolDown=10;

    //bool for making the character invincible
    bool isInvincible;

    //Bool for safe room
    public static bool safe;

    // Use this for initialization
    void Start () {
        //set health at start/restart of game/level
        if (StatManager.health <= 0)
            StatManager.health = playerHealth;

        //get animation component
        healthAni = healthBar.GetComponent<Animator>();

        //get animation component
        keyAni = keyUI.GetComponent<Animator>();

        healthAni.SetInteger("Health", StatManager.health);
    }
	
	// Update is called once per frame
	void Update ()
    {
        healthAni.SetInteger("Health", StatManager.health);
        keyAni.SetBool("HasKey", StatManager.hasKey);
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
            {
                SceneManagement.LevelChange();
                StatManager.hasKey = false;
            }
        }
        else if (collision.gameObject.tag == "Exit")
        {
            if (StatManager.hasKey)
                SceneManagement.LevelChange();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Safe")
        {
            safe = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Safe")
        {
            safe = false;
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
