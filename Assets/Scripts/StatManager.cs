using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour {

    //current level the player is on
    public static int level { get; set; }

    //current health of player
    public static int health { get; set; }

    //player must have a key to complete level
    public static bool hasKey;

  

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        hasKey = false;
       
	}
	
	// Update is called once per frame
	void Update () {


	}

}
