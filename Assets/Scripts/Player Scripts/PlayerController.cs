using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float jumpForce = 5f;           // Amount of force added when the player jumps.
    private const float smoothing = .05f; // How much to smooth out the movement
    private readonly bool airMovement = true;   // Whether or not a player can steer while jumping
    public LayerMask WhatIsGround;    // A mask determining what is ground to the character
    public Transform groundCheck;     // A position marking where to check if the player is grounded.
    const float groundRadius = .4f; // Radius of the overlap circle to determine if grounded
    public bool grounded;             // Whether or not the player is grounded.
    private bool facingRight = true;  // For determining which way the player is currently facing.

    private Collider2D Ladders;  // Colliders of the ladders that can be climbed
    public float climbSpeed;
    private bool doubleJumped;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    
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
                grounded = true;
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

            // Gets mouse position relative to the player
            var mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            var direction = new Vector2(mousePos.x - transform.position.x, mousePos.y = transform.position.y);

            // If the input is moving the player right and the player is facing left
            if (direction.x > 1 && !facingRight)
            {
                // Flip X Scale
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right
            else if (direction.x < -1 && facingRight)
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
            rb.velocity = Vector2.up * jumpForce; // or AddForce(new Vector2(0f, jumpForce));
            doubleJumped = true;
        }

        if (grounded && jump)
		{
            // Add a vertical force to the player.
            grounded = false;
            rb.velocity = Vector2.up * jumpForce; // or AddForce(new Vector2(0f, jumpForce));
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

        // Flip
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        //transform.Rotate(0f, 180f, 0f);
	}
}
