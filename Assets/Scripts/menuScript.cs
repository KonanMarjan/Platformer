using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour {
    public void LoadTest()
    {
        GameManager.instance.InitGame();
        Application.LoadLevel(1);
    }
    public void Quit()
    {
        Application.Quit();
    }

	// Use this for initialization
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
	}
	
	// Update is called once per frame

}
