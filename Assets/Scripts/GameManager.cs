using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private int totalScore = 0;
    private bool pause = false;
    private bool controlSwitched = false;

    public int TotalScore
    {
        get
        {
            return totalScore;
        }
        set
        {
            if (value >= 0)
                totalScore = value;
        }
    }
    public bool Pause
    {
        get
        {
            return pause;
        }
    }
    public bool ControlSwitched
    {
        get
        {
            return controlSwitched;
        }
    }

    public void InitGame()
    {
        pause = false;

    }

    public void MakePause(bool makePauseFlag)
    {
        pause = makePauseFlag;
    }
    public void MakeSwitchControl()
    {
        controlSwitched = !controlSwitched;
    }
    public void Resume()
    {
        pause = false;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
	void Awake () 
    {
        if (instance == null)
            instance = this;
        else
            if (instance != this)
                Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
	}

}
