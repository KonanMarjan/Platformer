using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraOffsetX;
    public float cameraOffsetY;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = new Vector3 (player.transform.position.x + cameraOffsetX, player.transform.position.y + cameraOffsetY, transform.position.z);
	}
}
