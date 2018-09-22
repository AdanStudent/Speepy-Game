﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour {

    //Buttons used in menu
    public Button StartButton1;
    public Button StartButton2;
    public Button ExitButton;
    public Button MenuButton;
    public Button CreditsButton;
    public Button RestartButton;


    // Use this for initialization
    void Start () {
        //dont destroy this game object
        DontDestroyOnLoad(this);
        //create code for buttons, buttons only work if there is an object attached to it
        if (StartButton1 != null)
        {
            Button btn = StartButton1.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }
        if (StartButton2 != null)
        {
            Button btn = StartButton1.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick5);
        }
        if (ExitButton != null)
        {
            Button btn1 = ExitButton.GetComponent<Button>();
            btn1.onClick.AddListener(TaskOnClick1);
        }
        if (MenuButton != null)
        {
            Button btn2 = MenuButton.GetComponent<Button>();
            btn2.onClick.AddListener(TaskOnClick2);
        }
        if (CreditsButton != null)
        {
            Button btn3 = CreditsButton.GetComponent<Button>();
            btn3.onClick.AddListener(TaskOnClick3);
        }
        if (RestartButton != null)
        {
            Button btn4 = RestartButton.GetComponent<Button>();
            btn4.onClick.AddListener(TaskOnClick4);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    //loads instructions
    void TaskOnClick()
    {
        SceneManager.LoadScene("Instructions");
    }

    //exits game
    void TaskOnClick1()
    {
        Application.Quit();
    }

    //returns to start menu
    void TaskOnClick2()
    {
        SceneManager.LoadScene("StartMenu");
    }

    //loads credits
    void TaskOnClick3()
    {
        SceneManager.LoadScene("Credits");
    }

    //loads checkpoint
    void TaskOnClick4()
    {
        SceneManager.LoadScene("Level"+StatManager.level);
    }

    //loads game
    void TaskOnClick5()
    {
        SceneManager.LoadScene("Level1");
        StatManager.level = 1;
    }

    //moves through levels as they are completed
    public static void LevelChange()
    {
        if (StatManager.level.Equals(1))
            SceneManager.LoadScene("Level2");
        else if (StatManager.level.Equals(2))
            SceneManager.LoadScene("Level3");
        else if (StatManager.level.Equals(3))
            Win();
        StatManager.level++;
    }

    //loads win scene
    public static void Win()
    {
        SceneManager.LoadScene("Victory");
    }
    //loads gameover scene
    public static void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
