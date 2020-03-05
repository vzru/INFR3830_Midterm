using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject paddle1;
    public GameObject paddle2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Paddle1 Position: " + paddle1.transform.position.x);
        Debug.Log("Paddle2 Position: " + paddle2.transform.position.x);

        ball.GetComponent<BallMovement>();
        //Debug.Log("Ball Velocity: " + ball.xspeed + ", " + ball.yspeed);
        
    }
}
