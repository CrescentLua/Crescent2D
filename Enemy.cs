using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour {

    Rigidbody2D rb;
    float JumpPower;
    float walkspeed;
    float Health;
    float DetermineJump; 
    bool isFacingRight;
    bool isJumping;
    bool PlayerisNear;
    float FlingDirection;

    Vector3 Horizontal;

    public GameObject EnemyHealth;
    public ParticleSystem DeathExplosion;



    // Use this for initialization
    void Start () {
        Horizontal.x = gameObject.transform.position.x;
        isFacingRight = false;
        isJumping = false;

        JumpPower = 5.5f;
        walkspeed = 0.25f;
        Health = 100.0f; 

        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        EnemyHealth.GetComponent<TextMeshProUGUI>().text = " Health: " + Health.ToString();

        if (isFacingRight == false)
        {
            
            gameObject.transform.position += Horizontal * walkspeed * Time.deltaTime;
            JumpStart();
        }

        else if (isFacingRight == true)
        {
           
            gameObject.transform.position -= Horizontal * walkspeed * Time.deltaTime;
            JumpStart();
        }


        if (Health <= 0.0f)
        {
            Instantiate(DeathExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    void JumpStart()
    {
        if (isJumping == false)
        {
            isJumping = true; 
            StartCoroutine(Jump());
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBlocker")
        {
            isFacingRight = !isFacingRight;
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
        }

        if (collision.gameObject.tag == "Projectile")
        {

            if (Health > 0.0f)
            {
                Health = Health - 11.5f;
            }


            if (!isFacingRight)
            {
                FlingDirection = 3.5f;
            }

            else if (isFacingRight)
            {
                FlingDirection = -3.5f;
            }

            StartCoroutine(EnemyFling());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBlocker")
        {
            isFacingRight = !isFacingRight;
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
        }

        else if (collision.gameObject.tag == "Player")
        {
            PlayerisNear = true; 
        }
    }


    IEnumerator Jump()
    {
        DetermineJump = Random.Range(0, 10);
        yield return new WaitForSeconds(2.5f);

        if (DetermineJump > 5 || PlayerisNear == true)
        {
          rb.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
        }

        isJumping = false;
        PlayerisNear = false; 
    }

    IEnumerator EnemyFling()
    {
        rb.AddForce(new Vector2(FlingDirection, 0.5f), ForceMode2D.Impulse);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
