using UnityEngine;

[System.Serializable]
public class Wave
{
    private const float FREQ_MIN = 0f;
    private const float FREQ_MAX = 0.3f;
    private const float AMP_MIN = -1f;
    private const float AMP_MAX = 1f;

    public float seed;

    [Range(FREQ_MIN, FREQ_MAX)]
    public float frequency;

    [Range(AMP_MIN, AMP_MAX)]
    public float amplitude;

}