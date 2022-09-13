using UnityEditor;
using UnityEngine;

namespace UI.Scripts
{
    
    public class VectorRangeAttribute : PropertyAttribute {
        
        public readonly float fMinX, fMaxX, fMinY, fMaxY;

        public VectorRangeAttribute(float fMinX, float fMaxX, float fMinY, float fMaxY)
        {
            this.fMinX = fMinX;
            this.fMaxX = fMaxX;
            this.fMinY = fMinY;
            this.fMaxY = fMaxY;
        }
        
    }

    [CustomPropertyDrawer(typeof(VectorRangeAttribute))]
    public class VectorRangeAttributeDrawer : PropertyDrawer
    {
        
        const int helpHeight = 30;
        const int textHeight = 16;
        
        VectorRangeAttribute rangeAttribute {  get { return (VectorRangeAttribute)attribute;  } }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Color previous = GUI.color;
            GUI.color = !IsValid(property) ? Color.red : Color.white;
            Rect textFieldPosition = position;
            textFieldPosition.width = position.width;
            textFieldPosition.height = position.height;
            EditorGUI.BeginChangeCheck();
            Vector2 val = EditorGUI.Vector2Field(textFieldPosition, label, property.vector2Value);
            if (EditorGUI.EndChangeCheck())
            {
                val.x = Mathf.Clamp(val.x, rangeAttribute.fMinX, rangeAttribute.fMaxX);
                val.y = Mathf.Clamp(val.y, rangeAttribute.fMinY, rangeAttribute.fMaxY);
                property.vector2Value = val;
            }
            Rect helpPosition = position;
            helpPosition.y += 16;
            helpPosition.height = 16;
            DrawHelpBox(helpPosition, property);
            GUI.color = previous;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!IsValid(property))
            {
                return 32;
            }
            return base.GetPropertyHeight(property, label);
        }
        
        void DrawHelpBox(Rect position, SerializedProperty prop)
        {
            if (IsValid(prop))
                return;

            EditorGUI.HelpBox(position, $"Invalid Range X [{rangeAttribute.fMinX}]-[{rangeAttribute.fMaxX}] Y [{rangeAttribute.fMinY}]-[{rangeAttribute.fMaxY}]", MessageType.Error);
        }
        
        bool IsValid(SerializedProperty prop)
        {
            Vector2 vector = prop.vector2Value;
            return vector.x >= rangeAttribute.fMinX && vector.x <= rangeAttribute.fMaxX && vector.y >= rangeAttribute.fMinY && vector.y <= rangeAttribute.fMaxY;
        } 
    }

}