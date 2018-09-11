using UnityEngine;


public class UnityGhost : MonoBehaviour {

    //Reference of Ghost State
    public Ghost _ghost;

    //Reference of Basic AI code
    public SteeringBehaviors _behaviors;
    private MovingAgent _agent;

    public float MaxSpeed;
    public float MaxForce;
    public float Mass;

    //maybe these can be retrieved from the rigidbody?
    public Vector2 Direction;
    public Vector2 Location;
    public Vector2 Heading;

    

    // Use this for initialization
    void Start () {
        _agent = new MovingAgent(this, _behaviors, Vector2.down);
        this.Location = this.transform.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
        _agent.UpdateForces();

        this.Heading = this.Direction.normalized;
        this.transform.position = this.Location;

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
    }
}
