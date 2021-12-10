using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)]
    [SerializeField]
    private float movementSmoothing = .05f; // How much to smooth out the movement

    [SerializeField]
    private float runSpeed = 15f;

    private Animator animator;
    private Rigidbody2D rb2D;
    
    private float horizontalMove;
    private bool facingRight = true; // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;
    
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
    private void Update () {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat(Speed, Mathf.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        Move(horizontalMove * Time.fixedDeltaTime);
    }

    private void Move(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb2D.velocity.y);
        // And then smoothing it out and applying it to the character
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}