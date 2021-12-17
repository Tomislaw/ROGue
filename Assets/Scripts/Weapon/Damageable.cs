using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public int health;

    public int maxHealth;

    public bool destroyOnDeath;

    public bool IsDead { get; private set; }

    [SerializeField]
    private UnityEvent OnDeath;

    [SerializeField]
    private UnityEvent OnDamage;

    private FlashFX flash;

    public List<GameObject> spawnOnDeath = new List<GameObject>();

    private void Start()
    {
        flash = GetComponent<FlashFX>();
    }

    public void Kill()
    {
        Damage(health);
    }

    public void Damage(int damage)
    {
        if (IsDead)
            return;

        health -= damage;

        OnDamage.Invoke();

        if (health <= 0)
        {
            IsDead = true;
            OnDeath.Invoke();
            if (destroyOnDeath)
                Destroy(gameObject);

            foreach(var go in spawnOnDeath)
            {
                var g = Instantiate(go);
                g.transform.position = transform.position;
            }
        }
        else if (damage > 0 && flash != null)
        {

            flash.Flash();
        }
    }
}
