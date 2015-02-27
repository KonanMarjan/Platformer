using UnityEngine;
using System.Collections;

public class finishScript : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            GameManager.instance.TotalScore += LevelManager.instance.Score;
            LevelManager.instance.InitLevel();
            levelMenu.instance.ActivateFinishMenu(true);
            GameManager.instance.MakePause(true);
            levelMenu.instance.TotalScoreUpdate();
        }
    }
}
