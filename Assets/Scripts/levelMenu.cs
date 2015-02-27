using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class levelMenu : MonoBehaviour
{

    public static levelMenu instance = null;
    public GameObject menu;
    public GameObject scoreText;
    public GameObject finishMenu;
    public GameObject totalScoreText;
    public GameObject switchControlText;
    private bool displayMenu = false;
    private bool pause;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            if (instance != this)
                Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        menu.SetActive(displayMenu);
        ActivateFinishMenu(false);
        InitSwitchControl();
    }
    private void InitSwitchControl()
    {
        if (GameManager.instance.ControlSwitched)
        {
            switchControlText.GetComponent<Text>().text = "swithed";
        }
        else
        {
            switchControlText.GetComponent<Text>().text = "default";
        }
    }
    public void PauseButton()
    {
        
        displayMenu = !displayMenu;
        pause = !pause;
        GameManager.instance.MakePause(pause);
        menu.SetActive(displayMenu);

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
                PauseButton();
            else
                MainMenuButton();
        }

    }
    public void RestartButton()
    {
        
        LevelManager.instance.Restart();
        ScoreUpdate();
        menu.SetActive(false);
        ActivateFinishMenu(false);
        GameManager.instance.MakePause(false);
    }
    public void ResumeButton()
    {
        displayMenu = false;
        menu.SetActive(displayMenu);
        pause = false;
        GameManager.instance.MakePause(pause);
    }
    public void SwitchControlButton()
    {
        GameManager.instance.MakeSwitchControl();
        InitSwitchControl();
        
    }
    public void MainMenuButton()
    {
        Application.LoadLevel(0);
        Destroy(gameObject);
    }
    public void ScoreUpdate()
    {
        scoreText.GetComponent<Text>().text = LevelManager.instance.Score.ToString();
    }
    public void ActivateFinishMenu(bool status)
    {
        finishMenu.SetActive(status);
    }
    public void TotalScoreUpdate()
    {
        totalScoreText.GetComponent<Text>().text = GameManager.instance.TotalScore.ToString();
    }
}

