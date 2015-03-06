using UnityEngine;
using System.Collections;

public class ropeScript : MonoBehaviour {

    private GameObject player;
    private Collider2D collIgnore;
	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        collIgnore = player.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(collIgnore, GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
