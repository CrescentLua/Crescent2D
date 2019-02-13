using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    bool FacingRight = true;
    bool isJumping = false;
    bool isCrouching = false; 
    float JumpWait = 1.0f;
    float JumpPower = 5.5f; 
    float walkspeed = 3.0f;
    float MovingHorizontal = 0.0f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
     
            animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
            transform.position += horizontal * walkspeed * Time.deltaTime;


        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            FacingRight = false;
        }

        else if (Input.GetAxis("Horizontal") > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            FacingRight = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && FacingRight && isCrouching == false && isJumping == true)
        {
            transform.position = new Vector2(transform.localPosition.x + 1.5f, transform.localPosition.y);
            animator.Play("Animation_Dash");
        }

        else if (Input.GetKeyDown(KeyCode.F) && !FacingRight && isCrouching == false && isJumping == true)
        {
            transform.position = new Vector2(transform.localPosition.x - 1.5f, transform.localPosition.y);
            animator.Play("Animation_Dash");
        }

        //Crouching 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.Play("Animation_Crouch");
            animator.SetFloat("Horizontal", 0);
            isCrouching = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = true;
            animator.Play("Animation_Idle");
        }


        if (Input.GetKey(KeyCode.LeftShift)  || Input.GetKey(KeyCode.RightShift))
        {
            walkspeed = 7.0f;
            animator.speed = 2.0f; 
        }

        else
        StartCoroutine(WaitForShiftUp());


        if (Input.GetButtonDown("Jump") && isJumping == false && isCrouching == false)
        {
            isJumping = true; 
            StartCoroutine(Jump());
        }


        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if 'Character' collides with something tagged as 'NextLevel'
        if (collision.gameObject.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


    IEnumerator Jump()
    {
        rb.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
        yield return new WaitForSeconds(JumpWait);

        //Set jumping to false, so player can jump after a certain period of time
        isJumping = false; 
    }


    IEnumerator WaitForShiftUp()
    {
        yield return new WaitForSeconds(0.3f);
        animator.speed = 1.0f;
        walkspeed = 3.0f;
    }

}
