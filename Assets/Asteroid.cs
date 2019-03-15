using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Speed = -5;

	// Use this for initialization
	void Start ()
	{
	    var r2d = GetComponent<Rigidbody2D>();

	    // Add a vertical speed to the enemy
	    r2d.velocity = new Vector3(r2d.velocity.x, Speed);

	    // Make the enemy rotate on itself
	    r2d.angularVelocity = Random.Range(-200, 200);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
