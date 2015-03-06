using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

    private GameObject player;     // reference to player
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float xSmooth;
    public float ySmooth;
    public float tragetSmooth;
    //public float correctSmooth;
    //public float changeSideSmooth;

    

    void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");

        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, player.transform.position.y + cameraOffsetY, transform.position.z);       
    }



    void FollowHorizontal()
    {
        float targetX = Mathf.Lerp(transform.position.x, player.transform.position.x + cameraOffsetX, xSmooth * Time.deltaTime); ;
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }


    void FollowVertical()
    {
        float targetY = Mathf.Lerp(transform.position.y, player.transform.position.y + cameraOffsetY, ySmooth * Time.deltaTime); ;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
    void GoToTarget(Collider2D target)
    {
        float targetX = Mathf.Lerp(transform.position.x, target.transform.position.x, tragetSmooth * Time.deltaTime);
        float targetY = Mathf.Lerp(transform.position.y, target.transform.position.y, tragetSmooth * Time.deltaTime);
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
    void PlayerLockedCamera()
    {
        FollowHorizontal();
        FollowVertical();
    }
    /*void CorrectVerticly()
    {
        float targetY = Mathf.Lerp(transform.position.y, player.transform.position.y + cameraOffsetY, correctSmooth * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
    void CorrectHorizontaly()
    {
        float targetX = Mathf.Lerp(transform.position.x, player.transform.position.x + cameraOffsetX, correctSmooth * Time.deltaTime);
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
    */
	// Update is called once per frame
    void Update()
    {
        if (player.GetComponent<ZonesScript>().allowHor)
        {
            FollowHorizontal();
        }
        else if (player.GetComponent<ZonesScript>().allowVer)
        {
            FollowVertical();
        }
        else if (player.GetComponent<ZonesScript>().allowTarget)
            GoToTarget(player.GetComponent<ZonesScript>().target);
        else if (player.GetComponent<ZonesScript>().changeSide)
        {
            cameraOffsetX = -cameraOffsetX;
            player.GetComponent<ZonesScript>().changeSide = false;
        }

        else
            PlayerLockedCamera();
        //if (player.GetComponent<PlayerController>().Grounded)


        /*FollowVertival();
        FollowHorizontal();*/
    }

}
