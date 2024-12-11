using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomLabelAttribute))]
public class CustomLabelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomLabelAttribute customLabel = (CustomLabelAttribute)attribute;

        // 配列やリストの要素かどうかチェック
        if (property.isArray && property.propertyType != SerializedPropertyType.String)
        {
            // 配列やリストの各要素にカスタムラベルを表示
            EditorGUI.PropertyField(position, property, new GUIContent(customLabel.Value), true);
        }
        else
        {
            // 通常のプロパティにカスタムラベルを表示
            EditorGUI.PropertyField(position, property, new GUIContent(customLabel.Value), true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
