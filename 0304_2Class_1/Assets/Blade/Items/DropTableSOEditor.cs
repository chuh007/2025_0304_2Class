using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace Blade.Items
{
    [CustomEditor(typeof(DropTableSO))]
    public class DropTableSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset = default;
        private string _rootFolderPath;
        
        public override VisualElement CreateInspectorGUI()
        {
            InitializeWindow();
            VisualElement root = new VisualElement();
            visualTreeAsset.CloneTree(root);
            
            IntegerField minField = root.Q<IntegerField>("Min");
            IntegerField maxField = root.Q<IntegerField>("Max");
            minField.RegisterValueChangedCallback(HandleMinValueChange);
            maxField.RegisterValueChangedCallback(HandleMaxValueChange);

            return root;
        }

        private void InitializeWindow()
        {
            MonoScript monoScript = MonoScript.FromScriptableObject(this);
            string scriptPath = AssetDatabase.GetAssetPath(monoScript);

            _rootFolderPath = Directory.GetParent(Path.GetDirectoryName(scriptPath)).FullName.Replace("\\", "/");
            _rootFolderPath = "Assets" + _rootFolderPath.Substring(Application.dataPath.Length);
            
            visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{_rootFolderPath}/Items/DropTableUI.uxml");
            Debug.Assert(visualTreeAsset != null, "Visual tree asset is null");
        }

        
        private void HandleMaxValueChange(ChangeEvent<int> evt)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            string message = AssetDatabase.RenameAsset(assetPath, evt.newValue.ToString());
            if (string.IsNullOrEmpty(message)) //메시지가 없다는건 성공적으로 교체되었다는 뜻
            {
                (evt.target as IntegerField).SetValueWithoutNotify(evt.previousValue);
            }
        }

        private void HandleMinValueChange(ChangeEvent<int> evt)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            string message = AssetDatabase.RenameAsset(assetPath, evt.newValue.ToString());
            if (string.IsNullOrEmpty(message)) //메시지가 없다는건 성공적으로 교체되었다는 뜻
            {
                (evt.target as IntegerField).SetValueWithoutNotify(evt.previousValue);
            }
        }

        private void HandleAssetNameChange(ChangeEvent<string> evt)
        {
            if (string.IsNullOrEmpty(evt.newValue))
            {
                EditorUtility.DisplayDialog("Error", "Name cannot be empty", "OK");
                return;
            }

            //현재 SO의 경로를 알아낸다.
            string assetPath = AssetDatabase.GetAssetPath(target);
            string newName = $"{evt.newValue}"; //신규 파일이름

            string message = AssetDatabase.RenameAsset(assetPath, newName); //새로운 이름으로 다시 저장
            if (string.IsNullOrEmpty(message)) //메시지가 없다는건 성공적으로 교체되었다는 뜻
            {
                target.name = newName;
            }
            else
            {
                (evt.target as TextField).SetValueWithoutNotify(evt.previousValue);
                EditorUtility.DisplayDialog("Error", message, "OK");
            }
        }
    }
}