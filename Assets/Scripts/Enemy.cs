using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Rigidbody2D rb;

    public float speed;

    public bool isFacingRight;

    public int health = 2;

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
            health = 5;
        }

        Debug.Log("health was not set. defaulting to 5.");

       // healthScale = healthBar.sizeDelta.x / health;


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

    private void OnCollisionEnter2D(Collision2D c)
    {
        // check if the enemy ran into a barrier
        if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Solid" || c.gameObject.tag == "Player")
        {
            //flip the enemy
            flip();
        }

        if (c.gameObject.tag == "projectile")
        {
            // Remove 1 HP
            health--;
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

        if (c.gameObject.tag == "slash")
        {
            // Remove 1 HP
            health -= 2;
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

        if (c.gameObject.tag == "sProjectile" && c.gameObject.tag != "eprojectile")
        {
            health -= 7;

            //healthBar.sizeDelta = new Vector2(health * healthScale, healthBar.sizeDelta.y);

            if (health <= 0)
            {
                

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

        scaleFactor.x *= -1; //scaleFactor.x = -scaleFactor.x

        transform.localScale = scaleFactor;

    }
}
