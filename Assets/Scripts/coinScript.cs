using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class coinScript : MonoBehaviour {

	// Use this for initialization
	void Awake () 
    {
        //text.GetComponent<Text>().text = score.ToString();
        //totalText.GetComponent<Text>().text = score.ToString();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "coin")
        {
            other.gameObject.SetActive(false);
            //score++;
            LevelManager.instance.ScoreInc(1);
            levelMenu.instance.ScoreUpdate();
        }


    }
}
