using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//used for the differents types of behaviors the Moving Agent will enact
public enum SteeringBehaviors { None, Seek, Arrive, Pursuit, Wander}

//used for the Arrive calculations
enum Deceleration { Fast = 1, Normal, Slow}


class MovingAgent
{
    //reference for the agent it will be directing
    private UnityGhost _unityGhost;

    //probably will need a reference of the Player
    //private Player _player;


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

    #region SteeringBehaviors

    private Vector2 BehaviorSwitch()
    {
        switch (this._unityGhost._behaviors)
        {
            case SteeringBehaviors.None:
                break;

            case SteeringBehaviors.Seek:
                _steeringForce = Seek(_homeLoc);
                break;

            case SteeringBehaviors.Arrive:
                _steeringForce = Arrive(_homeLoc, Deceleration.Slow);
                break;

            //case Behaviors.Pursuit:
            //    if (BasicAgent1 != null)
            //        steeringForce = Pursuit(this.BasicAgent1);
            //    break;

            case SteeringBehaviors.Wander:
                _steeringForce = Wander();
                break;

            default:
                break;
        }

        return this._steeringForce;
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

    //private Vector2 Pursuit(Player evader)
    //{
    //    Vector2 ToEvader = evader.Location - this.agent.Location;

    //    float RelativeHeading = Vector2.Dot(this.agent.Heading, evader.Heading);

    //    if (((Vector2.Dot(ToEvader, this.agent.Heading) > 0)) && (RelativeHeading < -.95))
    //    {
    //        return Seek(evader.Location);
    //    }

    //    float LookAheadTime = ToEvader.magnitude / (this.agent.MaxSpeed + evader.Speed);
    //    return Seek(evader.Location + evader.Direction * LookAheadTime);
    //}

    //private Vector2 Evade(BasicAgent pursuer)
    //{
    //    Vector2 ToPursuer = pursuer.Location - this.agent.Location;

    //    float LookAheadTime = ToPursuer.magnitude / (this.agent.MaxSpeed + pursuer.Speed);

    //    return Flee(pursuer.Location + pursuer.Direction * LookAheadTime);
    //}

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


        //Vector3 unitCircleWander = UnityEngine.Random.insideUnitCircle.normalized;
        //unitCircleWander.x = Mathf.Clamp(unitCircleWander.x, -1.6f, 1.6f);
        //unitCircleWander.y = Mathf.Clamp(unitCircleWander.y, -1, 1f);

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
            Debug.Log(this._steeringForce);
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

