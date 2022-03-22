#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Extra.Either
{
    [CustomPropertyDrawer(typeof(Either<,>), true),
     CustomPropertyDrawer(typeof(Either<,,>), true),
     CustomPropertyDrawer(typeof(Either<,,,>), true)]
    public class EitherDrawer : PropertyDrawer
    {
        private static readonly float ButtonSideLength = EditorGUIUtility.singleLineHeight;
        private static string AsTypePropertyName(int index) => $"<AsT{index}>k__BackingField";

        private string[] _popupOptions;
        private GUIStyle _popupStyle;
        private float _propertyHeight;

        // Maps GetPropertyHeight to a variable so it can change depending on selected type.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => _propertyHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Retrieve property value as type Either using reflection.
            if (fieldInfo.GetValue(property.serializedObject.targetObject) is not Either propertyValue)
            {
                throw new Exception($"Property {property.name} is not of type {nameof(Either)}.");
            }

            // If not yet initialized, initialize type selection popup style and type options list.
            InitializePopupStyles(propertyValue, ref _popupStyle, ref _popupOptions);

            // Gather all properties representing the differently typed values of the object.
            var typeCount = propertyValue.TypeCount;
            var props = GatherSerializedProperties(property, typeCount);

            // Retrieve index property, which keeps track of selected type.
            var index = property.FindPropertyRelative("index");

            label = EditorGUI.BeginProperty(position, label, property);
            {
                EditorGUI.BeginChangeCheck();

                // Segment position Rect into left and right parts for button and property field respectively.
                var (buttonRect, propertyRect) = CalculateButtonAndPropertyRects(position, label);

                // Draw popup and use recorded index value to determine selected type.
                var newIndex = EditorGUI.Popup(buttonRect, index.intValue, _popupOptions, _popupStyle);
                index.intValue = newIndex;
                var propertyToDraw = props[newIndex];

                // Draw appropriate property and set height.
                if (propertyToDraw == null)
                {
                    EditorGUI.LabelField(propertyRect, "Unable to serialize this property.");
                    _propertyHeight = EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    GUIContent propertyLabel;

                    // Ensure that the button does not overlap with the property's foldout.
                    if (propertyToDraw.hasVisibleChildren)
                    {
                        EditorGUIUtility.labelWidth *= 0.5f;
                        propertyRect.xMin += ButtonSideLength * 0.5f;
                        propertyLabel = new GUIContent($"{_popupOptions[newIndex][3..]} Foldout");
                    }
                    else
                    {
                        propertyLabel = GUIContent.none;
                    }

                    EditorGUI.PropertyField(propertyRect, propertyToDraw, propertyLabel, true);
                    _propertyHeight = EditorGUI.GetPropertyHeight(propertyToDraw, label, true);

                    // Reset labelWidth to default value.
                    EditorGUIUtility.labelWidth = 0;
                }
            }
            EditorGUI.EndProperty();
        }

        private void InitializePopupStyles(Either propertyValue, ref GUIStyle popupStyle, ref string[] popupOptions)
        {
            // Retrieve "three-dots" menu style.
            popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
            {
                imagePosition = ImagePosition.ImageOnly
            };

            // Transform type Either's "TypeNames" property by prepending each entry with "As ".
            popupOptions ??= propertyValue.TypeNames.Select(i => $"As {i}").ToArray();
        }

        private static SerializedProperty[] GatherSerializedProperties(SerializedProperty property, int typeCount)
        {
            var res = new SerializedProperty[typeCount];
            for (var i = 0; i < typeCount; i++)
            {
                res[i] = property.FindPropertyRelative(AsTypePropertyName(i));
            }

            return res;
        }

        private static (Rect, Rect) CalculateButtonAndPropertyRects(Rect position, GUIContent label)
        {
            // Determine rect where field will be drawn.
            var positionNoLabel = EditorGUI.PrefixLabel(position, label);

            var buttonRect = positionNoLabel;
            buttonRect.xMax = buttonRect.xMin + ButtonSideLength;
            buttonRect.yMax = buttonRect.yMin + ButtonSideLength;

            var propertyRect = positionNoLabel;
            propertyRect.xMin += ButtonSideLength;

            return (buttonRect, propertyRect);
        }
    }
}

#endif