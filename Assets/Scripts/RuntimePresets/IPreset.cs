using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace RuntimePresets
{
    public interface IPreset
    {
        Type ComponentType { get; }

        Component TargetComponent { get; set; }

        bool ApplyTo(UnityObject target);

        bool ApplyTo(IEnumerable<UnityObject> targets);

        bool UpdateFrom(Component component);
        
        bool Free();

        bool CanBeAppliedTo(UnityObject target);

        string GetTargetFullTypeName();

        string GetTargetTypeName();
    }
}