using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class UnityGhost : MonoBehaviour {

    //Reference of Ghost State
    public Ghost _ghost;

    //Reference of Basic AI code
    public SteeringBehaviors _behaviors;
    private MovingAgent _agent;

    public float MaxSpeed;
    public float MaxForce;
    public float Mass;

    public Vector2 Direction;
    public Vector2 Location;
    public Vector2 Heading;

    

    // Use this for initialization
    void Start () {
        _agent = new MovingAgent(this, _behaviors, Vector2.down);
        this.Location = this.transform.position;

        this._agent.RunOnStart();

	}
	
	// Update is called once per frame
	void Update ()
    {
        _agent.UpdateForces();

        //Updates Heading as it is needed for AI calculations
        this.Heading = this.Direction.normalized;
        //Updates Location based on AI calculations
        this.transform.position = this.Location;

        //During gameplay will update as player comes into view vs just wandering
        UpdateGhostState();
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(this.transform.position, this.Heading);
    }


    //used to update Ghost's behavior based on it's state
    private void UpdateGhostState()
    {
        //Start with Wander state

        //If player is in "view" start chasing

        //If stunned stop movement

        //If wanders too far off screen go back home
    }
}
