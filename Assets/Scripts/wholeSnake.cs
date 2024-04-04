using DG.Tweening;
using UnityEngine;

public class WholeSnake : MonoBehaviour
{
    
    
    private void Awake()
    {

        DOTween.SetTweensCapacity(50000, 50);
    }
    
    public float moveDuration = 4f;
    public void LinearTo()
    {
        
        Transform child = transform.GetChild(0);
        Renderer renderer = child.GetComponent<Renderer>();
        renderer.sortingLayerName = "background";
        for(int i = 1; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            renderer = child.GetComponent<Renderer>();
            renderer.sortingLayerName = "background";
        }
        Vector3 position = new Vector3(transform.position.x, -11.0f, transform.position.z);
        transform.DOMove(position, moveDuration);

        UIController.Instance.Dead();
    }
}