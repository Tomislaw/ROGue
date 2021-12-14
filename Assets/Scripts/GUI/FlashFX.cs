using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFX : MonoBehaviour
{
    public Material FlashMaterial;
    public float FlashSpeed = 0.1f;

    private Coroutine flashRoutine;

    private Dictionary<SpriteRenderer, Material> _materials = new Dictionary<SpriteRenderer,Material>();
    void Start()
    {
        _materials.Clear();
        var components = GetComponentsInChildren<SpriteRenderer>();
        foreach(var component in components)
        {
            var material = component.material;
            _materials.Add(component, material);
        }
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach(var pair in _materials)
        {
            pair.Key.material = FlashMaterial;
        }
        yield return new WaitForSeconds(FlashSpeed);

        foreach (var pair in _materials)
        {
            pair.Key.material = pair.Value;
        }

        flashRoutine = null;
    }
}
