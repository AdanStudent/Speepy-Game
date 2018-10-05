using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //how fast the player moves
    [SerializeField]
    float speed = 10.0f;

    public float Speed()
    {
        return speed;
    }

    //where the flashlight can move on the x-axis
    [SerializeField]
    float flashlightXConstraint = .5f;

    //where the flashlight can move on the y-axis
    [SerializeField]
    float flashlightYConstraint = 1.3f;

    //flashlight object
    public GameObject flashLight;

    //Animator object
    Animator animator;

    //player's sprite renderer
    SpriteRenderer flip;

    //the current direction of player

    public DIRECTIONS CurrentDirection { get; set; }

    public enum DIRECTIONS { Up, Right, Down, Left };

    //the player's current position coordinates
    Vector2 lastPos;

    //Use this for intialization 
    void Start()
    {
        //get the player object's animator component
        animator = GetComponent<Animator>();

        //get the player object's sprite renderer component
        flip = GetComponent<SpriteRenderer>();

        //starting direction
        CurrentDirection = DIRECTIONS.Down;

        lastPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set input information
        float translationX = Input.GetAxis("Horizontal") ;
        float translationY = Input.GetAxis("Vertical") ;

        // Set facing direction
        SetDirection(translationX, translationY);

        //set new position
        GetComponent<Rigidbody2D>().velocity = new Vector2(translationX * (speed * Time.deltaTime), translationY * (speed * Time.deltaTime));
        GetComponent<Rigidbody2D>().AddForce(new Vector2(translationX*speed,translationY*speed));
        //Freeze rotation so that the player object doesn't spin after hitting another collider
        GetComponent<Rigidbody2D>().freezeRotation=true;

        //restrict flashlight movement to front of player
        SetFlashlightDirection();

        //move flashlight position
        //flashLight.transform.position = Vector2.Lerp(flashLight.transform.position, transform.position, speed);
       
    }

    //set the direction that the player object is facing
    private void SetDirection(float x, float y)
    {
        //set the current direction of player
        if (x < 0)
        {
            CurrentDirection = DIRECTIONS.Left;
            flip.flipX = true;
        }
        else if (x > 0)
        {
            CurrentDirection = DIRECTIONS.Right;
            flip.flipX = false;
        }
        else if (y < 0)
        {
            CurrentDirection = DIRECTIONS.Down;
            flip.flipX = false;
        }
        else if (y > 0)
        {
            CurrentDirection = DIRECTIONS.Up;
            flip.flipX = false;
        }

        //check to see if player is currently moving
        CheckIsMoving();
        //set direction
        animator.SetInteger("direction", (int)CurrentDirection);

      
    }

    //checks to see if the player is moving
    private void CheckIsMoving()
    {
        Vector2 currentPos = transform.position;
        //check to see if currently moving
        if (lastPos.Equals(currentPos))
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        lastPos = currentPos;
    }

    //change the flashlight restrictions to always be in front of player no matter what direction
    private void SetFlashlightDirection()
    {
        if(CurrentDirection.CompareTo(DIRECTIONS.Down)==0)
        {
            //flip the flashlight to face down
            flashLight.transform.localRotation = Quaternion.Euler(0, -180, 0);
            //move flashlight position
            flashLight.transform.position = Vector2.Lerp(flashLight.transform.position, new Vector2(transform.position.x, transform.position.y - flashlightYConstraint), speed);
        }
        else if (CurrentDirection.CompareTo(DIRECTIONS.Up) == 0)
        {
            
            //flip the flashlight to face up
            flashLight.transform.localRotation=Quaternion.Euler(180, 0, 0);
            //move flashlight position
            flashLight.transform.position = Vector2.Lerp(flashLight.transform.position, new Vector2(transform.position.x, transform.position.y+flashlightYConstraint), speed);
        }
        else if (CurrentDirection.CompareTo(DIRECTIONS.Right) == 0)
        {
            //flip the flashlight to face right
            flashLight.transform.localRotation = Quaternion.Euler(0, 0,90);
            //restrict flashlight movement to front of player
            //move flashlight position
            flashLight.transform.position = Vector2.Lerp(flashLight.transform.position, new Vector2(transform.position.x + flashlightXConstraint, transform.position.y), speed);

        }
        else if (CurrentDirection.CompareTo(DIRECTIONS.Left) == 0)
        {
            //flip the flashlight to face left
            flashLight.transform.localRotation = Quaternion.Euler(0, 0, -90);
            //restrict flashlight movement to front of player

            //move flashlight position
            flashLight.transform.position = Vector2.Lerp(flashLight.transform.position, new Vector2(transform.position.x-flashlightXConstraint, transform.position.y), speed);
        }
    }

}
