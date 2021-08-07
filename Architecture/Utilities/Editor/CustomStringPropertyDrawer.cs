namespace Architecture.Utilities.Editor
{
  /*  [CustomPropertyDrawer(typeof(string))]
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class CustomStringPropertyDrawer : PropertyDrawer
    {
        private static int controlId = 0;
        private Rect buttonRect = new Rect(0, 0, 20, 20);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(SerializedPropertyType.ObjectReference, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var folderPathAttr = attribute as FolderPathAttribute;
            if (folderPathAttr != null && !folderPathAttr.assetObject)
            {
                folderPathAttr.assetObject = ScriptableObject.CreateInstance<FolderPathAttribute.AssetObject>();
                folderPathAttr.assetObject.defaultAsset =
                    AssetDatabase.LoadAssetAtPath<DefaultAsset>(property.stringValue);
                folderPathAttr.serializedObject = new SerializedObject(folderPathAttr.assetObject);
                folderPathAttr.serializedProperty = folderPathAttr.serializedObject.FindProperty("defaultAsset");
            }
            folderPathAttr.assetObject.defaultAsset = EditorGUI.ObjectField(position, label, folderPathAttr.assetObject.defaultAsset, typeof(DefaultAsset)) as DefaultAsset;
            property.stringValue = folderPathAttr.assetObject.defaultAsset
                ? Regex.Replace(AssetDatabase.GetAssetPath(folderPathAttr.assetObject.defaultAsset), ".+/Resources/", "") 
                : string.Empty;
        }
    }*/
}


