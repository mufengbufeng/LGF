using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LGF.Util
{
    public static class FrameworkUtil
    {
        private static BindingFlags BindingFlags
        {
            get => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        }

        public static T1[] GetFields<T1>(object obj) where T1 : class
        {
            return FrameworkUtil.GetFields<T1>(obj, obj.GetType(), FrameworkUtil.BindingFlags);
        }

        public static T1[] GetFields<T1>(object obj, System.Type type, BindingFlags bindingFlags) where T1 : class
        {
            return FrameworkUtil.GetFields<T1>(obj, obj.GetType(), bindingFlags);
        }

        private static T1[] GetFields<T1>(object obj, System.Type type, BindingFlags bindingFlags, bool isSubclasssOf) where T1 : class
        {
            FieldInfo[] fields = type.GetFields(bindingFlags);
            List<T1> objList = (List<T1>)null;
            foreach (FieldInfo fieldInfo in fields)
            {
                if (isSubclasssOf)
                {
                    if (!fieldInfo.FieldType.IsSubclassOf(typeof(T1)))
                        continue;
                }
                else if (!typeof(T1).IsAssignableFrom(fieldInfo.FieldType))
                    continue;

                T1 obj1 = fieldInfo.GetValue(obj) as T1;
                objList ??= new List<T1>();
                objList.Add(obj1);
            }

            return objList?.ToArray();
        }

        public static void AutoSetUIField(object obj, GameObject viewObj, string prefix = "UI_")
        {
            FrameworkUtil.AutoSetUIField(obj, obj.GetType(), viewObj, FrameworkUtil.BindingFlags, prefix);
        }

        public static void AutoSetUIField(object obj, System.Type type, GameObject viewObj, string prefix = "UI_")
        {
            FrameworkUtil.AutoSetUIField(obj, type, viewObj, FrameworkUtil.BindingFlags, prefix);
        }

        public static void AutoSetUIField(object obj, System.Type type, GameObject viewObj, BindingFlags bindingFlags, string prefix)
        {
            foreach (FieldInfo field in type.GetFields(bindingFlags))
            {
                if (field.FieldType.IsSubclassOf(typeof(Component)) && field.Name.StartsWith(prefix))
                {
                    Component componentInChild = FrameworkUtil.FindComponentInChild(viewObj, field.FieldType, field.Name.Remove(0, prefix.Length));
                    if ((UnityEngine.Object)componentInChild != (UnityEngine.Object)null)
                        field.SetValue(obj, (object)componentInChild);
                }
            }
        }
        
        public static Component FindComponentInChild(GameObject obj, System.Type type, string name)
        {
            if ((UnityEngine.Object)obj == (UnityEngine.Object)null)
            {
                return (Component)null;
            }
            
            Component[] componentsInChildren = obj.GetComponentsInChildren(type, true);
            foreach (Component component in componentsInChildren)
            {
                if (component.gameObject.name == name)
                    return component;
            }

            return (Component)null;
        } 
        
    }
}