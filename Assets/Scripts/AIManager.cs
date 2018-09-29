﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public int ghostCount;
    public GameObject prefab;
    public List<GameObject> ghosts;

	// Use this for initialization
	void Start () {
        //create ghosts needed in the scence
        for (int i = 0; i < ghostCount; i++)
        {
            ghosts.Add(Instantiate(prefab,
                new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f), 0), 
                Quaternion.identity, this.transform));
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerHealth.safe)
        {
            RunAway();
        }
	}

    private void RunAway()
    {
        for (int i = 0; i < ghostCount; i++)
        {
            ghosts[i].GetComponent<UnityGhost>()._behaviors = SteeringBehaviors.Seek;
        }
    }
}
