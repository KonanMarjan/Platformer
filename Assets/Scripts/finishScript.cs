using UnityEngine;
using System.Collections;

public class finishScript : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            GameManager.instance.TotalScore += LevelManager.instance.Score;
            levelMenu.instance.ActivateFinishMenu(true);
            GameManager.instance.MakePause(true);
            levelMenu.instance.TotalScoreUpdate();
        }
    }
}
