using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum HoleState
{
    on,off
}
public class Hole : MonoBehaviour
{
    private HoleState holeState = HoleState.off;
    public Sprite big;
    public Sprite small;
    private SpriteRenderer spriteRenderer;
    private Collider2D holeCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        holeCollider = GetComponent<BoxCollider2D>();
        CloseHole();

    }
    private void Update()
    {
        switch (holeState)
        {
            case HoleState.off:
                break;
            case HoleState.on:
                OnUpdate();
                break;

        }
    }

    void OnUpdate()
    {

    }

    public void OpenHole()
    {
        holeState = HoleState.on;
        spriteRenderer.sprite = big;
        holeCollider.enabled = true; 
    }
    public void CloseHole()
    {
        holeState = HoleState.off;
        spriteRenderer.sprite = small;
        holeCollider.enabled = false; 
    }
    public bool IsOpen()
    {
        return (holeState == HoleState.on);
    }
}
