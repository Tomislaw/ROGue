using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    public int health;

    [SerializeField]
    public int maxHealth;

    public bool IsDead { get; private set; }

    [SerializeField]
    private UnityEvent OnDeath;

    public void Damage(int damage)
    {
        if (IsDead)
            return;

        health -= damage;
        if(health <= 0)
        {
            IsDead = true;
            OnDeath.Invoke();
        }
    }
}
