using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{

    Rigidbody2D rb;

    public float speed;

    public Transform projectileSpawnPoint; // where you want the projectile to spawn
    public Eprojectile projectilePrefab; // what you want to spawn
    public float projectileForce; // effects the speed

    //handle how often the projectiles will fire.
    public float projectileFireRate = 2.5f;
    float timeSinceLastFire = 0.0f;

    public bool isFacingRight;

    public int health = 20;

    public RectTransform healthBar;
    float healthScale;


    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        if (!rb)
        {
            Debug.LogWarning("No Rigidbody2D found.");
        }

        if (speed <= 0)
        {
            speed = 5.0f;

            Debug.LogWarning("Default speeding to " + speed);
        }

        if (health <= 0)
        {
            health = 20;


            Debug.Log("health was not set. defaulting to 20.");

            healthScale = healthBar.sizeDelta.x / health;
        }

        if (projectileFireRate <= 0)
        {
            projectileFireRate = 1.0f;

            Debug.LogWarning("Default projectileFireRate to " + projectileFireRate);
        }

        if (projectileForce <= 0)
        {
            projectileForce = 50.0f;

            Debug.LogWarning("Default projectileForce to " + projectileForce);
        }

    }

    // Update is called once per frame
    void Update()
    {

        //check if Enemy is facing right
        if (!isFacingRight)
            //move Enemy left
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else
            //move enemy right
            rb.velocity = new Vector2(speed, rb.velocity.y);


    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        // check if the enemy ran into a barrier
        if (c.gameObject.tag == "Enemy Barrier")
        {
            //flip the enemy
            flip();
        }
    }

    private void OnTriggerStay2D(Collider2D range)
    {


        if (range.gameObject.tag == "Player")
        {
            if (Time.time > timeSinceLastFire + projectileFireRate)
            {
                fire();

                timeSinceLastFire = Time.time; //timestamp of the last fire

                // fire();

            }
        }

    }

    private void OnCollisionEnter2D(Collision2D c)
    {

        if (c.gameObject.tag == "projectile" && c.gameObject.tag != "Eprojectile")
        {            
            health--;
            healthBar.sizeDelta = new Vector2(health * healthScale, healthBar.sizeDelta.y);
            // health -= c.gameObject.GetComponent<Projectile>().GetDamage();
            if (health <= 0)
            {                
                Destroy(gameObject);
            }

        }

        if (c.gameObject.tag == "sProjectile")
        {
            health -= 7;

            healthBar.sizeDelta = new Vector2(health * healthScale, healthBar.sizeDelta.y);

            if (health <= 0)
            {
                /*kill enemy
                 * play sound
                 * animation
                 */

                //when HP = 0, destroy enemy
                Destroy(gameObject);
                SceneManager.LoadScene("Credits");
                //SoundManager.instance.playESound(SoundManager.instance.creditSong);
            }

        }

        if (c.gameObject.tag == "slash")
        {
            // Remove 1 HP
            health -= 2;
            healthBar.sizeDelta = new Vector2(health * healthScale, healthBar.sizeDelta.y);
            // health -= c.gameObject.GetComponent<Projectile>().GetDamage();

            if (health <= 0)
            {
                /*kill enemy
                 * play sound
                 * animation
                 */

                //when HP = 0, destroy enemy
                SoundManager.instance.playSingleSound(SoundManager.instance.dieSound);
                Game_Manager.instance.score += 10;
                Destroy(gameObject);
            }

        }

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

        // scaleFactor.x *= -1; //scaleFactor.x = -scaleFactor.x

        transform.localScale = scaleFactor;

    }

    void fire()
    {
        Eprojectile temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileForce, 0); //makes the projectile move when you spawn it

        temp.GetComponent<Rigidbody2D>().velocity = Vector2.right * projectileForce; //does the same thing as the one above, just a different way of doing it

    }
}
