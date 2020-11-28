using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    public void Close()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public void Open()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
