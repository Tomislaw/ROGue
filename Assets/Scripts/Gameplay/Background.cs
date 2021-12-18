using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = new Vector3(
            Camera.main.transform.position.x, 
            Camera.main.transform.position.y, 
            transform.position.z);
    }
}
