using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float playerLineSpeed;  // line speed of player
    public float jumpForceX;       // jump force by X axis
    public float jumpForceY;       // jump force by Y axis
    public float wallRunSpeed;     // speed of wall run
    public float slideSpeed;       // speed of slide by wall
    public float playerHigh;       // high of player
    public float playerWidth;      // width of player
    public float groundRushInc;    // increment of line sdeed by doing ground rush
    public float groundRushLimit;  // max line speed whed rushing
    public LayerMask whatIsGround; // reference to floor
    public LayerMask whatIsWall;   // reference to wall
    public Transform groundCheck;  // ground check circle
    public Transform wallCheck;    // wall check circle

    private float playerCurLineSpeed; // current players speed
    private float radius = 0.02f;     // radius of groundcheck circle
    private bool isJump = false;      // jump button pressed;
    private bool grounded = false;    // player is grounded
    private bool doubleJump = true;   // double jump is allow


    void Jump(bool firstJump)
    {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForceY); // vertical component of jump speed
            if (firstJump)                         //  change line speed if it`s a first jump
                playerCurLineSpeed += jumpForceX;  //  horizontal component of jump force
    }
	// Use this for initialization
	void Start () 
    {
        playerCurLineSpeed = playerLineSpeed; // initializing current player speed
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            isJump = true;  // jump button pressed
	}
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround); // check grounded of player
        if (grounded)
        {
            playerCurLineSpeed = playerLineSpeed;   // correction of line speed after jump (dispose of horizontal component of jump speed)
            doubleJump = true;                      // if on the ground allow double jump
        }

        rigidbody2D.velocity = new Vector2(playerCurLineSpeed, rigidbody2D.velocity.y);  // movig player with const speed
        if (isJump)                //  if jump button pressed
        {
            if (grounded)          // if player on the ground
            {
                Jump(true);        // make first jump (flag "true" is telling that it`s a first jump)
            }
            else if (doubleJump)   // checking if double jump allow
            {
                Jump(false);         // do the second jump (flag "false" is telling that it`s not a first jump)
                doubleJump = false;  // forbid double jump
            }
            isJump = false;          // reset info about jump button pressed
        }
    }
}
