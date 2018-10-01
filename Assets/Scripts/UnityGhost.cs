using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class UnityGhost : MonoBehaviour {

    //Reference of Ghost State
    public Ghost _ghost;

    //Reference of Basic AI code
    public SteeringBehaviors _behaviors;
    private MovingAgent _agent;

    //public accessors for the AI calculations
    public float MaxSpeed;
    public float MaxForce;
    public float Mass;

    public Vector2 Direction;
    public Vector2 Location;
    public Vector2 Heading;

    public Vector2 _homeLocation = Random.insideUnitCircle * 3;


    // Use this for initialization
    void Start () {
        _agent = new MovingAgent(this, _behaviors, _homeLocation);
        this.Location = this.transform.position;
        
        //Setting the AI to start in Wander when loaded
        UpdateAIBehavior(SteeringBehaviors.Wander);

        //Gets components for the AI
        this._agent.RunOnStart();

	}
	
	// Update is called once per frame
	void Update ()
    {
        
        //Updates AI's calculations
        _agent.UpdateForces();
        this.MaxSpeed = 2;
        //Updates Heading as it is needed for AI calculations for next Update frame
        this.Heading = this.Direction.normalized;

        //Updates Location based on AI calculations
        this.transform.position = this.Location;

        //During gameplay will update as player comes into view vs just wandering
        UpdateGhostState();
	}
    //to be called by the manager passing in a list of other ghosts
    public void UnityGhostSeparation(List<UnityGhost> ghosts)
    {
        //this will pass the list to the MovingAgent where the actual calculations will be done
        _agent.AddGhostsReference(ghosts);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(this.transform.position, this.Heading);
    }


    //used to update Ghost's behavior based on it's state
    private void UpdateGhostState()
    {

        //If player is in "view" start chasing
        if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 5)
        {
            UpdateAIBehavior(SteeringBehaviors.Pursuit);
            this.MaxSpeed *= 2;
        }

        //If wanders too far off screen go back home
        if (Vector3.Distance(this.transform.position, this._homeLocation) > 20)
        {
            UpdateAIBehavior(SteeringBehaviors.Seek);
            this.MaxSpeed *= 2;
        }

        //If stunned countdownTimer until back to Wander State
        if (isStunned)
        {
            _stunnedTimer -= Time.deltaTime;

            if (_stunnedTimer < 0)
            {
                isStunned = false;
                this.transform.position = new Vector3(0, 0, 0);
            }
        }
        else if(!isStunned && this._behaviors != SteeringBehaviors.Pursuit)
        {
            UpdateAIBehavior(SteeringBehaviors.Wander);
        }
    }

    float _stunnedTimer;
    bool isStunned;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Light")
        {
            Debug.Log("light");
            UpdateAIBehavior(SteeringBehaviors.None);

            //Start countdown timer
            _stunnedTimer = 5f;
            isStunned = true;
        }
        else if(collision.gameObject.tag == "Player")
        {
            this.transform.position = _homeLocation;
            UpdateAIBehavior(SteeringBehaviors.Seek);
        }

    }

    private void UpdateAIBehavior(SteeringBehaviors steeringBehaviors)
    {
        this._behaviors = steeringBehaviors;
    }
}
