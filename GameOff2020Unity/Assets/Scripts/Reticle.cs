using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField] private Texture2D crosshair;

    private Vector2 hotspot;

    private void Start()
    {
        hotspot = new Vector2(crosshair.width / 2, crosshair.height / 2);
        Cursor.SetCursor(crosshair, hotspot, CursorMode.Auto);
    }
}
