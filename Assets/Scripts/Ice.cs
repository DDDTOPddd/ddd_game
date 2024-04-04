using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Tui : MonoBehaviour
{
    public virtual int CanMoveTo(Vector2 dir)
    {
        return 0;
    }
}
public class Ice : Tui
{

    public Tilemap groundTilemap;
    public LayerMask stone;
    public LayerMask food;
    protected LayerMask combinedLayerMask;
    public float dropSpeed = 60.0f;
    public bool ShoudDrop = false;
    private void Update()
    {
        if (!IsOnGround())
        {
            gameObject.layer = 0;
            ShoudDrop = true;

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
        RaycastHit2D hits = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, stone);
        RaycastHit2D hitf = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, food);

        if (!hits && !hitf)
        {
            print("Ã»¿¿");
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
                int pd = hitf.collider.GetComponent<Tui>().CanMoveTo(dir);
                if (pd == 1)
                {
                    transform.Translate(dir);
                }
                return pd;
            }

        }

        return 0;

    }

    bool IsOnGround()
    {
        Vector3Int position = groundTilemap.WorldToCell(transform.position);
        if (groundTilemap.HasTile(position)) return true;
        return false;
    }
    void Drop()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.sortingLayerName = "background";
        Vector3 newPosition = transform.position;
        newPosition.y -= dropSpeed * Time.deltaTime;
        transform.position = newPosition;

    }
}
