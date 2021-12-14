
using UnityEngine;

public class HealthPickup : Pickup
{
    public int health = 30;
    protected override void Entered(GameObject gameObject)
    {
        var damageable = gameObject.GetComponent<Damageable>();
        if(damageable != null)
        {
            damageable.health = Mathf.Clamp(damageable.health + health, 0, damageable.maxHealth);
        }
    }
}