using System;
using UnityEngine;

public enum GhostState { Stunned, Spawn, Searching, Dead }

public class Ghost {

    protected GhostState _ghostState;

    public GhostState State
    {
        get { return _ghostState; }

        set
        {
            if(_ghostState != value)
            {
                this.Log(string.Format("{0} was: {1} now {2}", this.ToString(), _ghostState, value));
                _ghostState = value;
            }
        }
    }

    //constructor
    public Ghost()
    {
        this.State = GhostState.Spawn;
    }

    public virtual void Log(string v)
    {
        Debug.Log(v);
    }
}
