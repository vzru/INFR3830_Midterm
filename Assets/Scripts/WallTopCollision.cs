using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTopCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        BallMovement.yspeed = -BallMovement.yspeed;
        BallMovement.collflag = true;

    }
}
