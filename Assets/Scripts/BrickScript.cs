using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(BallMovement.collflag == true)
        {
            BallMovement.yspeed = -BallMovement.yspeed;
            BallMovement.collflag = false;
            Destroy(gameObject);
        }
    }
}
