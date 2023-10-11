using UnityEngine;

public class ContinuousCameraShake : MonoBehaviour
{
    public float shakeStrength = 0.1f;     // 震动强度
    public float shakeFrequency = 1.0f;    // 噪声采样频率

    private Vector3 originalPos;
    private float timeCounter = 0.0f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.PerlinNoise(timeCounter * shakeFrequency, 0) * 2 - 1; // 产生-1到1之间的值
        float y = Mathf.PerlinNoise(0, timeCounter * shakeFrequency) * 2 - 1; // 产生-1到1之间的值
        transform.localPosition = originalPos + new Vector3(x, y, 0) * shakeStrength;
        timeCounter += Time.deltaTime;
    }
}
