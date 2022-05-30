using System;
using UnityEditor;
using UnityEngine;

namespace Extra.Either
{
    [Serializable]
    public struct None
    {
        public static None Instance = default;
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
    public class Optional<T> : Either<T, None>
    {
        public Optional(int index = 0, T asT = default, None asNone = default) : base(index, asT, asNone) { }

        public static implicit operator T(Optional<T> optional) => optional.AsT0;
        public static implicit operator None(Optional<T> optional) => optional.AsT1;

        public static implicit operator Optional<T>(T value) => new(0, asT: value);
        public static implicit operator Optional<T>(None value) => new(1);
    }
}