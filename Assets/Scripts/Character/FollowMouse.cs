using System;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject cursor;
    public GameObject midpoint;
    public GameObject target;

    public void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        cursor.transform.position = mousePosition;
        midpoint.transform.position = (cursor.transform.position +target.transform.position)/2;


    }
}
