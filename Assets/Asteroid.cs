using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Speed = .3f;
    public int Health = 3;
    private bool _isFlashFromHit = false;
    private float _isFlashFromHitTimer = .2f;
    private float _isFlashFromHitCurrentTimer = 0f;    

	// Use this for initialization
	void Start ()
	{
	    var r2d = GetComponent<Rigidbody2D>();

	    // Add a vertical speed to the enemy
	    r2d.velocity = new Vector3(r2d.velocity.x, Speed);

	    // Make the enemy rotate on itself
	    //r2d.angularVelocity = Random.Range(-200, 200);
	}
	
	// Update is called once per frame
	void Update () {

        if(_isFlashFromHit)
        {
            _isFlashFromHitCurrentTimer += Time.deltaTime;

            if(_isFlashFromHitCurrentTimer >= _isFlashFromHitTimer)
            {
                _isFlashFromHit = false;
                _isFlashFromHitCurrentTimer = 0f;

                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.black;
            }
        }

        if(this.Health <= 0)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        if(!_isFlashFromHit)
        {
            _isFlashFromHit = true;
        }

        Debug.Log("Current Health " + this.Health);

        this.Health = this.Health - 1;
    }
}
