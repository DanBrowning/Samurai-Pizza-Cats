using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    
    public Button quitBtn;
    public Transform Canvas_Pause;

    private static bool _paused = false;


    // Use this for initialization

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Canvas_Pause.gameObject.activeInHierarchy == false)
            {
                Canvas_Pause.gameObject.SetActive(true);
                Time.timeScale = 0;
                paused = true;

            }
            else
            {
                Canvas_Pause.gameObject.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            }
        }

        if (quitBtn)
            quitBtn.onClick.AddListener(QuitGame);




    }

    public static bool paused
    {
        get { return _paused; }
        set { _paused = value; }
    }


    public void QuitGame()
    {
        //Quits game (only works one the .exe, it does not work in the editor)
        Debug.Log("Quit Game");

        SceneManager.LoadScene("Menu");
        SoundManager.instance.playESound(SoundManager.instance.menuSong);
    }
}
