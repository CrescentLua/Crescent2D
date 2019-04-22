using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    bool FacingRight = true;
    bool isJumping = false;
    bool isCrouching = false; 
    float JumpWait = 1.0f;
    float JumpPower = 6.5f; 
    float walkspeed = 3.0f;
    float MovingHorizontal = 0.0f;
    float Health;

    public Transform ProjectileSpawnPoint;
    public float projectileForce;   // How fast to fire it
    public Rigidbody2D projectile; // What to spawn

    public GameObject UIHealthText;
    public GameObject UIHealthBar;

    public GameObject UIStatusText; 
    public GameObject UIStatusBar;

    float FlingDirection; 
    public bool isDead;

    float scaleFactor;

    string Status; 

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
        Health = 100.0f;
        projectileForce = 5.0f;

        Status = "Good";

        if (GameObject.Find("Canvas"))
        {
            UIHealthText = GameObject.Find("Canvas/Profile/Health");
            UIHealthBar = GameObject.Find("Canvas/Profile/HealthPanel");
            UIStatusText = GameObject.Find("Canvas/Profile/StatusPanel/Status");
            scaleFactor = (float)UIHealthBar.GetComponent<RectTransform>().sizeDelta.x / Health;
        }
    }

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
            animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
            transform.position += horizontal * walkspeed * Time.deltaTime;

        if (Health > 0)
        {
            UIHealthText.GetComponent<Text>().text = "Health: " + Health.ToString();
            UIStatusText.GetComponent<Text>().text = "Status: " + Status;
            UIHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Health * scaleFactor,
            UIHealthBar.GetComponent<RectTransform>().sizeDelta.y);
        }

        if (Health < 75.0f && Health > 40.0f)
        {
            UIHealthBar.GetComponent<Image>().color = new Color(255, 191, 0);
            Status = "Hurt";
        }


        if (Health < 35.0f)
        {
            UIHealthBar.GetComponent<Image>().color = new Color(255, 0, 0);
            Status = "Dying";
        }

        if (Health < 0)
        {
            Status = "Dead";
            UIHealthText.GetComponent<Text>().text = "Health: " + "0";
            StartCoroutine(Respawn());
        }

         if (Health > 75.0f)
        {
            UIHealthBar.GetComponent<Image>().color = new Color(0, 255, 0);
            Status = "Good";
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            FacingRight = false;
            flipProjectileSpawnpoint(); 
        }

        else if (Input.GetAxis("Horizontal") > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            FacingRight = true;
            flipProjectileSpawnpoint();
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


        if (Input.GetButtonDown("Fire1"))
        {
            if (isJumping == false)
            {
                fire(); 
            }

        }
    }

    void flipProjectileSpawnpoint()
    {
        Vector3 scaleFactor = ProjectileSpawnPoint.transform.localPosition;

        if (FacingRight == false)
        {
            scaleFactor.x = (ProjectileSpawnPoint.localPosition.x - ProjectileSpawnPoint.localPosition.x) + (-0.5f);  // or - -scaleFactor.x
            scaleFactor.y = -0.263f; 
            // Assign updated value back to 'localScale'
            ProjectileSpawnPoint.localPosition = scaleFactor; 
        }

        else if (FacingRight == true)
        {
            scaleFactor.x = ((-ProjectileSpawnPoint.localPosition.x) - (-ProjectileSpawnPoint.localPosition.x)) + (0.5f); // or - -scaleFactor.x
            scaleFactor.y = -0.263f;                                                                                                            // Assign updated value back to 'localScale'
            ProjectileSpawnPoint.localPosition = scaleFactor;
        }
    }


    void fire()
    {


        Debug.Log("Pew Pew");

        // Check if 'projectileSpawnPoint' and 'projectile' exist
        if (ProjectileSpawnPoint && projectile)
        {
            // Create the 'Projectile' and add to Scene
            animator.Play("Animation_Shoot");
            Rigidbody2D temp = Instantiate(projectile, ProjectileSpawnPoint.position,
                ProjectileSpawnPoint.rotation);

            // Stop 'Character' from hitting 'Projectile'
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
                temp.GetComponent<Collider2D>(), true);

            // Check what direction 'Character' is facing before firing
            if (FacingRight)
                temp.AddForce(ProjectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            else
                temp.AddForce(-ProjectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if 'Character' collides with something tagged as 'NextLevel'
        if (collision.gameObject.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

         if (collision.gameObject.tag == "Enemy")
        {

            Health = Health - Random.Range(3.5f, 20.5f);

            if (!FacingRight)
            {
                FlingDirection = 15.0f; 
            } 

            else if (FacingRight)
                {
                  FlingDirection = -15.0f;
                }

            StartCoroutine(Fling());   
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathFloor")
        {
            isDead = true; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.tag == "JumpPowerup")
        {
            Destroy(collision.gameObject);
            StartCoroutine(JumpPowerUp());
        }

        if (collision.gameObject.tag == "HealthPowerup")
        {
            Destroy(collision.gameObject);
            Health = Health + 100.0f; 
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

    IEnumerator Fling()
    {
        rb.AddForce(new Vector2(FlingDirection, 0.5f), ForceMode2D.Impulse);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    IEnumerator JumpPowerUp()
    {
        JumpPower = 13.0f; 
        yield return new WaitForSeconds(6.0f);
        JumpPower = 6.5f;
    }

}
