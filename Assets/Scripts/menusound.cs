using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menusound : MonoBehaviour {

    private GameObject gameSound;
    public AudioSource source;
    public AudioClip clip;


    // Use this for initialization
    void Start () {
        gameSound = GameObject.FindGameObjectWithTag("GameSound");
        if(gameSound!=null)
        {
            Destroy(gameSound);
        }
        source.PlayOneShot(clip);
    }
	
	// Update is called once per frame
	void Update () {
        if (!source.isPlaying)
        {
            source.PlayOneShot(clip);
        }
    }
}
