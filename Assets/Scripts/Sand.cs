using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    public Sprite small;
    public Sprite big;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = small ;
    }
    public void Tobig()
    {
        spriteRenderer.sprite = big;
    }
    public void Tosmall()
    {
        spriteRenderer.sprite = small;
    }
}
