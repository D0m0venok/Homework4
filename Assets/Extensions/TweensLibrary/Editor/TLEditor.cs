using UnityEditor;
using UnityEngine;

namespace Extensions
{
    public static class TLEditor
    {
        public static void DrawCommon(this SerializedObject serializedObject, string propertiesPath = null)
        {
            var isGeneric = serializedObject.FindProperty(propertiesPath + "_isGeneric");
            var actionType = serializedObject.FindProperty(propertiesPath + "_actionType");
            var tweenType = serializedObject.FindProperty(propertiesPath + "_tweenType");
            var fromFloat = serializedObject.FindProperty(propertiesPath + "_fromFloat");
            var fromVector = serializedObject.FindProperty(propertiesPath + "_fromVector");
            var fromColor = serializedObject.FindProperty(propertiesPath + "_fromColor");
            var toFloat = serializedObject.FindProperty(propertiesPath + "_toFloat");
            var toVector = serializedObject.FindProperty(propertiesPath + "_toVector");
            var toColor = serializedObject.FindProperty(propertiesPath + "_toColor");
            var time = serializedObject.FindProperty(propertiesPath + "_time");
            var useRandomTime = serializedObject.FindProperty(propertiesPath + "_useRandomTime");
            var minTime = serializedObject.FindProperty(propertiesPath + "_minTime");
            var maxTime = serializedObject.FindProperty(propertiesPath + "_maxTime");
            var ease = serializedObject.FindProperty(propertiesPath + "_ease");
            var isRelative = serializedObject.FindProperty(propertiesPath + "_isRelative");
            var resetOnComplete = serializedObject.FindProperty(propertiesPath + "_resetOnComplete");

            if (isGeneric != null)
                EditorGUILayout.PropertyField(isGeneric);

            if (isGeneric == null || !isGeneric.boolValue)
            {
                EditorGUILayout.PropertyField(actionType);
                EditorGUILayout.PropertyField(tweenType);

                var action = (TLElementActionType) actionType.intValue;
                var tween = (TLElementTweenType) tweenType.intValue;
                if (tween == TLElementTweenType.From || tween == TLElementTweenType.FromTo)
                {
                    SerializedProperty prop = null;
                    switch (action.GetValueType())
                    {
                        case TLElementValueType.Vector:
                            prop = fromVector;
                            break;
                        case TLElementValueType.Float:
                            prop = fromFloat;
                            break;
                        case TLElementValueType.Color:
                            prop = fromColor;
                            break;
                    }

                    EditorGUILayout.PropertyField(prop, new GUIContent("From"));
                }

                if (tween == TLElementTweenType.To || tween == TLElementTweenType.FromTo)
                {
                    SerializedProperty prop = null;
                    switch (action.GetValueType())
                    {
                        case TLElementValueType.Vector:
                            prop = toVector;
                            break;
                        case TLElementValueType.Float:
                            prop = toFloat;
                            break;
                        case TLElementValueType.Color:
                            prop = toColor;
                            break;
                    }

                    EditorGUILayout.PropertyField(prop, new GUIContent("To"));
                    if (action == TLElementActionType.Fade)
                    {
                        fromFloat.floatValue = Mathf.Clamp01(fromFloat.floatValue);
                        toFloat.floatValue = Mathf.Clamp01(toFloat.floatValue);
                    }
                }
            }

            EditorGUILayout.PropertyField(useRandomTime);
            if (useRandomTime.boolValue)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Time", GUILayout.Width(EditorGUIUtility.labelWidth - 5));
                GUILayout.Label("Min");
                minTime.floatValue = EditorGUILayout.FloatField(minTime.floatValue);
                GUILayout.Label("Max");
                maxTime.floatValue = EditorGUILayout.FloatField(maxTime.floatValue);

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.PropertyField(time);
            }

            EditorGUILayout.PropertyField(ease);

            if (isGeneric != null && isGeneric.boolValue)
                return;

            EditorGUILayout.PropertyField(isRelative);
            EditorGUILayout.PropertyField(resetOnComplete);
        }
        public static void DrawComponentOnly(this SerializedObject serializedObject, string propertiesPath = null)
        {
            var delay = serializedObject.FindProperty(propertiesPath + "_delay");
            var loops = serializedObject.FindProperty(propertiesPath + "_loops");
            var loopType = serializedObject.FindProperty(propertiesPath + "_loopType");
            var update = serializedObject.FindProperty(propertiesPath + "_isTimeScaleIndependent");
            var useEvents = serializedObject.FindProperty("_useEvents");
            var onStartEvent = serializedObject.FindProperty("_onStartEvent");
            var onCompleteEvent = serializedObject.FindProperty("_onCompleteEvent");

            EditorGUILayout.PropertyField(delay);
            if (delay.floatValue < 0)
                delay.floatValue = 0;

            EditorGUILayout.PropertyField(loops);
            if (loops.intValue < -1)
                loops.intValue = -1;

            EditorGUILayout.PropertyField(loopType);
            EditorGUILayout.PropertyField(update);

            EditorGUILayout.Space();
        
            EditorGUILayout.PropertyField(useEvents);
            if(!useEvents.boolValue)
                return;

            EditorGUILayout.PropertyField(onStartEvent);
            EditorGUILayout.PropertyField(onCompleteEvent);
        }
    }
}