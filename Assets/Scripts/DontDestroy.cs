using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

   public AudioSource source;
    public AudioClip clip;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        source.PlayOneShot(clip,.5f);
    }
	
	// Update is called once per frame
	void Update () {
		if(!source.isPlaying)
        {
            source.PlayOneShot(clip,.5f);
        }
	}
}
