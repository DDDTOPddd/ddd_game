using DG.Tweening;
using UnityEngine;

public class WholeSnake : MonoBehaviour
{
    private void Awake()
    {
        DOTween.SetTweensCapacity(25000, 50);
    }
    public float moveDuration = 4f;
    public void LinearTo()
    {
        Vector3 position = new Vector3(transform.position.x, -9.0f, transform.position.z);
        transform.DOMove(position, moveDuration);


    }
}