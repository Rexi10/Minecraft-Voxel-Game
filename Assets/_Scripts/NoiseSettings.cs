using UnityEngine;

[CreateAssetMenu(fileName = "NoiseSettings", menuName = "Noise/NoiseSettings")]
public class NoiseSettings : ScriptableObject
{
    public float noiseZoom;
    public int octaves;
    public Vector2Int offset;
    public Vector2Int worldOffset;
    public float persistence;
    public float redistributionModifier;
    public float exponent;
}
