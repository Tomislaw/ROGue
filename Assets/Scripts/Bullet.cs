using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private int damage = 40;

    private Rigidbody2D rb;

    [SerializeField]
    private GameObject impactEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Launch(Vector2 dir)
    {
        rb.velocity = dir * speed;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        var enemy = hitInfo.GetComponent<Damageable>();
        if (enemy != null)
        {
            enemy.Damage(damage);
        }
        
        Destroy(gameObject);
    }
}