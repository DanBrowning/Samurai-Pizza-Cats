using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{

    public float speed;
    public float lifeTime; //this tells the object how long it lives for before it gets destroyed


    // Use this for initialization
    void Start()
    {
        if (lifeTime <= 0)
        {
            lifeTime = 1.0f;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        
        Destroy(gameObject, lifeTime); 
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D c) 
    {


      if (c.gameObject.tag != "Player" && c.gameObject.tag != "Solid" && c.gameObject.tag != "Boss")
        {
            Destroy(c.gameObject);  
        }
       
    }




}
