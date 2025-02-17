using System.Collections;
using UnityEngine;
using UnityEditor;

namespace DirePixel
{
    [CustomEditor(typeof(SpriteSettings))]
    public class SpriteSettings_Inspector : Editor
    {
        #region Fields & Properties

        private SerializedProperty _targetPath;
        private SerializedProperty _pixelsPerUnit;
        private SerializedProperty _isSpritesheet;
        private SerializedProperty _spriteSize;
        private SerializedProperty _spritePivotPoint;

        #endregion

        #region Monobehaviour Callbacks

        private void OnEnable()
        {
            _targetPath = serializedObject.FindProperty("TargetPath");
            _pixelsPerUnit = serializedObject.FindProperty("PixelsPerUnit");
            _isSpritesheet = serializedObject.FindProperty("IsSpritesheet");
            _spriteSize = serializedObject.FindProperty("SpriteSize");
            _spritePivotPoint = serializedObject.FindProperty("SpritePivotPoint");
        }

        #endregion

        #region GUI Management

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_targetPath, new GUIContent("Target Path"));
            EditorGUILayout.PropertyField(_pixelsPerUnit, new GUIContent("Pixels Per Unit"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_isSpritesheet, new GUIContent("Is Spritesheet"));
            if(_isSpritesheet.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(_spriteSize, new GUIContent("Sprite Size"));
                EditorGUILayout.PropertyField(_spritePivotPoint, new GUIContent("Sprite Pivot Point"));

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}