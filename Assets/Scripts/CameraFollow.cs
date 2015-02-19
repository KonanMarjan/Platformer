using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraStepX;

    private bool changeSide = false; // flag allow to chage side
    private bool followHorizontal = true;
    private Animator anim;


    /* forbid change side and correction position of camera */
    void SetChangeSideFalse()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z);
        changeSide = false;
    }
    /*changes side*/
    void ChangeSide()
    {
        if (cameraOffsetX < 0) // facing left
        {
            if (transform.position.x - player.transform.position.x > cameraOffsetX)
            {
                transform.position = new Vector3(transform.position.x - cameraStepX, transform.position.y, transform.position.z); // transport camera to other side
            }
            else
            {
                SetChangeSideFalse();
            }
        } 
        else                  // facing right
        {
            if (transform.position.x - player.transform.position.x < cameraOffsetX)
            {
                transform.position = new Vector3(transform.position.x + cameraStepX, transform.position.y, transform.position.z);
            }
            else
            {
                SetChangeSideFalse();
            }
        }

    }
    void CameraMove(bool followHorizontal)
    {
        if (followHorizontal)
            transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z); // following camera
        else
            transform.position = new Vector3(transform.position.x, player.transform.position.y + cameraOffsetY, transform.position.z); // following camera


    }
    // Use this for initialization
	void Start () 
    {
        anim = player.GetComponent<Animator>();
        //camera.orthographicSize
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*if (anim.GetBool("wallAttached"))
        {
            followHorizontal = false;
            changeSide = true;
        }
        if (anim.GetBool("grounded"))
            followHorizontal = true;*/
        if ((player.transform.localScale.x < 0 && cameraOffsetX > 0) || (player.transform.localScale.x > 0 && cameraOffsetX < 0)) // check change of facing
        {
            cameraOffsetX = -cameraOffsetX;
            changeSide = true;
        }
        if (changeSide)
        {
            ChangeSide();
        }
        else
            CameraMove(followHorizontal);   
	}
}
