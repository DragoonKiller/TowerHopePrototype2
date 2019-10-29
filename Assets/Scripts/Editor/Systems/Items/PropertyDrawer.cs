using UnityEngine;
using UnityEditor;

namespace Systems
{
    using Utils;
    
    [CustomPropertyDrawer(typeof(ItemDispatcher.Inventory))]
    public class ItemDispatcherInventoryPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            if(property.isExpanded)
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("inventory"));
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("rule"));
                var cls = property.FindPropertyRelative("classifiers");
                height += EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);
                for(int i = 0; i < cls.arraySize; i++) height += EditorGUI.GetPropertyHeight(cls.GetArrayElementAtIndex(i));
            }
            return height;
        }
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            
            var curHeight = 0f;
            
            {
                var height = base.GetPropertyHeight(property, label);
                property.isExpanded = EditorGUI.Foldout(new Rect(rect.position, new Vector2(rect.width, height)), property.isExpanded, label);
                curHeight += height;
            }
            
            if(property.isExpanded)
            {
                {
                    var ivt = property.FindPropertyRelative("inventory");
                    var height = EditorGUI.GetPropertyHeight(ivt);
                    var pos = new Rect(rect.position + new Vector2(0, curHeight), new Vector2(rect.width, height));
                    EditorGUI.ObjectField(pos, ivt);
                    curHeight += height;
                }
                
                {
                    var rule = property.FindPropertyRelative("rule");
                    var height = EditorGUI.GetPropertyHeight(rule);
                    var pos = new Rect(rect.position + new Vector2(0, curHeight), new Vector2(rect.width, height));
                    rule.enumValueIndex = (int)(ItemDispatcher.DispatchRule)EditorGUI.EnumPopup(pos, "rule", (ItemDispatcher.DispatchRule)rule.enumValueIndex);
                    curHeight += height;
                }
                
                var classifiers = property.FindPropertyRelative("classifiers");
                
                {
                    var height = EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);
                    var pos = new Rect(rect.position + new Vector2(0, curHeight), new Vector2(rect.width, height));
                    classifiers.arraySize = EditorGUI.IntField(pos, "Classifiers", classifiers.arraySize);
                    curHeight += height;
                }
                
                EditorGUI.indentLevel += 1;
                for(int i = 0; i < classifiers.arraySize; i++)
                {
                    var cls = classifiers.GetArrayElementAtIndex(i);
                    var height = EditorGUI.GetPropertyHeight(cls);
                    var pos = new Rect(rect.position + new Vector2(0, curHeight), new Vector2(rect.width, height));
                    cls.stringValue = EditorGUI.TextField(pos, $"|-- {i}", cls.stringValue);
                    curHeight += height;
                }
                EditorGUI.indentLevel -= 1;
            }
            EditorGUI.EndProperty();
        }
    }
    
}
