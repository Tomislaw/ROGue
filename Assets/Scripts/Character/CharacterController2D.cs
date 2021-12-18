using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)]
    public float movementSmoothing = .05f; // How much to smooth out the movement

    public float runSpeed = 15f;

    public float jumpSpeed = 15f;


    public Collider2D groundCheck;

    public PrefabWeapon weapon;

    public Vector2 maxRotation = new Vector2(-30, 30);
    public GameObject armTransform;

    public LayerMask groundLayerMask;

    private Animator animator;
    private Rigidbody2D rb2D;
    
    private float horizontalMove;
    private float verticalMove;
    private bool isJumping = false;
    private bool facingRight = true; // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Killed = Animator.StringToHash("Dead");
    private bool isDead = false;
    private bool shoot = false;


    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
    private void Update () {
        if (isDead)
            return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
        shoot = Input.GetButton("Fire1");

        animator.SetFloat(Speed, Mathf.Abs(horizontalMove));
        animator.SetBool(Jumping, !IsGrounded());
    }

    private void FixedUpdate()
    {
        if(isDead) 
            return;

        if (isJumping)
        {
            if (rb2D.velocity.y <= 0 && IsGrounded())
                isJumping = false;
        }

        if(weapon != null)
            weapon.isFiring = shoot;

        Move(horizontalMove * Time.fixedDeltaTime);
        if (!isJumping && verticalMove > 0 && IsGrounded())
            Jump();
        else if (verticalMove < 0)
            DropFromPlatform();

        UpdateArms();
    }

    private bool IsGrounded()
    {
        if(groundCheck==null)
            return false;

        return groundCheck.IsTouchingLayers(groundLayerMask);
    }

    private void Jump()
    {
        isJumping = true;
        rb2D.velocity = new Vector3(rb2D.velocity.x, 0,0);
        rb2D.AddForce(new Vector2(0, jumpSpeed));
    }

    public void Dead()
    {
        isDead = true;
        animator.SetBool(Killed, isDead);
        if (weapon != null)
        {
            weapon.isFiring = false;
            weapon.enabled = false;
        }
        armTransform.transform.localRotation = Quaternion.Euler(0f, 0f, 0);

    }

    private void DropFromPlatform()
    {
        // todo
    }

    private void Move(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);

    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (armTransform == null || weapon == null)
            return;

        foreach (Transform child in armTransform.transform)
        {
            Destroy(child.gameObject);
        }
        this.weapon = Instantiate(weapon, armTransform.transform).GetComponent<PrefabWeapon>();

    }

    private void UpdateArms()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        FacePosition(pos);
        var dir = pos - armTransform.transform.position;
        dir.Normalize();

        if (transform.eulerAngles.y > 90)
        {
            dir.x *= -1;
        }

        float angle = Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        angle = Mathf.Clamp(angle, maxRotation.x, maxRotation.y);
        armTransform.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected void FacePosition(Vector3 position)
    {
        bool right = transform.position.x < position.x;
        if (right != facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }

    }
}