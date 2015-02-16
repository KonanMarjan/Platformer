using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    //public float floorOffset = 16f; //floor move coordinates
    //public float backOffset = 66f;  // background move coordinates
    //public float floorStep = 3f;    // floor move step
    //public float backStep = 66f;     // background move step
    //public GameObject floor;        // reference to platform
    //public GameObject back;         // reference to background
    //public GameObject camera;       //reference to camera
    //public GameObject cameraWheel;       // reference to camera wheel
    //public float cameraOffsetRight; // offset for camera when facing right
    //public float cameraOffsetLeft;  // offset for camera when facing left
    //public float cameraOffsetUp;    // horizontal offset
    //private bool corrVertical = false;   // vertial camera correction flag
    //private float cameraOffset;     // camera offset

    public float cameraOffsetVertical;
    public float cameraOffsetHorizontal;
    public float verticalCameraEdge;
    public float horizontalCameraEdge;
    public GameObject cameraObject;
    private Vector3 cameraOffset;

    public float lineSpeed = 10f;   //player speed
    public float jumpForce = 0.5f;  // force of jump
    public float rushForce = 7000f; // force of rush
    public float cameraStep;    // step of camera correct


    public LayerMask whatIsGround; // reference to floor
    public LayerMask whatIsWall;  // reference to wall
    public LayerMask whatIsPlayer; // reference to player
    public Transform groundCheck;  // ground check circle
    public Transform wallCheck;   // wall check circle
    public Transform cameraEdge; // camera edge object
    public float wallRunSpeed; // speed of wall run
    public float slideSpeed;       //speed of wall slide
    public float slideSpeedInc;   // speed increment for wall slide
    public float playerHigh;
    public float playerWidth;
    public float groundRushInc;
    public float groundRushLimit;

    private bool jumpButtonPressed = false;
    private bool rushButtonPressed = false;
    private bool cameraEdgeOut = false;

    private float curSpeed;
    private bool makeGroundRush = false;
    private bool correctCameraAllow = false;
    private float cameraStepHor;  // horizontal step of camera
    private float cameraStepVer;  // vertical step of camera
    private bool facingRight = true;

    private bool oneAirRushDone = false; // air rush flag
    private bool wallRunDone = false;
    private bool doubleJump = false; // double jump flag
    private bool doSecondJump = false;  // allow jump in the air flag
    private float radius = 0.02f;   // radius of check circle
    private bool grounded = false;  // grounded flag
    private bool wallAttached = false;     // attached to wall flag
    private bool nowRushing = false;    // current rush status
    private bool isRush = false;   // rush action
    private bool isJump = false;   // jump action
    private bool doAirRush = false; // allow rush in the air flag
    private Animator anim;      // animator of rush animation

    // Use this for initialization
    void Start()
    {
        /*verticalCameraEdge = verticalCameraEdge + cameraOffsetVertical;
        horizontalCameraEdge = horizontalCameraEdge + cameraOffsetHorizontal;*/
        //cameraEdge.localPosition = new Vector3(cameraEdge.position.x,-verticalCameraEdge, cameraEdge.position.z);
        //cameraEdge.position = new Vector3(transform.position.x - playerWidth / 2f, cameraEdge.position.y, cameraEdge.position.z);
        cameraOffset = new Vector3(cameraOffsetHorizontal, cameraOffsetVertical, -0.5f);
        cameraObject.transform.position = transform.position + cameraOffset;
        cameraStepHor = cameraStep;
        cameraStepVer = cameraStep;
        anim = GetComponent<Animator>();
        slideSpeed = -slideSpeed;
        curSpeed = lineSpeed;
    }
    void SetWallRunFalse()
    {
        anim.SetBool("wallRunAllow", false);
        isRush = false;
    }
    /*void CameraCorrHorizontal()
    {
        if (facingRight)
        {
            if ((rigidbody2D.position.x - cameraWheel.rigidbody2D.position.x > cameraOffset) && !nowRushing)
            {
                cameraWheel.rigidbody2D.MovePosition(cameraWheel.rigidbody2D.position + new Vector2(cameraStepHor, 0));
                //cameraWheel.transform.position += new Vector3(cameraStepHor, 0, 0);

            }
        }
        else
        {
            if ((rigidbody2D.position.x - cameraWheel.rigidbody2D.position.x < cameraOffset) && !nowRushing)
            {
                cameraWheel.rigidbody2D.MovePosition(cameraWheel.rigidbody2D.position + new Vector2(cameraStepHor, 0));
                //cameraWheel.transform.position += new Vector3(cameraStepHor, 0, 0);
            }
        }
    }*/
    /*public void JumpButton()
    {
        jumpButtonPressed = true;
    }
    public void RushButton()
    {
        rushButtonPressed = true;
    }*/

    void CameraCorrVertical()
    {
        if (cameraObject.transform.position.y - transform.position.y < cameraOffsetVertical)
        {
            cameraObject.transform.position += new Vector3(0, cameraStepVer, 0);
        }
        else
        {
            correctCameraAllow = false;
            cameraObject.transform.position = transform.position + cameraOffset;
        }
    }
    void CameraCorrHorizontal()
    {

    }
    void CameraFollow()
    {
        if (cameraEdgeOut)
        {
            correctCameraAllow = true;
        }
        if (correctCameraAllow)
        {
            CameraCorrVertical();
        }
        if (!grounded && (cameraObject.transform.position.y - transform.position.y > cameraOffsetVertical))
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, transform.position.y + cameraOffsetVertical, cameraObject.transform.position.z);
        //if (!nowRushing)
        cameraObject.transform.position = new Vector3(transform.position.x + cameraOffsetHorizontal, cameraObject.transform.position.y, cameraObject.transform.position.z);
        //else
        {
            //cameraObject.transform.position += new Vector3(cameraStepHor, 0, 0);
        }


    }
    void MakeFirstJump()
    {
        if (grounded && wallAttached)
        {
            rigidbody2D.isKinematic = false;
        }
        rigidbody2D.AddForce(new Vector2(0, jumpForce)); // first jump
        isJump = false;
        doubleJump = false;
        oneAirRushDone = false;
    }

    void Rush()
    {
        if (wallAttached)
        {
            anim.SetBool("wallRunAllow", true);
            rigidbody2D.isKinematic = false;
            if (anim.GetCurrentAnimationClipState(0)[0].clip.name == "wallRun")
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, wallRunSpeed);
            }
            wallRunDone = true;
            //anim.SetBool("wallRunAllow", false);
            /*else
                anim.SetBool("wallRunAllow", false);*/
        }
        else
        {

            if (grounded)
            {
                rigidbody2D.AddForce(new Vector2(rushForce, 0));// ground rush
                anim.Play("rush"); // rush animation
                isRush = false;
                oneAirRushDone = false;
            }
            else
            {
                doAirRush = true;     // allow to jump in the air**
                rigidbody2D.isKinematic = true; // all forces deleted
                if (doAirRush)
                {
                    rigidbody2D.isKinematic = false; // applayng physics
                    rigidbody2D.AddForce(new Vector2(rushForce, 0)); // air rush
                    anim.Play("rush"); // rush animation
                    isRush = false;
                    doAirRush = false;
                    oneAirRushDone = true;
                }
            }
        }

    }
    void Jump()
    {
        if (wallAttached)
        {

            //if (!grounded)
            {
                Flip();
            }
            MakeFirstJump();
        }
        if (grounded && !wallAttached) // single jump
        {
            anim.SetBool("grounded", false);
            MakeFirstJump();

        }
        else // double jump
        {
            doSecondJump = true;     // allow to jump in the air**
            rigidbody2D.isKinematic = true; // all forces deleted
            if (doSecondJump)
            {
                rigidbody2D.isKinematic = false; // applayng physics
                rigidbody2D.AddForce(new Vector2(0, jumpForce)); // second jump
                oneAirRushDone = false;
                isJump = false;
                doSecondJump = false;
            }
        }


    }
    void Go()
    {
        if (!wallAttached)
        {
            rigidbody2D.isKinematic = false;
            rigidbody2D.velocity = new Vector2(lineSpeed, rigidbody2D.velocity.y);
        }
        else
        {

            //rigidbody2D.velocity = Vector2.zero;
            if (anim.GetCurrentAnimationClipState(0)[0].clip.name == "wallSlide")
            {
                if (!grounded)
                {
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, slideSpeed);  //for slide with const speed
                    /*slideSpeed = slideSpeed - slideSpeedInc;                                // for slide with acceleration
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, slideSpeed); // for slide with acceleration*/
                }
                else
                {
                    rigidbody2D.isKinematic = false;
                }
            }
            else
            {
                rigidbody2D.isKinematic = true;

            }

        }
    }
    void Flip()
    {
        cameraOffsetHorizontal = -cameraOffsetHorizontal;
        cameraStepHor = -cameraStepHor;
        groundRushLimit = -groundRushLimit;
        lineSpeed = -lineSpeed;
        curSpeed = -curSpeed;
        rushForce = -rushForce;
        groundRushInc = -groundRushInc;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingRight = !facingRight;
        /*cameraStepHor = -cameraStepHor;
        if (facingRight)
            cameraOffset = cameraOffsetRight;
        else
            cameraOffset = cameraOffsetLeft;*/

    }
    void GroundRush()
    {
        if (makeGroundRush && Mathf.Abs(lineSpeed) <= Mathf.Abs(groundRushLimit))
        {
            lineSpeed += groundRushInc;
        }
        else
            if (Mathf.Abs(lineSpeed) > Mathf.Abs(curSpeed))
                lineSpeed -= groundRushInc;
    }
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);
        wallAttached = Physics2D.OverlapArea(wallCheck.position, new Vector2(wallCheck.position.x + 3 * radius, wallCheck.position.y + playerHigh), whatIsWall);
        cameraEdgeOut = !Physics2D.OverlapArea(cameraEdge.position, new Vector2(cameraEdge.position.x + 2 * horizontalCameraEdge, cameraEdge.position.y + 2 * verticalCameraEdge), whatIsPlayer);
        anim.SetBool("atWall", wallAttached);
        anim.SetBool("grounded", grounded);
        Go();
        if (isRush)
            Rush();
        if (isJump)
            Jump();

    }
    // Update is called once per frame
    void Update()
    {
        CameraFollow();
        GroundRush();
        if (grounded) { oneAirRushDone = false; wallRunDone = false; }
        if (!wallAttached) { wallRunDone = false; } else { lineSpeed = curSpeed; }


        //PlatformMoving();
        //BackGroundMove();
        if (anim.GetCurrentAnimationClipState(0)[0].clip.name != "rush")
        {
            rigidbody2D.gravityScale = 1;
            nowRushing = false;
        }
        else
        {
            rigidbody2D.gravityScale = 0;
            nowRushing = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || wallAttached || !doubleJump) && !nowRushing)
        {
            isJump = true;
            if (!doubleJump && !grounded)
                doubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !nowRushing && !oneAirRushDone && !wallRunDone && (!grounded || (grounded && wallAttached)))
        {
            curSpeed = lineSpeed;
            isRush = true;
        }
        if (Input.GetKey(KeyCode.LeftControl) && grounded)
        {
            makeGroundRush = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            makeGroundRush = false;
        }

    }
    void LateUpdate()
    {

    }


    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "cameraEdge")
            corrVertical = true;
    }*/

    /*void PlatformMoving()
   {
       if (transform.position.x > floorOffset)
       {

           floor.transform.position = new Vector3(floor.transform.position.x + floorStep, floor.transform.position.y, floor.transform.position.z);
           floorOffset += floorStep;
       }
   }
   void BackGroundMove()
   {
       if (transform.position.x > backOffset)
       {

           back.transform.position = new Vector3(back.transform.position.x + backStep, back.transform.position.y, back.transform.position.z);
           backOffset += backStep;
       }
   }*/
}

