using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Damageable damageable;
    public RectTransform bar;

    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bar == null)
            return;

        if (damageable == null)
        {
            bar.localPosition = -new Vector3(rectTransform.sizeDelta.x, 0, 0);
        }
        else
        {
            var health = 1f - Mathf.Clamp((float) damageable.health /  damageable.maxHealth, 0, 1);
            bar.localPosition = -new Vector3(rectTransform.sizeDelta.x * health, 0, 0);
        }
    }
}
