using UnityEngine;
using UnityEditor;

namespace DirePixel.Animation
{
    [CustomEditor(typeof(PaperDoll))]
    public class PaperDoll_Inspector : Editor
    {
        #region Fields & Properties

        private SerializedProperty _replacementTexture;
        private SerializedProperty _texturePath;
        private SerializedProperty _isChild;
        private SerializedProperty _syncRenderer;

        #endregion

        #region Monobehaviour Callbacks

        private void OnEnable()
        {
            _replacementTexture = serializedObject.FindProperty("ReplacementTexture");
            _texturePath = serializedObject.FindProperty("TexturePath");
            _isChild = serializedObject.FindProperty("IsChild");
            _syncRenderer = serializedObject.FindProperty("SyncRenderer");
        }

        #endregion

        #region GUI Management

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_replacementTexture, new GUIContent("Replacement Texture"));
            EditorGUILayout.PropertyField(_texturePath, new GUIContent("Texture Path"));
            EditorGUILayout.PropertyField(_isChild, new GUIContent("Is Child"));
            if(_isChild.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(_syncRenderer, new GUIContent("Sync Renderer"));

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}