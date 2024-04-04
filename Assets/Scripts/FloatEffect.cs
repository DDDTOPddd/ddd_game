using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 0.05f; // 振幅
    public float frequency = 1f; // 频率

    private float originalLocalY;

    void Start()
    {
        originalLocalY = transform.localPosition.y;
    }

    void Update()
    {
        // 用Sin函数让物体在原始Y位置基础上浮动
        transform.localPosition = new Vector3(transform.localPosition.x,
                                              originalLocalY + Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude,
                                              transform.localPosition.z);
    }
}