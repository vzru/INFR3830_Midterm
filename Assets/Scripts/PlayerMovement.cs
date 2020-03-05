using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-20f * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(20f * Time.deltaTime, 0, 0);
        }

        float h = 30.0f * Time.deltaTime * Input.GetAxis("Mouse X");
        transform.Translate(h, 0, 0);

        if(transform.position.x > 13)
        {
            transform.position = new Vector3(13, -15, 0);
        }
        else if (transform.position.x < -13)
        {
            transform.position = new Vector3(-13, -15, 0);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        BallMovement.yspeed = -BallMovement.yspeed;
        //check to see if the ball is on the right side of the paddle if it is then the x speed
        //component of the ball is set to positive, otherwise it's set to negative
        if(other.gameObject.transform.position.x > gameObject.transform.position.x)
        {
            BallMovement.xspeed = Mathf.Abs(BallMovement.xspeed);
        }
        else
        {
            BallMovement.xspeed = - Mathf.Abs(BallMovement.xspeed);
        }

        BallMovement.collflag = true;
    }
}
