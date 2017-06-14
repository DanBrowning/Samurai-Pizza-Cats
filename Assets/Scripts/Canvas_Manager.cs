using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Manager : MonoBehaviour
{

    public Button startBtn;
    public Button credBtn;
    public Button quitBtn;


    // Use this for initialization
    void Start()
    {

        if (startBtn)
            startBtn.onClick.AddListener(Game_Manager.instance.StartGame);

        if (credBtn)
            credBtn.onClick.AddListener(Game_Manager.instance.credits);

        if (quitBtn)
            quitBtn.onClick.AddListener(Game_Manager.instance.QuitGame);



    }

    // Update is called once per frame
    void Update()
    {

    }
}
