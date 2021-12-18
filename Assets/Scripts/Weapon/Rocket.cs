using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float range = 3f;
    public int damage = 30;
    public float speed = 10f;

    public float aimSmoothing = 10;
    private float aimVelocity;

    [SerializeField]
    private float lifeTime = 2f;

    private GameObject target;
    private Rigidbody2D rb;
    private Damageable damageable;

    private bool isExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<CharacterController2D>().gameObject;
        damageable = GetComponent<Damageable>();
        damageable.destroyOnDeath = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null)
        {
            var dir = transform.position - target.transform.position;
            dir.Normalize();

            var targetAngle = Mathf.Round(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            var smoothedAngle = Mathf.SmoothDampAngle(
                    transform.eulerAngles.z,
                    targetAngle,
                    ref aimVelocity,
                    aimSmoothing);


            if (transform.eulerAngles.y > 90)
                smoothedAngle = -smoothedAngle + 180;
            transform.rotation = Quaternion.Euler(0f, 0f, smoothedAngle);


            rb.velocity = -transform.right * speed;
        }

        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0) {
            damageable.Kill();
        }
    }

    public void Explode()
    {
        if (isExploded)
            return;
        isExploded = true;

        var damagedList = new List<Damageable>();
        foreach (var item in Physics2D.OverlapCircleAll(transform.position, range))
        {
            if (item.gameObject != target)
                continue;

            var damageable = item.GetComponent<Damageable>();
            if (damagedList.Contains(damageable))
                continue;
            damagedList.Add(damageable);

            if (damageable != null)
                damageable.Damage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        damageable.Kill();
    }
}
