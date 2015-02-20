using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraStepX;
    public float cameraStepY;
    public float playerSpaceAbove;
    public float playerSpaceUnder;
    public float cameraEdgeHor;
    public float playerHeight;

    private bool cameraCorrectAllowHor = false; // flag allow to chage side
    private bool allowMoveCameraHor = true;
    private bool cameraCorrectAllowVer = false;
    private bool allowMoveCameraVerUp = false;
    private bool allowMoveCameraVerDown = false;

    private Animator anim;
    private float cameraEdgeUp;
    private float cameraEdgeDown;


    // Use this for initialization
    void Start()
    {
        cameraEdgeUp = transform.position.y + (2 * camera.orthographicSize) * playerSpaceAbove / 100 - (playerHeight / 2f); // calculate vertical edge of camera (% of height)
        cameraEdgeDown = transform.position.y - (2 * camera.orthographicSize) * playerSpaceUnder / 100 + (playerHeight / 2f);
        anim = player.GetComponent<Animator>();
    }


    /* forbid change side and correction position of camera */
    void SetCameraCorrectAllowHorFalse()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffsetX, transform.position.y, transform.position.z);
        cameraCorrectAllowHor = false;
    }
    void SetCameraCorrectAllowVerFalse()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y + cameraOffsetY, transform.position.z);
        cameraCorrectAllowVer = false;
    }
    /**/
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
        if (allowMoveCameraVerUp)
            transform.position = new Vector3(transform.position.x, player.transform.position.y - cameraEdgeUp, transform.position.z); // following camera
        if (allowMoveCameraVerDown)
            transform.position = new Vector3(transform.position.x, player.transform.position.y - cameraEdgeDown, transform.position.z); // following camera
    }
    
	
	// Update is called once per frame
	void Update () 
    {
        if (anim.GetBool("wallAttached"))
            allowMoveCameraHor = false;
        if (anim.GetBool("grounded"))
            cameraCorrectAllowVer = true;
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

        if (player.transform.position.y - transform.position.y > cameraEdgeUp)
        {
            allowMoveCameraVerUp = true;
            allowMoveCameraVerDown = false;
            cameraCorrectAllowVer = false;
        }
        else
            allowMoveCameraVerUp = false;

        if (player.transform.position.y - transform.position.y < cameraEdgeDown)
        {
            allowMoveCameraVerDown = true;
            allowMoveCameraVerUp = false;
            cameraCorrectAllowVer = false;
        }
        else
        {
            allowMoveCameraVerDown = false;
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
