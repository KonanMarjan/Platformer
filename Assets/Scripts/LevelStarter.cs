using UnityEngine;
using System.Collections;

public class LevelStarter : MonoBehaviour {
        public levelMenu levelMenu;
	// Use this for initialization
	void Awake () {
        if (levelMenu.instance == null)
            Instantiate(levelMenu);
	}
            
}
