using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    Rigidbody2D rb;

    public Rigidbody2D rb2;

    public float speed;
    public int ammoValue;
    public int ammoValueS;

    public float jumpForce;
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool Climbing;

    Animator anim;

    // Handle Projectile Instantiation (aka creation)
    public Transform projectileSpawnPoint; // where you want the projectile to spawn
    public Projectiles projectilePrefab; // what you want to spawn
    public float projectileForce; // effects the speed

    public Transform projectileSpawnPointS; // where you want the projectile to spawn
    public Projectiles projectilePrefabS; // what you want to spawn
    public float projectileForceS; // effects the speed

    public Transform projectileSpawnPointSl; // where you want the projectile to spawn
    public Projectiles projectilePrefabSl; // what you want to spawn
    public float projectileForceSl;

    public RectTransform healthBar;
    float healthScale;

    public bool isFacingRight; 

    public int _hits;
    //float timeSinceLastFire = 0.0f;
    //public float projectileFireRate;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        if (!rb)
        {
            Debug.LogWarning("No Rigidbody2D found.");
        }

        if (!rb2)
        {
            rb2 = GetComponent<Rigidbody2D>();
        }

        if (speed <= 0)
        {
            speed = 4.0f;

            Debug.LogWarning("Default speeding to " + speed);
        }

        if (jumpForce <= 0)
        {
            jumpForce = 6.0f;

            Debug.LogWarning("Default jumpForce to " + jumpForce);
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;

            Debug.LogWarning("Default groundCheckRadius to " + groundCheckRadius);
        }

        if (!groundCheck)
        {
            Debug.LogWarning("No groundCheck found.");
        }

        anim = GetComponent<Animator>();

        if (!anim)
        {
            Debug.LogWarning("No animator found.");
        }

        if (!projectileSpawnPoint)
        {
            Debug.LogError("No projectileSpawnPoint found on game object.");
        }

        if (!projectilePrefab)
        {
            Debug.LogError("No projectilePrefab found on game object.");
        }

        if (projectileForce == 0)
        {
            projectileForce = 7.0f;

            Debug.Log("projectileForce was not set. Defaulting to " + projectileForce);
        }

        hits = 3;
        ammoValue = 5;
        ammoValueS = 1;
        Game_Manager.instance.special = 1;
        Game_Manager.instance.ammo = 5;

        healthScale = healthBar.sizeDelta.x / hits;

    }

    // Update is called once per frame
    void Update()
    {

        if (Pause.paused)
            return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        float moveValue = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("Attacking", true);
            slash();
        }
        else
        {
            anim.SetBool("Attacking", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && ammoValue > 0)
        {
            anim.SetBool("Shoot", true);
            fire();
            ammoValue--;
            Game_Manager.instance.ammo--;

        }
        else
        {
            anim.SetBool("Shoot", false);
        }

        if (Input.GetKey(KeyCode.DownArrow) && isGrounded)
        {
            anim.SetBool("Crawl", true);
            speed = 2.0f;
        }
        else
        {
            anim.SetBool("Crawl", false);
            speed = 4.0f;
        }

        rb.velocity = new Vector2(speed * moveValue, rb.velocity.y);

        anim.SetFloat("MoveValue", Mathf.Abs(moveValue));

        if (Input.GetKeyDown(KeyCode.Z) && ammoValueS > 0)
        {
            anim.SetBool("StrongAttack", true);
                fireS();
                ammoValueS--;
            Game_Manager.instance.special--;
        }
        else
        {
            anim.SetBool("StrongAttack", false);
        }

        if ((isFacingRight && moveValue < 0) || (!isFacingRight && moveValue > 0))
            flip();
              
        if (Climbing == true)
        {
            rb2.gravityScale = 0;
            anim.SetBool("Ladder", true);



            //this is up
            if (Input.GetAxis("Vertical") > 0)
            {
                //turn off gravity
                //move up the ladder at a slow pace
                //did that for testing
                transform.Translate(0, 1 * Time.deltaTime, 0);
                SendMessage("onClimb", SendMessageOptions.DontRequireReceiver);
            }
            //this is down
            else if (Input.GetAxis("Vertical") < 0)
            {
                transform.Translate(0, -1 * Time.deltaTime, 0);
                SendMessage("onClimb", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                //if you are not moving then just go idle on the ladder
                SendMessage("onClimbIdle", SendMessageOptions.DontRequireReceiver);
            }            

        }
        else
        {
            rb2.gravityScale = 1;
        }




        if (hits == 0)
        {            
            SceneManager.LoadScene("Game_Over");
            SoundManager.instance.playESound(SoundManager.instance.gameOverSong);
        }

    } // end update

    void OnTriggerEnter2D(Collider2D Collect)
    {
        if (Collect.gameObject.tag == "Ammo_Collect")
        {
            ammoValue += 5;
            Game_Manager.instance.score += 5;
            Game_Manager.instance.ammo += 5;


            /*if (Game_Manager.instance)
               Game_Manager.instance.ammo += ammoValue;
           else
               Debug.Log("no Game_Manager Found!!");
           Destroy(gameObject);*/
            Collect.gameObject.SetActive(false);
        }
        else if (Collect.gameObject.CompareTag("Health_Collect"))
        {
            hits++;
            healthBar.sizeDelta = new Vector2(hits * healthScale, healthBar.sizeDelta.y);
            Game_Manager.instance.score += 5;
            //Destroy(gameObject);
            Collect.gameObject.SetActive(false);
        }
        else if (Collect.gameObject.CompareTag("Special_Collect"))
        {
            ammoValueS++;
            Game_Manager.instance.score += 5;
            Game_Manager.instance.special++;
            //Destroy(gameObject);
            Collect.gameObject.SetActive(false);
        }

        if (Collect.gameObject.CompareTag("Enemy"))
        {
            hits--;
            healthBar.sizeDelta = new Vector2(hits * healthScale, healthBar.sizeDelta.y);
            SoundManager.instance.playSingleSound(SoundManager.instance.hurtSound);
            anim.SetBool("hurtBool", true);            
        }

        if (Collect.gameObject.CompareTag("Eprojectile"))
        {
            hits--;
            healthBar.sizeDelta = new Vector2(hits * healthScale, healthBar.sizeDelta.y);
            SoundManager.instance.playSingleSound(SoundManager.instance.hurtSound);
            anim.SetBool("hurtBool", true);
        }


    }        

    private void OnTriggerStay2D(Collider2D Trigger)
    {
        if (Trigger.gameObject.CompareTag("Water"))
        {
            anim.SetBool("Swimming", true);
            jumpForce = 2;
            speed = 1;
            rb2.gravityScale = 0.5f;
            
        }
        else
        {
            anim.SetBool("Swimming", false);
            jumpForce = 6;
            speed = 4;
            rb2.gravityScale = 1;
        }

        if (Trigger.gameObject.CompareTag("Ladder") && (Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            anim.SetBool("Ladder", true);
            Climbing = true;
        }
      }


        private void OnTriggerExit2D(Collider2D trigger)
    {
        
            if (trigger.gameObject.CompareTag("Ladder"))
        {
            anim.SetBool("Ladder", false);
      
            Climbing = false;
        }
        if (trigger.gameObject.CompareTag("Enemy"))
            anim.SetBool("hurtBool", false);

    }

       

    void fire()
    { //instantiate means to create. creates the projectile and adds it to the scene as a clone
      //prefab is what is being created, spawnpoint is where, rotation is 
        Projectiles temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileForce, 0); //makes the projectile move when you spawn it

        temp.GetComponent<Rigidbody2D>().velocity = Vector2.right * projectileForce; //does the same thing as the one above, just a different way of doing it

        if (isFacingRight)
            temp.speed = projectileForce;
        else if (!isFacingRight)
            temp.speed = -projectileForce;
    }

    void fireS()
    { //instantiate means to create. creates the projectile and adds it to the scene as a clone
      //prefab is what is being created, spawnpoint is where, rotation is 
        Projectiles temp = Instantiate(projectilePrefabS, projectileSpawnPointS.position, projectileSpawnPointS.rotation);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileForceS, 0); //makes the projectile move when you spawn it

        temp.GetComponent<Rigidbody2D>().velocity = Vector2.right * projectileForceS; //does the same thing as the one above, just a different way of doing it

        if (isFacingRight)
            temp.speed = projectileForceS;
        else if (!isFacingRight)
            temp.speed = -projectileForceS;
    }

    void slash()
    { //instantiate means to create. creates the projectile and adds it to the scene as a clone
      //prefab is what is being created, spawnpoint is where, rotation is 
        Projectiles temp = Instantiate(projectilePrefabSl, projectileSpawnPointSl.position, projectileSpawnPointSl.rotation);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileForceSl, 0); //makes the projectile move when you spawn it

        temp.GetComponent<Rigidbody2D>().velocity = Vector2.right * projectileForceSl; //does the same thing as the one above, just a different way of doing it

        if (isFacingRight)
            temp.speed = projectileForceSl;
        else if (!isFacingRight)
            temp.speed = -projectileForceSl;
    }

    void flip()
    {
        isFacingRight = !isFacingRight;

        /* if (isFacingRight == true)  just another way to write it, do not have both or it will counteract each other
             isFacingRight = false;
         else
             isFacingRight = true;
             */

        Vector3 scaleFactor = transform.localScale;

        scaleFactor.x *= -1; //scaleFactor.x = -scaleFactor.x

        transform.localScale = scaleFactor;
    }









    public int hits
    {
        get { return _hits; }
        set
        {
            _hits = value;

        }
    }

}


