using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance = null;

    public float playerLineSpeed;  // line speed of player
    public float jumpForceY;       // jump force by Y axis
    public float slideSpeed;       // speed of slide by wall
    public int playerPixelHeight;       // height of player
    public int playerPixelWidth;      // width of player
    public float scale;
    public int pixelsPerUnits;
    public LayerMask whatIsGround; // reference to floor
    public LayerMask whatIsWall;   // reference to wall
    public LayerMask whatIsRope;
    public float wallRunSpeed;     // speed of wall run
    public float wallRunDistance;
    public float wallChekOffset;
    public float rushSpeed;
    public float rushDistance;
    public float climbSpeed;


    private GameObject levelManager;
    private float playerWidth;
    private float playerHeight;
    private float playerCurLineSpeed; // current players speed
    private float ropeCheckDistance = 1.5f;
    private float groundCheckDirect = -1f;
    private float wallCheckDirect = 1f;
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
    private bool atRope = false;
    private bool facingRight = true;
    private bool wallClimbAllowHor = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            if (instance != this)
                Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
        levelManager = GameObject.FindWithTag("levelManager");
    }
    // Use this for initialization
    void Start()
    {
        playerHeight = (float)playerPixelHeight / (float)pixelsPerUnits * scale;
        playerWidth = (float)playerPixelWidth / (float)pixelsPerUnits * scale;
        anim = GetComponent<Animator>();
        playerCurLineSpeed = playerLineSpeed; // initializing current player speed
    }

    void Go(float speed)
    {
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);  // movig player with const speed
    }
    void Jump()
    {
        airRushDone = false;
            rigidbody2D.velocity = new Vector2(playerCurLineSpeed, jumpForceY); // vertical component of jump speed 
    }
    void WallSlide()
    {
        if (!grounded)
        {
            rigidbody2D.velocity = new Vector2(0, -slideSpeed);  // player slide
        }
    }
    void WallClimb()
    {

        if (!grounded)
        {
            float speed;
            if (facingRight)
            {
                speed = climbSpeed;
            }
            else
            {
                speed = -climbSpeed;
            }


            RaycastHit2D climbHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - playerHeight / 2), new Vector2(wallCheckDirect, 0), playerWidth / 2, whatIsWall);


            if (climbHit.collider != null)
            {

                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y - playerHeight / 2), climbHit.point, Color.red);
                wallClimbAllowHor = true;
                rigidbody2D.velocity = new Vector2(0, climbSpeed);
            }
            else
                if (wallClimbAllowHor)
                    rigidbody2D.velocity = new Vector2(speed, 0);
        }
        else
        {
            wallClimbAllowHor = false;
        }

    }
    void RopeSlide(Collider2D rope)
    {
        
        doubleJump = true;
        rigidbody2D.AddForce(new Vector2(0, -Physics2D.gravity.y)); // force to destroy gravity
        float angle;
        angle = rope.gameObject.transform.rotation.eulerAngles.z;
        if (angle > 180)
        {
            angle = 360 - angle;
        }

        float ySpeed = Mathf.Abs(playerCurLineSpeed)* Mathf.Tan(angle / 180 * Mathf.PI);
        rigidbody2D.velocity = new Vector2(playerCurLineSpeed, -ySpeed); 
    }
    void WallRun()
    {
        RaycastHit2D headHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + playerHeight / 2), new Vector2(0, 1f), ropeCheckDistance, whatIsGround);
        if (Mathf.Abs(transform.position.y - startPosition) < wallRunDistance && wallAttached && headHit.collider == null)
            rigidbody2D.velocity = new Vector2(0, wallRunSpeed);
        else
        {
            /*if (!wallAttached)
                wallClimbAllow = true;*/
            nowWallRuning = false;
            anim.SetBool("nowWallRuning", nowWallRuning);
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
            anim.SetBool("nowRushing", nowRushing);
            isRush_wallRun = false;
            Go(playerCurLineSpeed);
        }
    }
    void Flip()
    {
        wallCheckDirect = -wallCheckDirect;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        playerCurLineSpeed = -playerCurLineSpeed;
        rushSpeed = -rushSpeed;
        //playerLineSpeed = -playerLineSpeed;
        //wallCheckWidth = -wallCheckWidth;
        //groundCheckWidth = -groundCheckWidth;
        facingRight = !facingRight;
    }

	
	// Update is called once per frame
	void Update () 
    {
        if (GameManager.instance.Pause)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRush_wallRun = true;
        }
        Touch myTouch = new Touch();
        if (Input.touchCount > 0)
        {
            myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began)
            {
                if (myTouch.position.y < Screen.height * 3 / 4)
                    if (myTouch.position.x < Screen.width / 2)
                    {
                            if (!GameManager.instance.ControlSwitched)
                                isJump = true;  // jump button pressed
                            else
                                isRush_wallRun = true;
                    }
                    else
                        if (!GameManager.instance.ControlSwitched)
                            isRush_wallRun = true;
                        else
                            isJump = true;  // jump button pressed

            }
        }
        if (isRush_wallRun && !(nowRushing || nowWallRuning))
        {

            if (wallAttached && !wallRunDone && levelManager.GetComponent<LevelManager>().allowWallRun)
            {
                startPosition = transform.position.y;
                nowWallRuning = true;
                anim.SetBool("nowWallRuning", nowWallRuning);
                
            }
            else
                isRush_wallRun = false;
            if ((grounded || !airRushDone) && !wallAttached && !atRope && levelManager.GetComponent<LevelManager>().allowRush)
            {
                nowRushing = true;
                anim.SetBool("nowRushing", nowRushing);
                startPosition = transform.position.x;
            }
        }

	}
    void FixedUpdate()
    {
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + playerHeight / 2), new Vector2(transform.position.x, transform.position.y + playerHeight / 2 + 3 * ropeCheckDistance));

        if (GameManager.instance.Pause)
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            rigidbody2D.gravityScale = 0;
            return;
        }
        else rigidbody2D.gravityScale = 1;
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, new Vector2(0, groundCheckDirect), playerHeight/2, whatIsGround);
        if (hitGround.collider != null)
        {
            grounded = true;
        }
        else
            grounded = false;
        RaycastHit2D hitWall = Physics2D.Raycast(new Vector2 (transform.position.x, transform.position.y + wallChekOffset), new Vector2(wallCheckDirect, 0), playerWidth / 2, whatIsWall);
        if (hitWall.collider != null)
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + wallChekOffset), hitWall.point, Color.red);

            wallAttached = true;
        }
        else
            wallAttached = false;
        anim.SetBool("grounded", grounded);
        anim.SetBool("wallAttached", wallAttached);
        RaycastHit2D takeRope = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + playerHeight / 2), new Vector2(0, 1f), ropeCheckDistance, whatIsRope);
        if (takeRope.collider != null && ((takeRope.collider.gameObject.transform.rotation.eulerAngles.z > 180 && facingRight) || (takeRope.collider.gameObject.transform.rotation.eulerAngles.z < 180 && !facingRight)))
        {
            atRope = true;
            RopeSlide(takeRope.collider);
        }
        else
            atRope = false;
        anim.SetBool("ropeAttached", atRope);
        if (grounded)
        {
            Go(playerCurLineSpeed);
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
        }

        if (isJump && !(nowRushing || nowWallRuning))                //  if jump button pressed
        {
            if (grounded || atRope)          // if player on the ground
            {
                Jump();        // make first jump
            }
            else if (doubleJump && levelManager.GetComponent<LevelManager>().allowDoubleJump)   // checking if double jump allow
            {
                Jump();         // do the second jump (flag "false" is telling that it`s not a first jump)
                doubleJump = false;  // forbid double jump
                anim.SetBool("doubleJump", true);
            }
            if (wallAttached)
            {
                Flip();
                Jump();
                doubleJump = true;
                anim.SetBool("doubleJump", false);
            }
            isJump = false;
        }
        if (nowWallRuning)
            WallRun();
        
        if (nowRushing)
            Rush();
        if (!wallAttached)
        {
            WallClimb();
        }
    }
}
