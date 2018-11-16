using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    [SerializeField]
    Text tutText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool HasMoved = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Safe")
        {
                tutText.text = "This is the safe room.\n" +
                    "It is denoted by the white and black checkered pattern.\n" +
                    "\n Use w,s,a,d to move.";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Safe")
        {
            tutText.text = "Follow the arrow to find the key";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="key")
        {
            tutText.text = "Now follow the arrow to find \nthe stairs to the next level";
        }
        if (collision.gameObject.tag == "stair")
        {
            SceneManagement.TaskOnClick5();
        }
    }
}
