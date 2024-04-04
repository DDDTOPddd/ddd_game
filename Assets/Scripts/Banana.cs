using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Banana : Tui
{
    private AudioSource audioSource;
    public AudioClip DropAudio;
    public Tilemap groundTilemap;
    public LayerMask stone;
    public LayerMask food;
    protected  LayerMask combinedLayerMask;
    public float dropSpeed = 60.0f;
    protected bool ShoudDrop = false;
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!IsOnGround())
        {
            gameObject.layer = 0;
            audioSource.clip = DropAudio;
            audioSource.Play();
            ShoudDrop = true;
            UIController.Instance.FoodBeDrop();
        }
        if (ShoudDrop)
        {
            Drop();
        }
        if (transform.position.y < -9)
        {
            Destroy(gameObject);
        }
    }
    public override int CanMoveTo(Vector2 dir)
    {
        if (ShoudDrop)
        {
            return 0;
        }
        combinedLayerMask = food | stone;
        RaycastHit2D hits = Physics2D.Raycast(transform.position+(Vector3)dir*0.5f, dir, 0.5f, stone);
        RaycastHit2D hitf = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, food);

        if (!hits&&!hitf)
        {
            transform.Translate(dir);
            return 1;
        }
        else
        {
            if (hits)
            {
                return 2;
            }
            if (hitf)
            {
                int pd=hitf.collider.GetComponent<Tui>().CanMoveTo(dir);
                if(pd==1)
                {
                    transform.Translate(dir);
                }
                return pd;
            }

        }
        return 0;

    }

    public virtual int BeEat()
    {
        Destroy(gameObject);
        return 1;
    }
    bool IsOnGround()
    {
        Vector3Int position = groundTilemap.WorldToCell(transform.position);
        if (groundTilemap.HasTile(position)) return true;
        return false;
    }
    void Drop()
    {
        
        Transform child = transform.GetChild(0);
        Renderer renderer = child.GetComponent<Renderer>();
        renderer.sortingLayerName = "background";
        Vector3 newPosition = transform.position;
        newPosition.y -= dropSpeed * 10 *Time.deltaTime;
        transform.position = newPosition;
        
    }

    
}
