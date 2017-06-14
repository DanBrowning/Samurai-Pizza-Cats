using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game_Manager : MonoBehaviour
{

    static Game_Manager _instance = null;

    public GameObject playerPrefab;

    int _score;
    public Text scoreText;
    int _ammo;
    public Text ammoText;
    int _special;
    public Text specialText;



    // Use this for initialization
    void Start()
    {
        if (instance)
            DestroyImmediate(gameObject); // destroys the new Game_Manager upon scenes being loaded and keeps the old one
        else
        {
            instance = this;

            DontDestroyOnLoad(this);
        }


        // assign a starting score
        score = 0;

        // lives = 3;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Samurai Pizza Cats")
            {
                SceneManager.LoadScene("Menu");
            }
            else if (SceneManager.GetActiveScene().name == "Menu")
            {
                SceneManager.LoadScene("Samurai Pizza Cats");
            }
            else if (SceneManager.GetActiveScene().name == "Game_Over")
            {
                SceneManager.LoadScene("Menu");
                SoundManager.instance.playESound(SoundManager.instance.menuSong);
            }
            else if (SceneManager.GetActiveScene().name == "Credits")
            {
                SceneManager.LoadScene("Menu");
                SoundManager.instance.playESound(SoundManager.instance.menuSong);
            }
        }




    }

   
    public void StartGame()
    {
        SceneManager.LoadScene("Samurai Pizza Cats");
        SoundManager.instance.playESound(SoundManager.instance.levelSong);
    }


    public void credits()
    {
        SceneManager.LoadScene("Credits");
        SoundManager.instance.playESound(SoundManager.instance.creditSong);
    }


    
    public void QuitGame()
    {
       Debug.Log("Quit Game");

        Application.Quit();
    }





    //called when the character is spawned
    // all 4 of these can be used to spawn the character, it all depends on how you want to reference where the character is going to be spawned
    //public void spawnPlayer(int spawnLocation)
    // public void spawnPlayer(Vector3 spawnLocation)
    // public void spawnPlayer(Transform spawnLocation)
    public void spawnPlayer(int spawnLocation)
    {
        //requires the spawnPoint to be named (SceneName)_(number)
        // - scene_01_0
        string spawnPointName = SceneManager.GetActiveScene().name
            + "_" + spawnLocation;

        // find the location to spawn the character at
        Transform spawnPointTransform = GameObject.Find(spawnPointName).GetComponent<Transform>();

        //instantiate the character GameObject
        Instantiate(playerPrefab, spawnPointTransform.position, spawnPointTransform.rotation);


    }






    public static Game_Manager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            // check if 'scoreText' was set before tyring to update HUD
            if (scoreText)
                //update the HUD on every score change
                scoreText.text = "Score: " + score;
        } // can also just use 'set' though i have no idea how to do that right now
    }

    public int ammo
    {
        get { return _ammo; }
        set
        {
            _ammo = value;
            if (ammoText)
                ammoText.text = "Remaining Ammo: " + ammo;    
        }
    }

    public int special
    {
        get { return _special; }
        set
        {
            _special = value;
            // check if 'scoreText' was set before tyring to update HUD
            if (specialText)
                //update the HUD on every score change
                specialText.text = "Special: " + special;
        } // can also just use 'set' though i have no idea how to do that right now
    }




}
