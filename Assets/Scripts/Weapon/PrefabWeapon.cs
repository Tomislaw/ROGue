using UnityEngine;

public class PrefabWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float rateOfFire = 0.3f;

    [SerializeField]
    private float spread = 1f;

    private float timeToNextBullet = 0;

    private bool isFiring = false;

    private void Update()
    {
        isFiring = Input.GetButton("Fire1");
    }

    private void FixedUpdate()
    {
        if (timeToNextBullet >= 0)
            timeToNextBullet -= Time.fixedDeltaTime;
        else if (isFiring)
        {
            Shoot();
            timeToNextBullet = rateOfFire;
        }

    }

    protected virtual void Shoot()
    {
        if(bulletPrefab != null)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = firePoint.transform.position;
            bullet.transform.rotation = firePoint.transform.rotation;
            bullet.transform.Rotate(new Vector3(0,0,Random.Range(-spread,spread)));
        }
          
    }
}