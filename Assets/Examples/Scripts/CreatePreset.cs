using RuntimePresets;
using UnityEngine;

public class CreatePreset<T> : MonoBehaviour
    where T : Component
{
    public T otherComponent;
    public bool removeTemporaryPresetObject = true;
    private IPreset _preset;

    void Start()
    {
        var thisComponent = GetComponent<T>();
        _preset = Preset.From(otherComponent);
        _preset.ApplyTo(thisComponent);
        //Removes the temporary object created to store the runtime preset
        if(removeTemporaryPresetObject)
        {
            _preset.Free();
        }
    }
}