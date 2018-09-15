using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour {

    //Buttons used in menu
    public Button StartButton;
    public Button ExitButton;
    public Button MenuButton;
    public Button CreditsButton;
    public Button RestartButton;

    // Use this for initialization
    void Start () {
        //dont destroy this game object
        DontDestroyOnLoad(this);
        //create code for buttons, buttons only work if there is an object attached to it
        if (StartButton != null)
        {
            Button btn = StartButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
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

    //starts game
    void TaskOnClick()
    {
        SceneManager.LoadScene("Level1");
        StatManager.level = 1;
    }

    //exits game
    void TaskOnClick1()
    {
        Application.Quit();
    }

    //returns to start menu
    void TaskOnClick2()
    {
        SceneManager.LoadScene("Start Screen");
    }

    //loads credits
    void TaskOnClick3()
    {
        SceneManager.LoadScene("Credits Screen");
    }

    //loads checkpoint
    void TaskOnClick4()
    {
        SceneManager.LoadScene("Level"+StatManager.level);
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
        SceneManager.LoadScene("VictoryScene");
    }
    //loads gameover scene
    public static void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
