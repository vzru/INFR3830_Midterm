using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBricksScript : MonoBehaviour {

	// Use this for initialization
	void Start () {


		for(float y = 0; y < 8; y++)//number of rows
        {
            for(float x = 0; x < 15; x++)//number of columns
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x * 2f - 14f, y - 1f, -1f);
                cube.transform.localScale = new Vector3(1.9f, 0.9f, 1f);
                cube.AddComponent<BrickScript>();
                cube.GetComponent<Collider>().isTrigger = true;

                //cube colours:
                if (y < 2)
                    cube.GetComponent<Renderer>().material.color = Color.yellow;
                else if (y < 4)
                    cube.GetComponent<Renderer>().material.color = Color.cyan;
                else if (y < 6)
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                else
                    cube.GetComponent<Renderer>().material.color = Color.red;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
