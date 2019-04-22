using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    public float lifeTime;
    public ParticleSystem Explosion;
    // Use this for initialization
    void Start () {
        if (lifeTime == 0)
        {
            lifeTime = 3.5f;
        }

        Destroy(gameObject, lifeTime);
    }
	
	// Update is called once per frame
	void Update () {
       

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

        if (Explosion && collision.gameObject.tag != "Player")
         Instantiate(Explosion, transform.position, transform.rotation);
    } 
  }
