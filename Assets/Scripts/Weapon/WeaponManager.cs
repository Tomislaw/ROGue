using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Vector2 maxRotation = new Vector2 (-30,30);
    public GameObject armTransform;
    public CharacterController2D controller2D;
    public void ChangeWeapon(GameObject weapon)
    {
        if (armTransform == null || weapon == null)
            return;

        foreach (Transform child in armTransform.transform)
        {
            Destroy(child.gameObject);
        }
        var newWeapon = Instantiate(weapon, armTransform.transform);
        if (controller2D != null && newWeapon.GetComponent<PrefabWeapon>())
            controller2D.weapon = newWeapon.GetComponent<PrefabWeapon>();

    }

    private void FixedUpdate()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = pos - armTransform.transform.position;
        dir.Normalize();

        if (transform.eulerAngles.y > 90)
        {
            dir.x *= -1;
        }

        float angle = Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        angle = Mathf.Clamp(angle, maxRotation.x, maxRotation.y);
        armTransform.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
