using System;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;

    }
}
