using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private bool isVisible;
    public void Show(bool show)
    {
        isVisible = show;

    }

    private void Start()
    {
        Cursor.visible = isVisible;
    }

    private void Update()
    {
        Cursor.visible = isVisible;
    }
}
