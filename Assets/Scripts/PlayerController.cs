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


    private Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
 #if UNITY_STANDALONE || UNITY_WEBPLAYER
        /* 
         * keyboard control
         */
#else

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            touchOrigin = myTouch.position;
        }
#endif

	}
    void FixedUpdate()
    {

    }
}
