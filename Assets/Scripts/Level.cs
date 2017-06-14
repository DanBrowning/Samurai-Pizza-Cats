using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Level : MonoBehaviour
{

    public int spawnLocation;
    //public string spawnLocation
    //public transform spawnLocation
    //public Vector3 spawnLocation
    //public GameObject spawnLocation




    // Use this for initialization
    void Awake()
    {
        Pause.paused = false;
        Time.timeScale = 1f;
        if (spawnLocation < 0)
            spawnLocation = 0;


        // call spawnPlayer() from Game_Manager
        Game_Manager.instance.spawnPlayer(spawnLocation);

        Game_Manager.instance.scoreText = GameObject.Find("Text_Score").GetComponent<Text>(); // finds the score so the UI can interact with it

        Game_Manager.instance.scoreText.text = "Score: " + Game_Manager.instance.score; // gets the score text to spawn on the screen

        Game_Manager.instance.ammoText = GameObject.Find("ammoCount").GetComponent<Text>(); // finds the score so the UI can interact with it

        Game_Manager.instance.ammoText.text = "Ammo: " + Game_Manager.instance.ammo;

        Game_Manager.instance.specialText = GameObject.Find("Special").GetComponent<Text>(); // finds the score so the UI can interact with it

        Game_Manager.instance.specialText.text = "Special: " + Game_Manager.instance.special;
    }
}