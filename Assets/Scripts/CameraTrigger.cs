using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraTrigger : MonoBehaviour
{
    public bool triggerOnce;
    public UnityEvent OnVisible;

    private bool isTriggered;

    void Update()
    {
        if (triggerOnce && isTriggered)
            return;

        var bounds = Camera.main.WorldToViewportPoint(transform.position);
        var visible =  bounds.x < 1 && bounds.x > 0 && bounds.y < 1 && bounds.y > 0;
        if (visible != isTriggered)
            OnVisible.Invoke();
        isTriggered = visible;
    }
}
