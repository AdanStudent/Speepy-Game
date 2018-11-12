using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMovement : MonoBehaviour {

    public GameObject ghost;

    private Vector2 startLocation;
	// Use this for initialization
	void Start () {
        startLocation = transform.position;
	}

    bool top = false;
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().velocity = new Vector2((100 * Time.deltaTime), 0);

        ghost.transform.position = new Vector2(this.transform.position.x-2, this.transform.position.y);

        if(transform.position.x>=Camera.main.rect.width+8)
        {
            if (top)
            {
                transform.position = startLocation;
                top = false;
            }
            else
            {
                transform.position = new Vector2(startLocation.x, startLocation.y-7.5f);
                top = true;
            }
        }
        
    }
}
