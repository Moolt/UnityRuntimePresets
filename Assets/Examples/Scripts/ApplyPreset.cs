using RuntimePresets;
using UnityEngine;

public class ApplyPreset : MonoBehaviour
{
    public Preset _preset;
    public Light _light;

    private void Awake()
    {
        _preset.ApplyTo(_light);
    }
}
