using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public GameObject prefabWeapon;
    protected override void Entered(GameObject character)
    {
        var manager = character.GetComponent<WeaponManager>();
        if (manager != null)
        {
            manager.ChangeWeapon(prefabWeapon);
            Destroy(gameObject);
        }
    }
}
