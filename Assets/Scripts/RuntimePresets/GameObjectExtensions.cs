using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimePresets
{
    public static class GameObjectExtensions
    {
        public static Component GetOrCreateComponent(this GameObject target, Type type)
        {
            var component = target.GetComponent(type);

            if (component == null)
            {
                component = target.AddComponent(type);
            }

            return component;
        }

        public static T GetOrCreateComponent<T>(this GameObject target) where T : Component
        {
            return (T)target.GetOrCreateComponent(typeof(T));
        }
    }
}