using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRelaunch : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = new Vector3(0, -7, -1);
        BallMovement.xspeed = 8.0f;
        BallMovement.yspeed = -8.0f;
        BallMovement.launchTimer = 1.0f;
    }
}
