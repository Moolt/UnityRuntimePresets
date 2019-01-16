using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace RuntimePresets
{
    public class CreateRuntimePresetEditor
    {
        [MenuItem("CONTEXT/Component/Create Runtime Preset")]
        private static void CreateRuntimePreset(MenuCommand command)
        {
            var sourceComponent = (Component)command.context;
            var dummyObject = new GameObject();

            var targetComponent = dummyObject.GetOrCreateComponent(sourceComponent.GetType());
            var runtimePreset = dummyObject.GetOrCreateComponent<Preset>();
            runtimePreset.TargetComponent = targetComponent;

            UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponent);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(targetComponent);

            var prefab = PrefabUtility.CreatePrefab("Assets/New Preset.prefab", dummyObject);
            Object.DestroyImmediate(dummyObject);
        }
    }
}