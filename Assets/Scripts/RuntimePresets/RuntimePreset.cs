using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace RuntimePresets
{
    public class RuntimePreset : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private Component _component;

        public Type ComponentType => _component.GetType();

        public Component TargetComponent
        {
            get => _component;
            set => _component = value;
        }

        public bool ApplyTo(UnityObject target)
        {
            var component = target as Component;
            if (component == null || !CanBeAppliedTo(component)) return false;

            component.TransferValuesFrom(_component);
            return true;
        }

        public bool ApplyTo(IEnumerable<UnityObject> targets)
        {
            return targets.All(t => ApplyTo(t));
        }

        public bool CanBeAppliedTo(UnityObject target) => target.GetType() == ComponentType;

        public string GetTargetFullTypeName() => ComponentType?.FullName ?? string.Empty;

        public string GetTargetTypeName() => ComponentType?.Name ?? string.Empty;
    }
}