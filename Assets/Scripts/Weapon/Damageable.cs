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

    private FlashFX flash;

    private void Start()
    {
        flash = GetComponent<FlashFX>();
    }

    public void Damage(int damage)
    {
        if (IsDead)
            return;

        health -= damage;

        if (health <= 0)
        {
            IsDead = true;
            OnDeath.Invoke();
            if (destroyOnDeath)
                Destroy(gameObject);
        }
        else if (damage > 0 && flash != null)
        {
            flash.Flash();
        }
    }
}
