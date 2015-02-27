using UnityEngine;
using System.Collections;
using System;


public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;
    public GameObject player;
    public Transform start;
    private int score;
    private int curLevel;
    
    public bool allowDoubleJump;
    public bool allowRush;
    public bool allowWallRun;


    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (value >= 0)
                score = value;
        }
    }
    public int CurrentLevel
    {
        get
        {
            return curLevel;
        }
    }

    public void Restart()
    {
        InitLevel();
        LoadLevel(CurrentLevel);
    }
    public void LoadLevel(int level)
    {
        GameManager.instance.TotalScore += score;
        Application.LoadLevel(level);
    }


	// Use this for initialization
	void Awake () 
    {

        instance = this;
        if (PlayerController.instance == null)
            Instantiate(player);
        curLevel = Application.loadedLevel;
        InitLevel();
	}
    public void InitLevel()
    {
        Score = 0;
    }
    public void ScoreInc(int howMuch)
    {
        Score += howMuch;
    }
    public void ScoreDec(int howMuch)
    {
        Score -= howMuch;
    }
    void Start()
    {
        player.transform.position = start.position;
    }
}
