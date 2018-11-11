using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour {

    GameObject Key;
    GameObject Exit;
    GameObject FinalExit;
    float Speed = 5f;

    // Use this for initialization
    void Start () {
        Key = GameObject.FindGameObjectWithTag("key");
        Exit = GameObject.FindGameObjectWithTag("stair");
        FinalExit = GameObject.FindGameObjectWithTag("Exit");

    }

    private void Update()
    {
        if (Key != null)
        {
            LookTowardsItem(Key.transform);
        }
        else if (Exit != null)
        {
            LookTowardsItem(Exit.transform);
        }
        else
        {
            LookTowardsItem(FinalExit.transform);

        }
    }

    private void LookTowardsItem(Transform item)
    {
        Vector2 direction = item.position - transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 45;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Speed * Time.deltaTime);
    }
}
