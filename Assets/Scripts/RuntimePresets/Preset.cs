using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace RuntimePresets
{
    public class Preset : MonoBehaviour, IPreset
    {
        [HideInInspector]
        [SerializeField]
        private Component _component;

        /// <summary>
        /// Creates a preset from a component instance at runtime.
        /// This action requires a temporary <see cref="GameObject"/> to be created.
        /// Call <see cref="Free"/> to remove it when you're done to remove it.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of <see cref="Component"/></typeparam>
        /// <param name="component"></param>
        /// <returns>A new Preset instance.</returns>
        public static Preset From<T>(T component) where T : Component
        {
            var componentContainer = new GameObject($"tmp_{Mathf.Abs(component.GetInstanceID())}");
            var preset = componentContainer.GetOrCreateComponent<Preset>();
            var componentCopy = componentContainer.GetOrCreateComponent<T>();
            
            componentCopy.TransferValuesFrom(component);
            preset.TargetComponent = componentCopy;
            componentContainer.SetActive(false);
            return preset;
        }

        /// <summary>
        /// The component <see cref="Type"/> the preset is based on.
        /// </summary>
        public Type ComponentType => _component.GetType();

        /// <summary>
        /// The instance storing the presets values.
        /// </summary>
        public Component TargetComponent
        {
            get => _component;
            set => _component = value;
        }

        /// <summary>
        /// Transfers the preset values to an actual component instance.
        /// </summary>
        /// <returns>True, if the values were successfully transferred.</returns>
        public bool ApplyTo(UnityObject target)
        {
            var component = target as Component;
            if (component == null || !CanBeAppliedTo(component)) return false;

            component.TransferValuesFrom(_component);
            return true;
        }

        /// <summary>
        /// Transfers the preset values to the given instances.
        /// </summary>
        /// <param name="targets">A collection of components.</param>
        /// <returns>True, if the values were successfully transferred to all targets.</returns>
        public bool ApplyTo(IEnumerable<UnityObject> targets)
        {
            return targets.All(t => ApplyTo(t));
        }

        /// <summary>
        /// Stores the values of the given <paramref name="component"/> in the preset.
        /// </summary>
        /// <returns>True, if the values were successfully updated.</returns>
        public bool UpdateFrom(Component component)
        {
            if (_component != null && CanBeAppliedTo(component))
            {
                _component.TransferValuesFrom(component);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// If this preset has been created durin runtime, its temporary instance will be destroyed.
        /// </summary>
        /// <returns>True, if an instance has been destroyed.</returns>
        public bool Free()
        {
            var instanceHasBeenDestroyed = false;

            if (_component.gameObject.scene.name != null)
            {
                Destroy(_component.gameObject);
                instanceHasBeenDestroyed = true;
            }

            TargetComponent = null;
            return instanceHasBeenDestroyed;
        }

        /// <summary>
        /// A preset stores values specific to a component. Only a component of the same type can be used to apply the preset values.
        /// </summary>
        /// <returns>True, if the values can be applied.</returns>
        public bool CanBeAppliedTo(UnityObject target) => (target != null) ? (target.GetType() == ComponentType) : false;
        
        public string GetTargetFullTypeName() => ComponentType?.FullName ?? string.Empty;

        public string GetTargetTypeName() => ComponentType?.Name ?? string.Empty; 
    }
}
