using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;

    public bool triggerSelf = false;
    public float triggerSelfTime = 0;

    private void Start()
    {
        if(triggerSelf)
        Trigger(triggerSelfTime);
    }

    public void Trigger(float time)
    {
        StartCoroutine(StartTrigger(time));
    }

    IEnumerator StartTrigger(float time)
    {
        yield return new WaitForSeconds(time);
        OnTrigger.Invoke();
    }
}
