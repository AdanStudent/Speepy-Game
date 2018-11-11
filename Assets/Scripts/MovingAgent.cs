using System;
using System.Collections.Generic;
using UnityEngine;

//based on the code from this project
//https://github.com/shiffman/The-Nature-of-Code-Examples-p5.js/tree/master/chp06_agents


//used for the differents types of behaviors the Moving Agent will enact
public enum SteeringBehaviors { None, Seek, Arrive, Pursuit, Wander}

//used for the Arrive calculations
enum Deceleration { Fast = 1, Normal, Slow}


class MovingAgent
{

    public void RunOnStart()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerRB2D = _player.GetComponent<Rigidbody2D>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
    }


    //reference for the agent it will be directing
    private UnityGhost _unityGhost;

    //probably will need a reference of the Player
    private GameObject _player;
    private Rigidbody2D _playerRB2D;
    private PlayerMovement _playerMovement;

    private List<UnityGhost> otherGhosts;

    private Vector2 _steeringForce;
    private Vector2 _acceleration;
    private Vector2 _homeLoc;

    public MovingAgent(UnityGhost UNITYGHOST, SteeringBehaviors agentBehaviors, Vector2 HomeLocation)
    {
        this._unityGhost = UNITYGHOST;
        this._unityGhost.MaxSpeed = 2.0f;
        this._unityGhost.Mass = 5f;
        this._unityGhost.MaxForce = 0.5f;
        this._unityGhost.Direction = new Vector2(0, 0);
        this._homeLoc = HomeLocation;

    }

    public void AddGhostsReference(List<UnityGhost> ghosts)
    {
        otherGhosts = ghosts;
    }

    #region SteeringBehaviors

    private Vector2 BehaviorSwitch()
    {
        this._unityGhost.gameObject.tag = "Ghost";

        switch (this._unityGhost._behaviors)
        {
            case SteeringBehaviors.None:
                break;

            case SteeringBehaviors.Seek:
                this._unityGhost.gameObject.tag = "Untagged";
                this._homeLoc = this._player.transform.position * -1;
                _steeringForce = Seek(_homeLoc);
                break;

            case SteeringBehaviors.Arrive:
                _steeringForce = Arrive(_homeLoc, Deceleration.Slow);
                break;

            case SteeringBehaviors.Pursuit:
                if (this._player != null)
                {
                    _steeringForce = Separation() + Pursuit(this._player);
                }   
                break;

            case SteeringBehaviors.Wander:
                _steeringForce = Separation() + Wander();
                break;

            default:
                break;
        }

        return this._steeringForce;
    }

    private Vector2 Separation()
    {
        float desiredSeparation = .75f * 2; // how much the agents will be separated by
        Vector2 sum = new Vector2(); //a place holder for the sum of the other agents
        int count = 0; //keeps track of how many for some math later

        //check each agent
        foreach (var g in otherGhosts)
        {
            //checks their distance from this to other
            float dist = Vector2.Distance(this._unityGhost.Location, g.Location);

            //Checks that the distance is greater than 0 to avoid self
            //and if its closer to agent their wanted
            if ((dist > 0) && (dist < desiredSeparation))
            {
                var difference = this._unityGhost.Location - g.Location; //subtract their locations
                difference = difference.normalized; //normalize the calculation from above
                difference /= dist; // divide by distance 
                sum += difference; // add that to sum
                count++; //update counter
            }
        }

        Vector2 force = new Vector2();
        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized;
            sum *= this._unityGhost.MaxSpeed;
            force = sum - this._unityGhost.Direction;
        }

        return force;
    }

    private Vector2 Seek(Vector2 targetPosition)
    {
        Vector2 desiredVelocity = Vector3.Normalize(targetPosition - this._unityGhost.Location)
                                            * this._unityGhost.MaxSpeed;

        return (desiredVelocity - _unityGhost.Direction);
    }

    private Vector2 Arrive(Vector2 targetPosition, Deceleration decel)
    {
        Vector2 ToTarget = targetPosition - this._unityGhost.Location;

        float dist = ToTarget.magnitude;

        if (dist > 0)
        {
            const float DecelerationTweaker = 0.3f;

            float speed = dist / ((float)decel * DecelerationTweaker);
            speed = (speed < this._unityGhost.MaxSpeed) ? speed : this._unityGhost.MaxSpeed;

            Vector2 desiredVelocity = ToTarget * speed / dist;
            return (desiredVelocity - this._unityGhost.Direction);
        }
        return Vector2.zero;
    }

    private Vector2 Pursuit(GameObject evader)
    {
        Vector2 ToEvader = evader.transform.position - this._unityGhost.transform.position;

                                                                        //gets player heading
        float RelativeHeading = Vector2.Dot(this._unityGhost.Heading, this._playerRB2D.velocity.normalized);
    
        if (((Vector2.Dot(ToEvader, this._unityGhost.Heading) > 0)) && (RelativeHeading < -.95))
        {
            return Seek(evader.transform.position);
        }

        float LookAheadTime = (ToEvader.magnitude / (this._unityGhost.MaxSpeed + _playerMovement.Speed()));
        return Seek(evader.transform.position + (Vector3)(_playerRB2D.velocity * LookAheadTime));
    }

    float wanderTheta;
    private Vector2 Wander()
    {
        float wanderRadius = 50;
        float wanderDist = 80;
        float change = 0.3f;

        wanderTheta += UnityEngine.Random.Range(-change, change);
        this._unityGhost.Heading = this._unityGhost.Direction.normalized;

        Vector2 circlePos = this._unityGhost.Heading;
        circlePos *= wanderDist;
        circlePos += this._unityGhost.Location;
        float h = Vector2.Angle(Vector2.zero, this._unityGhost.Heading);

        Vector2 circleOffset = new Vector2(wanderRadius * Mathf.Cos(wanderTheta + h),
                                            wanderRadius * Mathf.Sin(wanderTheta + h));

        Vector2 target = circlePos + circleOffset;

            return Seek(target);
    }
    #endregion

    #region SteeringCalculations

    float time;
    internal void UpdateForces()
    {
        time = Time.deltaTime;

        ApplyForce(BehaviorSwitch());

        if (this._steeringForce.magnitude > 0.01f)
        {

            this._acceleration = _steeringForce / this._unityGhost.Mass;
            this._unityGhost.Direction += this._acceleration * (time);

            this.Truncate(ref this._unityGhost.Direction, this._unityGhost.MaxSpeed);
            Vector3.ClampMagnitude(this._unityGhost.Direction, UnityEngine.Random.Range(3, 15));
            this._unityGhost.Location += this._unityGhost.Direction * (time);

            if (this._unityGhost.Direction.SqrMagnitude() > 0.0001)
            {
                this._unityGhost.Heading = Vector3.Normalize(this._unityGhost.Direction);

                //also figure out the side Vector of the agent
            }
        }

        this._steeringForce = Vector2.zero;
    }

    void ApplyForce(Vector2 force)
    {
        if (!SumForces(force))
        {
            this._steeringForce += force;
        }
    }

    private bool SumForces(Vector2 ForceToAdd)
    {
        float MagSoFar = this._steeringForce.magnitude;

        float MagRemaining = this._unityGhost.MaxForce - MagSoFar;

        if (MagRemaining <= 0)
        {
            return false;
        }

        float MagToAdd = ForceToAdd.magnitude;

        if (MagToAdd < MagRemaining)
        {
            this._steeringForce += ForceToAdd;
        }
        else
        {
            this._steeringForce = this._steeringForce + (Vector2)(Vector3.Normalize(ForceToAdd) * MagRemaining);
        }
        return true;
    }

    #endregion

    public void Truncate(ref Vector2 original, float max)
    {
        if (original.magnitude > max)
        {
            original.Normalize();

            original *= max;
        }
    }


}

