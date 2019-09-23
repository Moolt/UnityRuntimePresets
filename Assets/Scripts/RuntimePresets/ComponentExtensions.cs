using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RuntimePresets
{
    public static class ComponentExtensions
    {
        private static Dictionary<string, string> exceptions = new Dictionary<string, string>
        {
            {"material", "sharedMaterial"},
            {"materials", "sharedMaterials"},
            {"mesh", "sharedMesh"}
        };

        public static T TransferValuesFrom<T>(this Component comp, T other, bool considerBaseClasses = true) where T : Component
        {
            var conditionalFlags = considerBaseClasses ? BindingFlags.FlattenHierarchy : BindingFlags.DeclaredOnly;
            
            // Type of the copy
            Type type = comp.GetType();
            
            // Type mismatch
            if (type != other.GetType())
            {
                return null;
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default | conditionalFlags;
            PropertyInfo[] propertyInfos = type.GetProperties(flags);

            //Handle variables
            foreach (var pinfo in propertyInfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        // Ignore obsolete variables to avoid editor warnings
                        if (HasAnnotation<ObsoleteAttribute>(pinfo) || HasAnnotation<NotSupportedException>(pinfo) || HasAnnotation<System.ComponentModel.EditorBrowsableAttribute>(pinfo))
                        {
                            continue;
                        }
                        if (exceptions.ContainsKey(pinfo.Name))
                        {
                            PropertyInfo exInfo = type.GetProperty(exceptions[pinfo.Name]);
                            exInfo.SetValue(comp, exInfo.GetValue(other, null), null);
                        }
                        else
                        {
                            pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                        }
                    }
                    catch (System.Exception e) { Debug.Log(e.Message); }
                }
            }

            // Handle properties
            FieldInfo[] finfos = type.GetFields(flags);

            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }

            return comp as T;
        }

        private static bool HasAnnotation<T>(PropertyInfo pinfo)
        {
            object[] attr = pinfo.GetCustomAttributes(typeof(T), true);

            return attr.Length > 0;
        }
    }
}