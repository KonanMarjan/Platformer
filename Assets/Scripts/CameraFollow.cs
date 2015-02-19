using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraStepX;
    public float playerSpaceAbove;
    public float playerSpaceUnder;
    public float cameraEdgeHor;
    public Transform cameraEdgeVertical;
    public Transform test;

    private bool cameraCorrectAllowHor = false; // flag allow to chage side
    private bool allowMoveCameraHor = true;

    private Animator anim;
    private float cameraEdgeUp;

    // Use this for initialization
    void Start()
    {
        cameraEdgeUp = transform.position.y + (2 * camera.orthographicSize) * playerSpaceAbove / 100; // calculate vertical edge of camera (% of height)
        float cameraEdgePosY = (2 * camera.orthographicSize) * playerSpaceUnder / 100;
        cameraEdgeVertical.transform.position = new Vector3(player.transform.position.x, transform.position.y - cameraEdgePosY, player.transform.position.z);
        anim = player.GetComponent<Animator>();
        test.transform.position = new Vector3(player.transform.position.x, cameraEdgeUp, player.transform.position.z);
    }


    /* forbid change side and correction position of camera */
    void SetCameraCorrectAllowHorFalse()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z);
        cameraCorrectAllowHor = false;
    }
    /*changes side*/
    void CameraCorrectHorizontal()
    {
        if (cameraOffsetX < 0) // facing left
        {
            if (transform.position.x - player.transform.position.x > cameraOffsetX)
            {
                transform.position = new Vector3(transform.position.x - cameraStepX, transform.position.y, transform.position.z); // transport camera to other side
            }
            else
            {
                SetCameraCorrectAllowHorFalse();
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
                SetCameraCorrectAllowHorFalse();
            }
        }

    }
    void CameraMoveHorizontal()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z); // following camera
    }
    void CameraMoveVertical()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y + cameraOffsetY, transform.position.z); // following camera
    }
    
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        /*if ((player.transform.localScale.x < 0 && cameraOffsetX > 0) || (player.transform.localScale.x > 0 && cameraOffsetX < 0)) // check change of facing
        {
            cameraOffsetX = -cameraOffsetX;
            cameraCorrectAllowHor = true;
        }*/
        if (anim.GetBool("wallAttached"))
            allowMoveCameraHor = false;

        if (transform.position.x - player.transform.position.x > cameraEdgeHor && cameraOffsetX > 0)
        {
            cameraCorrectAllowHor = true;
            cameraOffsetX = -cameraOffsetX;
            allowMoveCameraHor = true;
        }
        if (transform.position.x - player.transform.position.x < -cameraEdgeHor && cameraOffsetX < 0)
        {
            cameraCorrectAllowHor = true;
            cameraOffsetX = -cameraOffsetX;
            allowMoveCameraHor = true;
        }

        if ((cameraOffsetX > 0 && player.transform.position.x - transform.position.x > cameraEdgeHor) || (cameraOffsetX < 0 && player.transform.position.x - transform.position.x < -cameraEdgeHor))
        {
            cameraCorrectAllowHor = true;
            allowMoveCameraHor = true;
        }


        if (cameraCorrectAllowHor)
        {
            CameraCorrectHorizontal();
        }
        else
        {
            if (allowMoveCameraHor)
                CameraMoveHorizontal();
        }
            
	}
}
