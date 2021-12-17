using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public LayerMask mask;
    public AudioClip pickupSound;
    private bool entered = false;

    protected virtual void Entered(GameObject gameObject)
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!entered && ((1 << col.gameObject.layer) & mask) != 0)
        {
            Entered(col.gameObject);
            entered = true;
            AudioSource.PlayClipAtPoint(pickupSound, col.transform.position);
            Destroy(gameObject);
        }
    }
}
