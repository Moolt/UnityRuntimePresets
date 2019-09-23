using RuntimePresets;
using UnityEngine;

public class TransferComponentValues : MonoBehaviour
{
    public MeshRenderer otherComponent;

    void Start()
    {
        var thisComponent = GetComponent<MeshRenderer>();
        thisComponent.TransferValuesFrom(otherComponent);
    }
}