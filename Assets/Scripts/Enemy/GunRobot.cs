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

    private float timeToFire;
    private float timeToAim;
    private float timeToCooldown;

    public GameObject aimingObject;
    public Vector2 maxAimRotation = new Vector2(30,30);

    private CharacterController2D player;
    private Animator animator;
    private Rigidbody2D rb2D;

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

    private void Awake()
    {
        player = FindObjectOfType<CharacterController2D>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetBool(Move, rb2D.velocity.x > 0.2 || rb2D.velocity.x < -0.2);
   
    }

    private void FixedUpdate()
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

    private void Walk(Vector3 position)
    {
        FacePosition(position);
        rb2D.velocity = new Vector2(transform.position.x < position.x ? Speed : -Speed, rb2D.velocity.y);
    }

    private void Aim(Vector3 position)
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

        float angle = Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        angle = Mathf.Clamp(angle, maxAimRotation.x, maxAimRotation.y);
        aimingObject.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    private void Fire()
    {
        if(barrelPoint != null && bulletPrefab != null)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = barrelPoint.transform.position;
            bullet.Launch(barrelPoint.transform.right);
        }
    }

    private void FacePosition(Vector3 position)
    {
        bool right = transform.position.x < position.x;
        if(right != facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
          
    }
}
