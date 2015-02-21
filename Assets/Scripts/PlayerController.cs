using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float playerLineSpeed;  // line speed of player
    public float jumpForceX;       // jump force by X axis
    public float jumpForceY;       // jump force by Y axis
    public float slideSpeed;       // speed of slide by wall
    public float playerHeight;       // height of player
    public float playerWidth;      // width of player
    public LayerMask whatIsGround; // reference to floor
    public LayerMask whatIsWall;   // reference to wall
    public Transform groundCheck;  // ground check rectangle
    public Transform wallCheck;    // wall check rectangle
    public float wallRunSpeed;     // speed of wall run
    public float wallRunDistance;
    public float rushSpeed;
    public float rushDistance;



    private float playerCurLineSpeed; // current players speed
    private float radius = 0.06f;     // radius of groundcheck circle
    private float wallCheckWidth;     // width of rectangle for wallCheck
    private float groundCheckWidth;
    private bool isJump = false;      // jump button pressed;
    private bool grounded = false;    // player is grounded
    private bool wallAttached = false; //player is attached to wall
    private bool doubleJump = true;   // double jump is allow
    private Animator anim;
    private bool isRush_wallRun = false;
    private bool nowRushing = false;
    private bool nowWallRuning = false;
    private float startPosition;   // start position of rush or wall run
    private bool wallRunDone = false;
    private bool airRushDone = false;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        wallCheckWidth = 2 * radius;
        groundCheckWidth = playerWidth;
        float groundCheckCoordY = transform.position.y - (playerHeight / 2f - radius);    // vertical coordinates of groundCheck object
        float wallCheckCoordX = transform.position.x + (playerWidth / 2f - radius); // horizontal coordinates of wallCheck object
        groundCheck.position = new Vector3(-wallCheckCoordX, groundCheckCoordY, transform.position.z);  // put groundCheck object in correct position
        wallCheck.position = new Vector3(wallCheckCoordX, groundCheckCoordY, transform.position.z);         // put wallCheck object in correct position
        playerCurLineSpeed = playerLineSpeed; // initializing current player speed
    }


    void Jump(bool firstJump)
    {
        airRushDone = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForceY); // vertical component of jump speed
            if (firstJump)                         //  change line speed if it`s a first jump
                playerCurLineSpeed += jumpForceX;  //  horizontal component of jump force
    }
    void WallSlide()
    {
        if (!grounded)
        {
            rigidbody2D.velocity = new Vector2(0, -slideSpeed);  // player slide
        }
    }
    void WallRun()
    {
        if ((Mathf.Abs(transform.position.y - startPosition) < wallRunDistance) && wallAttached)
            rigidbody2D.velocity = new Vector2(0, wallRunSpeed);
        else
        {
            nowWallRuning = false;
            isRush_wallRun = false;
            wallRunDone = true;
        }
    }
    void Rush()
    {
        if ((Mathf.Abs(transform.position.x - startPosition) < rushDistance) && !wallAttached)
            rigidbody2D.velocity = new Vector2(rushSpeed, 0);
        else
        {
            if (!grounded)
            {
                airRushDone = true;
            }
            nowRushing = false;
            isRush_wallRun = false;
        }
    }
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        playerCurLineSpeed = -playerCurLineSpeed;
        rushSpeed = -rushSpeed;
        playerLineSpeed = -playerLineSpeed;
        wallCheckWidth = -wallCheckWidth;
        groundCheckWidth = -groundCheckWidth;
    }

	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            isJump = true;  // jump button pressed
        /*else
            isJump = false;*/
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isRush_wallRun = true;
        /*else
            isRush_wallRun = false;*/


        if (isRush_wallRun && !(nowRushing || nowWallRuning))
        {

            if (wallAttached && !wallRunDone)
            {
                startPosition = transform.position.y;
                nowWallRuning = true;
            }
            else
                isRush_wallRun = false;
            if ((grounded || !airRushDone) && !wallAttached)
            {
                nowRushing = true;
                startPosition = transform.position.x;
            }
        }

	}
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapArea(groundCheck.position, new Vector2(groundCheck.position.x + groundCheckWidth, groundCheck.position.y - radius), whatIsGround); // check grounded of player
        anim.SetBool("grounded", grounded);
        wallAttached = Physics2D.OverlapArea(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckWidth, wallCheck.position.y + playerHeight), whatIsWall); // check wall attach of player
        anim.SetBool("wallAttached", wallAttached);

        if (grounded)
        {
            playerCurLineSpeed = playerLineSpeed;   // correction of line speed after jump (dispose of horizontal component of jump speed)
            doubleJump = true;                      // if on the ground allow double jump
            anim.SetBool("doubleJump", false); 
        }
        if (wallAttached)
        {
            WallSlide();
        }
        else
        {
            wallRunDone = false;
            rigidbody2D.velocity = new Vector2(playerCurLineSpeed, rigidbody2D.velocity.y);  // movig player with const speed
        }

        if (isJump && !(nowRushing || nowWallRuning))                //  if jump button pressed
        {
            if (grounded)          // if player on the ground
            {
                Jump(true);        // make first jump (flag "true" is telling that it`s a first jump)
            }
            else if (doubleJump)   // checking if double jump allow
            {
                Jump(false);         // do the second jump (flag "false" is telling that it`s not a first jump)
                doubleJump = false;  // forbid double jump
                anim.SetBool("doubleJump", true);
            }
            if (wallAttached)
            {
                Flip();
                Jump(true);
                doubleJump = true;
                anim.SetBool("doubleJump", false);
            }
            isJump = false;
        }
        if (nowWallRuning)
            WallRun();
        
        if (nowRushing )
            Rush();
    }
}
