using UnityEngine;
using UnityEditor;
using TMPro;

public class GlobalFontSwapper : EditorWindow
{
    public TMP_FontAsset newFont;

    [MenuItem("Tools/Replace All Fonts")]
    public static void ShowWindow() => GetWindow<GlobalFontSwapper>("Font Swapper");

    void OnGUI()
    {
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Replace All Fonts in Scene"))
        {
            var textObjects = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
            foreach (var text in textObjects)
            {
                Undo.RecordObject(text, "Replace Font");
                text.font = newFont;
                EditorUtility.SetDirty(text);
            }
            Debug.Log($"Replaced fonts on {textObjects.Length} objects.");
        }
    }
}