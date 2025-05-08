using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManagerEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset visualTreeAsset = default;

    [MenuItem("Tools/PoolManager")]
    public static void ShowWindow()
    {
        PoolManagerEditor wnd = GetWindow<PoolManagerEditor>();
        wnd.titleContent = new GUIContent("PoolManagerEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        visualTreeAsset.CloneTree(root);
    }
}
