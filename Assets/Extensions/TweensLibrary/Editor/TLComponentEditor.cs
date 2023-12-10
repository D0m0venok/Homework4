using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Extensions
{
    [CustomEditor(typeof(TLComponent))]
    [CanEditMultipleObjects]
    public class TLComponentEditor : UnityEditor.Editor
    {
        void OnEnable()
        {
            _tweenTarget = serializedObject.FindProperty("_tweenTarget");
            _triggerType = serializedObject.FindProperty("_triggerType");
            _customTrigger = serializedObject.FindProperty("_customTrigger");
            _destroyOnComplete = serializedObject.FindProperty("_destroyOnComplete");
            _fromLibrary = serializedObject.FindProperty("_fromLibrary");
            _name = serializedObject.FindProperty("_name");
        }

        public override void OnInspectorGUI()
        {
            //if (Application.isPlaying)
            //    GUI.enabled = false;

            serializedObject.Update();

            EditorGUIUtility.wideMode = true;

            EditorGUILayout.PropertyField(_tweenTarget);

            EditorGUILayout.PropertyField(_triggerType);

            if((TLComponentTriggerType)_triggerType.intValue == TLComponentTriggerType.Custom)
                EditorGUILayout.PropertyField(_customTrigger);

            EditorGUILayout.PropertyField(_destroyOnComplete);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_fromLibrary);

            if (_fromLibrary.boolValue)
            {
                var elements = TL.GetAll();
                var names = elements.Select(e => e.Name).ToList();
                var index = EditorGUILayout.Popup(new GUIContent("Name"), names.IndexOf(_name.stringValue), names.ToArray());
                _name.stringValue = index >= 0 ? elements[index].Name : elements.First().Name;
            }
            else
            {
                serializedObject.DrawCommon();
            }

            serializedObject.DrawComponentOnly();

            serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty _tweenTarget;
        private SerializedProperty _triggerType;
        private SerializedProperty _customTrigger;
        private SerializedProperty _destroyOnComplete;
        private SerializedProperty _fromLibrary;
        private SerializedProperty _name;
    }
}