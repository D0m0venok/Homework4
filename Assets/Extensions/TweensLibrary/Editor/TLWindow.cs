using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Extensions
{
    public class TLWindow : EditorWindow
    {
        private static TLResource _resource;
        private static Vector2 _scroll;

        [MenuItem("Window/Tweens Library")]
        private static void Init()
        {
            var window = GetWindow(typeof(TLWindow));
            window.titleContent = new GUIContent("Tweens Library");
            window.Show();
        }

        private void OnEnable()
        {
            _resource = Resources.Load<TLResource>("TL/TLResource");
            if (_resource != null) 
                return;

            _resource = CreateInstance<TLResource>();
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder("Assets/Resources/TL"))
                AssetDatabase.CreateFolder("Assets/Resources", "TL");
            AssetDatabase.CreateAsset(_resource, "Assets/Resources/TL/TLResource.asset");
        }
        private void OnGUI()
        {
            var serializedObject = new SerializedObject(_resource);
            serializedObject.Update();

            var foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.focused = foldoutStyle.active = foldoutStyle.hover = foldoutStyle.normal;
            foldoutStyle.onFocused = foldoutStyle.onActive = foldoutStyle.onHover = foldoutStyle.onNormal;

            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.margin = new RectOffset(buttonStyle.margin.left, buttonStyle.margin.right, 0, 0);

            EditorGUIUtility.wideMode = true;

            EditorGUILayout.Space();

            GUI.changed = false;

            _resource.TweensTimeScale = EditorGUILayout.Slider("Tweens TimeScale:", _resource.TweensTimeScale, 0.1f, 10f);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            var elements = _resource.GetElements();
            var i = -1;
            foreach (var element in elements)
            {
                i++;

                var propertyPath = "_elements.Array.data[" + i + "].";
                var foldout = serializedObject.FindProperty(propertyPath + "_foldout");
                var name = serializedObject.FindProperty(propertyPath + "_name");
                var isGeneric = serializedObject.FindProperty(propertyPath + "_isGeneric");
                var time = serializedObject.FindProperty(propertyPath + "_time");
                var minTime = serializedObject.FindProperty(propertyPath + "_minTime");
                var maxTime = serializedObject.FindProperty(propertyPath + "_maxTime");
                var useRandomTime = serializedObject.FindProperty(propertyPath + "_useRandomTime");

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(2);

                var color = GUI.color;
                if(elements.Count(e => e.Name == element.Name) > 1)
                    GUI.color = Color.yellow;

                var timeText = useRandomTime.boolValue ? "(" + minTime.floatValue.ToString(CultureInfo.InvariantCulture) + "~" + maxTime.floatValue.ToString(CultureInfo.InvariantCulture) + ")" 
                    : time.floatValue.ToString(CultureInfo.InvariantCulture);
                foldout.boolValue = EditorGUILayout.Foldout(foldout.boolValue, string.Format("{0} ({1}, {2}s, {3}{4})", 
                        element.Name, isGeneric.boolValue ? "Generic" : element.ActionType.ToString(), timeText, element.Ease, element.IsRelative ? ", relative" : string.Empty), 
                    true, foldoutStyle);

                GUI.color = color;

                if (!foldout.boolValue)
                {
                    GUILayout.EndVertical();
                    continue;
                }

                if (Application.isPlaying)
                    GUI.enabled = false;

                EditorGUILayout.BeginHorizontal();

                name.stringValue = EditorGUILayout.TextField("", name.stringValue, GUILayout.MinWidth(100));

                if(GUILayout.Button("U", EditorStyles.toolbarButton, GUILayout.Width(20)))
                    _resource.MoveElementUp(element);
                if(GUILayout.Button("D", EditorStyles.toolbarButton, GUILayout.Width(20)))
                    _resource.MoveElementDown(element);

                ColorBackground(new Color32(255, 100, 100, 255), () =>
                {
                    if(GUILayout.Button("X", buttonStyle, GUILayout.Width(20)))
                        if(EditorUtility.DisplayDialog("Tweens Library", "Do you want to delete " + name.stringValue + "?", "Yes", "No"))
                            _resource.RemoveElement(name.stringValue);
                
                });

                EditorGUILayout.EndHorizontal();

                GUI.enabled = true;

                serializedObject.DrawCommon(propertyPath);
            
                GUILayout.Space(2);

                GUILayout.EndVertical();
                GUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying)
                GUI.enabled = false;

            ColorBackground(new Color32(100, 255, 100, 255), () =>
            {
                if(GUILayout.Button("Add"))
                    _resource.AddElement(new TLElement());
            });

            if (GUI.changed)
                EditorUtility.SetDirty(_resource);

            var createNames = GUILayout.Button("Create TLNames.cs");
            GUILayout.Space(6);

            if(!createNames)
                return;

            var sb = new StringBuilder();
            sb.AppendLine("public static class TLNames");
            sb.AppendLine("{");
            foreach (var element in _resource.GetElements().Distinct())
            {
                sb.AppendLine("\tpublic const string " + element.Name + " = \"" + element.Name + "\";");
            }
            sb.AppendLine("}");

            File.WriteAllText(Application.dataPath + "/Resources/TL/TLNames.cs", sb.ToString());
            AssetDatabase.Refresh();
        }
        private void ColorBackground(Color color, Action action)
        {
            if(action == null)
                return;

            var back = GUI.backgroundColor;
            GUI.backgroundColor = color;
            action();
            GUI.backgroundColor = back;
        }
        private void ColorContent(Color color, Action action)
        {
            if(action == null)
                return;

            var content = GUI.contentColor;
            GUI.contentColor = color;
            action();
            GUI.contentColor = content;
        }
    }
}