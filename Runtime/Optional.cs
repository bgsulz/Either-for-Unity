using System;
using UnityEditor;
using UnityEngine;

namespace Extra.Either
{
    [Serializable]
    public struct None
    {
        public static None Instance = new();
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(None))]
    public class NoneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => EditorGUI.LabelField(position, "None");
    }
#endif

    [Serializable]
    public class Optional<T> : Either<T, None> { }
}