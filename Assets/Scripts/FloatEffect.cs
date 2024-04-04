using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 0.05f; // ���
    public float frequency = 1f; // Ƶ��

    private float originalLocalY;

    void Start()
    {
        originalLocalY = transform.localPosition.y;
    }

    void Update()
    {
        // ��Sin������������ԭʼYλ�û����ϸ���
        transform.localPosition = new Vector3(transform.localPosition.x,
                                              originalLocalY + Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude,
                                              transform.localPosition.z);
    }
}