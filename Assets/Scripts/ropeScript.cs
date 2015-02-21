using UnityEngine;
using System.Collections;

public class ropeScript : MonoBehaviour {

    public GameObject player;
    private Collider2D collIgnore;
	// Use this for initialization
	void Start () 
    {
        Physics2D.IgnoreCollision(player.collider2D, collider2D);
        collIgnore = player.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(collIgnore, collider2D);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
