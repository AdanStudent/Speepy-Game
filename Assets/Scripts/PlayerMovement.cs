using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    //how fast the player moves
    [SerializeField]
    float speed = 10.0f;

    //where the flashlight can move on the x-axis
    [SerializeField]
    float flashlightXConstraint = .5f;

    //where the flashlight can move on the y-axis
    [SerializeField]
    float flashlightYConstraint = 1.3f;

    //flashlight object
    public GameObject flashLight;

    //the position of the mouse cursor on the screen
    private Vector3 mousePos;

    
    // Update is called once per frame
    void FixedUpdate()
    {
        //set input information
        float translationX = Input.GetAxis("Horizontal") ;
        float translationY = Input.GetAxis("Vertical") ;

        //set new position
        GetComponent<Rigidbody2D>().velocity = new Vector2(translationX * (speed * Time.deltaTime), translationY * (speed * Time.deltaTime));
        GetComponent<Rigidbody2D>().AddForce(new Vector2(translationX*speed,translationY*speed));

        //get the mouse postion on the screen
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //restrict flashlight movement to front of player
        mousePos.x = Mathf.Clamp(mousePos.x, transform.position.x- flashlightXConstraint, transform.position.x +flashlightXConstraint);
        mousePos.y = Mathf.Clamp(mousePos.y, transform.position.y - flashlightYConstraint, transform.position.y - flashlightYConstraint);

        //move flashlight position
        flashLight.transform.position = Vector2.Lerp(flashLight.transform.position, mousePos, speed);

    }


}
