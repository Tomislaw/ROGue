using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private int damage = 40;

    [SerializeField]
    private float lifeTime = 2f;

    private Rigidbody2D rb;

    [SerializeField]
    private GameObject impactEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
     
    }

    private void FixedUpdate()
    {

        if(rb != null)
        {
            rb.velocity = transform.right * speed;
        }

        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        var enemy = hitInfo.GetComponent<Damageable>();
        if (enemy != null)
        {
            enemy.Damage(damage);
        }
        
        if (impactEffect != null)
        {
            var point = hitInfo.ClosestPoint(transform.position);
            var hitEffect = Instantiate(impactEffect);
            hitEffect.transform.position = point;
        }
            
        Destroy(gameObject);
    }
}