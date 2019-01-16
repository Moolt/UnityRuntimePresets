using RuntimePresets;
using UnityEngine;

public class CreatePreset : MonoBehaviour
{
    public Light otherLight;
    private IPreset _preset;

    void Start()
    {
        var light = GetComponent<Light>();
        _preset = Preset.From(otherLight);
        _preset.ApplyTo(light);
        //Removes the temporary object created to store the runtime preset
        _preset.Free();        
    }
}
