using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Camera main;

    private void Start()
    {
        main = GetComponent<Camera>();
        main.aspect=(5f/4f);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        offset.z = -2;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
