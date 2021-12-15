using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class GunRobot : MonoBehaviour
{
    public float EngageDistance;
    public float FireTime;
    public float AimTime;
    public float CooldownTime;
    public float Speed;

    public float MovementSmoothing = 0.05f;
    public float AimSmoothing = 0.05f;

    private float timeToFire;
    private float timeToAim;
    private float timeToCooldown;

    public GameObject aimingObject;
    public Vector2 maxAimRotation = new Vector2(30,30);

    protected Vector3 velocity = Vector3.zero;
    protected float velocityAngle = 0;

    protected CharacterController2D player;
    protected Animator animator;
    protected Rigidbody2D rb2D;

    [SerializeField]
    private GameObject barrelPoint;

    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private bool facingRight = false;

    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int FirePhase = Animator.StringToHash("FirePhase");

    [SerializeField]
    private State state = State.Idle;

    public enum State
    {
        Idle,
        Walk,
        Aim,
        AimLocked,
        Fire,
        Cooldown
    }

    protected void Awake()
    {
        player = FindObjectOfType<CharacterController2D>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        animator.SetBool(Move, rb2D.velocity.x > 0.2 || rb2D.velocity.x < -0.2);
   
    }

    protected void FixedUpdate()
    {
        if (player == null)
            return;

        switch (state)
        {
            case State.Idle:
                state = State.Walk;
                aimingObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0);
                break;
            case State.Walk:

                if (Vector2.Distance(transform.position, player.transform.position) < EngageDistance)
                {

                    state = State.Aim;
                    timeToAim = AimTime;
                }
                else
                {
                    Walk(player.transform.position);
                }
                break;

            case State.Aim:
                timeToAim -= Time.fixedDeltaTime;
                Aim(player.transform.position);
                animator.SetInteger(FirePhase, 1);

                if (timeToAim < 0)
                {
                    state = State.AimLocked;
                    timeToFire = FireTime;
                }

                break;

            case State.AimLocked:
                timeToFire -= Time.fixedDeltaTime;
                animator.SetInteger(FirePhase, 2);

                if (timeToFire < 0)
                {
                    state = State.Fire;
                }

                break;

            case State.Fire:
                timeToCooldown = CooldownTime;
                animator.SetInteger(FirePhase, 3);
                Fire();
                state = State.Cooldown;
                break;

            case State.Cooldown:
                timeToCooldown -= Time.fixedDeltaTime;

                if (timeToCooldown < 0)
                    if (Vector2.Distance(transform.position, player.transform.position) < EngageDistance)
                    {
                        animator.SetInteger(FirePhase, 1);
                        state = State.Aim;
                        timeToAim = AimTime;
                    }
                    else
                    {
                        state = State.Idle;
                        animator.SetInteger(FirePhase, 0);
                    }
                        

                break;

        }
    }

    protected virtual void Walk(Vector3 position)
    {
        FacePosition(position);
        var targetVelocity = new Vector2(transform.position.x < position.x ? Speed : -Speed, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, MovementSmoothing);
    }

    protected virtual void Aim(Vector3 position)
    {
        if (aimingObject == null)
            return;

        FacePosition(position);

        var dir = aimingObject.transform.position - position;
        dir.Normalize();

        if (transform.eulerAngles.y > 90)
        {
            dir.x *= -1;
        }
     
        var targetAngle = Mathf.Round(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        var smoothedAngle = Mathf.SmoothDampAngle(
                aimingObject.transform.eulerAngles.z,
                Mathf.Clamp(targetAngle, maxAimRotation.x, maxAimRotation.y),
                ref velocityAngle,
                AimSmoothing);
        aimingObject.transform.localRotation = Quaternion.Euler(0f, 0f, smoothedAngle);
    }
    protected void Fire()
    {
        if(barrelPoint != null && bulletPrefab != null)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = barrelPoint.transform.position;
            bullet.transform.rotation = barrelPoint.transform.rotation;
        }
    }

    protected void FacePosition(Vector3 position)
    {
        bool right = transform.position.x < position.x;
        if(right != facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
          
    }
}
