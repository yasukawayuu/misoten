using UnityEngine;
using UnityEditor;

public class EditorExpansion : EditorWindow
{
    string text = "";
    string mid = "";
    string result = "";

    [MenuItem("EditorExpansion/Open EditorExpansion")]
    private static void Open()
    {
        EditorExpansion window = GetWindow<EditorExpansion>();
    }

    private void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("テキスト");
            if (GUILayout.Button("入力"))
                mid = text;
            if(GUILayout.Button("出力"))
                result = mid;
        }
        text = EditorGUILayout.TextArea(text, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2));
        result = EditorGUILayout.TextArea(result, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2));
    }
}