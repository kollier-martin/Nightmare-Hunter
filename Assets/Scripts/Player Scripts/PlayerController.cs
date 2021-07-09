using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 4.5f;           // Amount of force added when the player jumps.
    private const float smoothing = .05f; // How much to smooth out the movement
    private readonly bool airMovement = true;   // Whether or not a player can steer while jumping
    public LayerMask WhatIsGround;    // A mask determining what is ground to the character
    public Transform groundCheck;     // A position marking where to check if the player is grounded.
    const float groundRadius = .4f; // Radius of the overlap circle to determine if grounded
    public bool grounded;             // Whether or not the player is grounded.
    private bool facingRight = true;  // For determining which way the player is currently facing.

    private Collider2D Ladders;  // Colliders of the ladders that can be climbed
    public float climbSpeed = 5;
    private bool doubleJumped;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    // For Mouse Flipping
    Vector3 mousePos;
    Vector2 direction;

    // For Controller Flipping
    Vector3 JoystickPos;
    Vector2 dir;

    private void Awake()
	{
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        try
        {
            Ladders = GameObject.FindGameObjectWithTag("Ladders").GetComponent<Collider2D>();
        }
        catch (System.Exception)
        {
            
        }
    }

	private void FixedUpdate()
	{
        grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
		}
	}
    
    public void Move(float move, bool climb, bool jump)
	{
		// Only control the player if grounded or airControl is turned on
		if (grounded || airMovement)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);

            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothing);

            if (InputSwitch.KeyboardInput)
            {
                // Gets mouse position relative to the player
                mousePos = Input.mousePosition;
                mousePos = CameraSwitch.CurrentCam.ScreenToWorldPoint(mousePos);
                direction = new Vector2(mousePos.x - transform.position.x, mousePos.y = transform.position.y);
            }
            else if (InputSwitch.JoyStickInput)
            {
                // Joystick aiming
                JoystickPos = new Vector3(Input.GetAxis("RJoy X"), Input.GetAxis("RJoy Y"));
                dir = new Vector2(JoystickPos.x, JoystickPos.y);
            }

            // If the input is moving the player right and the player is facing left
            if (((direction.x > 0 ) && !facingRight) || ((dir.x > 0) && !facingRight))
            {
                // Flip X Scale
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right
            else if (((direction.x < 0 ) && facingRight) || ((dir.x < 0) && facingRight))
            {
				// Flip X Scale
				Flip();
            }
		}

        // If the player should jump
        if (grounded)
        {
            doubleJumped = false;
        }

        if (!grounded && !doubleJumped && jump)
        {
            rb.velocity = Vector2.up * jumpForce;
            doubleJumped = true;
        }

        if (grounded && jump)
		{
            // Add a vertical force to the player.
            grounded = false;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Ladders == null)
        {
            climb = false;
        }
        else
        {
            if (rb.IsTouching(Ladders) && climb)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    transform.Translate(Vector3.up * climbSpeed * Time.fixedDeltaTime);
                }
            }
        }
    }


	private void Flip()
	{
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    [ExecuteInEditMode]
    public void OnAnimatorMove()
    {
        var anim = GetComponent<Animator>();
        if (anim == null) return;
        
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (GetComponent<Player>().CurrentController.cutsceneDone == true)
        {
            anim.ApplyBuiltinRootMotion();
            Vector3 v = (anim.deltaPosition * 20 / Time.deltaTime);

            // we preserve the existing  part of the current velocity.
            v.x = rb.velocity.x;
            rb.velocity = v;
        }
        else if (GetComponent<Player>().CurrentController.cutsceneDone == false)
        {
            anim.applyRootMotion = false;
        }
    }
}
