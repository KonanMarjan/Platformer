using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;     // reference to player
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraStepX;
    public float cameraStepY;
    public float playerSpaceAbove;  // space for player above center of camera (% from height)
    public float playerSpaceUnder;  // space for player under center of camera (% from height)
    public float cameraEdgeHor;     // horizontal edge
    public float playerHeight;

    private bool cameraCorrectAllowHor = false; // flag allow to chage side
    private bool allowMoveCameraHor = true;
    private bool cameraCorrectAllowVer = false;
    private bool allowMoveCameraUp = false;
    private bool allowMoveCameraDown = false;

    private Animator anim;
    private float cameraEdgeUp;    // vertical upo edge
    private float cameraEdgeDown;  // vertical down edge


    // Use this for initialization
    void Start()
    {
        cameraEdgeUp = transform.position.y + (2 * camera.orthographicSize) * playerSpaceAbove / 100 - (playerHeight / 2f);    // calculate vertical up edge of camera (% to units)
        cameraEdgeDown = transform.position.y - (2 * camera.orthographicSize) * playerSpaceUnder / 100 - (playerHeight / 2f);  // calculate vertical down edge of camera (% to units)
        anim = player.GetComponent<Animator>();
    }


    /* forbid change side and correction position of camera */
    void SetCameraCorrectAllowHorFalse()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z);  // correction position of camera
        cameraCorrectAllowHor = false;  // forbid correct camera horizontaly
    }
    void SetCameraCorrectAllowVerFalse()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y + cameraOffsetY, transform.position.z);   // correction position of camera
        cameraCorrectAllowVer = false;  // forbid correct camera verticaly
    }
    /*correction camera*/
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
    void CameraCorrectVertical()
    {
        /*if (transform.position.y - player.transform.position.y > 0) // correct down
            if (transform.position.y - player.transform.position.y > cameraOffsetY)
                transform.position = new Vector3(transform.position.x, transform.position.y - cameraStepY, transform.position.z);
            else
                SetCameraCorrectAllowVerFalse();*/
    }
    void CameraMoveHorizontal()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z); // following camera
    }
    void CameraMoveVertical()
    {
        if (allowMoveCameraUp)
            transform.position = new Vector3(transform.position.x, player.transform.position.y - cameraEdgeUp, transform.position.z); // following camera
        if (allowMoveCameraDown)
            transform.position = new Vector3(transform.position.x, player.transform.position.y - cameraEdgeDown, transform.position.z); // following camera
    }
    
	
	// Update is called once per frame
	void Update () 
    {
        if (anim.GetBool("wallAttached"))   // touching wall
            allowMoveCameraHor = false;     // is stoping camera
        if (anim.GetBool("grounded"))
            cameraCorrectAllowVer = true;
        if (transform.position.x - player.transform.position.x > cameraEdgeHor && cameraOffsetX > 0) // player out of horizontal camera edge (after changing side) and facing right
        {
            cameraCorrectAllowHor = true;
            cameraOffsetX = -cameraOffsetX;
            allowMoveCameraHor = true;
        }
        if (transform.position.x - player.transform.position.x < -cameraEdgeHor && cameraOffsetX < 0) // player out of horizontal camera edge (after changing side) and facing left
        {
            cameraCorrectAllowHor = true;
            cameraOffsetX = -cameraOffsetX;
            allowMoveCameraHor = true;
        }

        if ((cameraOffsetX > 0 && player.transform.position.x - transform.position.x > cameraEdgeHor) || (cameraOffsetX < 0 && player.transform.position.x - transform.position.x < -cameraEdgeHor))  // player out of camera edge without changing side
        {
            cameraCorrectAllowHor = true;
            allowMoveCameraHor = true;
        }

        if (player.transform.position.y - transform.position.y > cameraEdgeUp)
        {
            allowMoveCameraUp = true;
            allowMoveCameraDown = false;
            cameraCorrectAllowVer = false;
        }
        else
            allowMoveCameraUp = false;

        if (player.transform.position.y - transform.position.y < cameraEdgeDown)
        {
            allowMoveCameraDown = true;
            allowMoveCameraUp = false;
            cameraCorrectAllowVer = false;
        }
        else
        {
            allowMoveCameraDown = false;
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
        CameraMoveVertical();
        if (cameraCorrectAllowVer)
            CameraCorrectVertical();
            
	}
}
