# What are (runtime) presets?

Presets are a way to store a components values to an asset file, that can be reapplied to other components either in the [editor](https://docs.unity3d.com/Manual/Presets.html) or by [scripting](https://docs.unity3d.com/2018.2/Documentation/ScriptReference/Presets.Preset.html).

Presets have one downside though: They are placed in the `UnityEditor` namespace and can therefore not be used in your compiled game. Loading presets at runtime is unfourtunately not possible. This plug-in intends solve this problem.

Runtime presets an alternative to the Unity presets by storing components as prefabs that can then be reapplied at runtime. Using runtime presets is nearly identical to using the default Unity presets:

```csharp
public class ApplyPreset : MonoBehaviour
{
    public Preset _preset;
    public Light _light;

    private void Awake()
    {
        _preset.ApplyTo(_light);
    }
}
```
## Setup

If you want to get a quick overview you should check out the [demo project](https://github.com/Moolt/UnityRuntimePresets/archive/master.zip). 
You can also download the [package](https://github.com/Moolt/UnityRuntimePresets/raw/master/runtimePresets.unitypackage) containing only the essential scripts.

After downloading and importing the package, you will find a new entry `Create Runtime Preset` in the gear menu ![alt text](https://github.com/Moolt/UnityRuntimePresets/raw/master/Documentation/gear_icon.png "gear icon") of a component.

## Creating a runtime preset

Open the gear menu ![alt text](https://github.com/Moolt/UnityRuntimePresets/raw/master/Documentation/gear_icon.png "gear icon") on any component in a prefab or scene and click `Create Runtime Preset`.

![alt text](https://raw.githubusercontent.com/Moolt/UnityRuntimePresets/master/Documentation/creating_preset.png "creating runtime preset")

The plug-in will then create the preset in the `Assets` folder. A newly created preset will always have the default name `New Preset`. Rename it after creation to prevent it from being overwritten.

![alt text](https://raw.githubusercontent.com/Moolt/UnityRuntimePresets/master/Documentation/preset_prefab.png "preset file")

The preset is simply an prefab with a copy of your selected component. It also has an additional component called `Preset`. Keep this in mind as we'll need it in the next step.

![alt text](https://raw.githubusercontent.com/Moolt/UnityRuntimePresets/master/Documentation/preset_component.png "preset detail")

## Loading runtime presets

Presets can be set in the inspector once you expose a variable of the `Preset` type.

```csharp
public class ApplyPreset : MonoBehaviour
{
    //A preset which needs to be set in the inspector
    public Preset _preset;
    //A light component that we want to modify
    public Light _light;

    private void Awake()
    {
        //Presets store values of a specific component type
        //Checking whether the values can be applied may be helpful
        if(_preset.CanBeAppliedTo(_light))
        {
            //Transfers the values from the preset onto the _light component
            _preset.ApplyTo(_light);
        }
    }
}
```

After compiling the script you can now set the preset and reference the light component. The `preset` field is restricted to only accepting `presets`. You won't accidentally reference something else than a `preset`.

![alt text](https://raw.githubusercontent.com/Moolt/UnityRuntimePresets/master/Documentation/script_with_preset.png "script inspector")

## Creating presets during runtime

Presets can also be created during runtime. You will need any component to initialize the preset with. The present can then be used to apply the values to any component of the same type.

```csharp
public class CreatePreset : MonoBehaviour
{
    //A reference to a light from another gameObject
    public Light otherLight;
    //The preset we want to store the lights values in    
    private Preset _preset;

    void Start()
    {
        //Getting the light component of this gameObject
        var light = GetComponent<Light>();
        //Create a new preset from "otherLight" and apply its values to "light"
        _preset = Preset.From(otherLight);
        _preset.ApplyTo(light);
        //Removes the temporary object created to store the runtime preset
        //This is optional
        _preset.Free();        
    }
}
```

The examples might be a bit forced, but I often find myself in more complex situations where runtime prefabs can be a real life saver.
I hope I can save you some time with this plug-in. Feel free to contact me if you have any questions.