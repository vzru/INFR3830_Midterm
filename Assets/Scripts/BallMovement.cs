using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public static float launchTimer = 2.0f;
    public static float xspeed = 8.0f;
    public static float yspeed = 8.0f;
    public static bool collflag = true;

	
	// Update is called once per frame
	void Update () {

        launchTimer -= Time.deltaTime;
        if(launchTimer <= 0)
        {
            transform.Translate(new Vector3(xspeed, yspeed, 0) * Time.deltaTime);
            launchTimer = 0.0f;
        }
	}
}
