using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float liveTime = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        liveTime -= Time.fixedDeltaTime;
        if (liveTime < 0)
            Destroy(gameObject);
    }
}
