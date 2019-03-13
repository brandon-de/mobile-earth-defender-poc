using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public float speed;
    public float lifeTime;

    private Vector3 stored_pos;

    public void SetBulletTarget(Transform target)
    {
        this.stored_pos = new Vector3(target.position.x, target.position.y, target.position.z);
    }
    
	// Use this for initialization
	void Start () {
        Invoke("DestroyBullet", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        
        //transform.Translate(transform.up * speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, this.stored_pos, Time.deltaTime * speed);
	}

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
