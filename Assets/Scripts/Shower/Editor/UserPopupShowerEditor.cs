using UnityEditor;
using UnityEngine;

namespace Homework4
{
    [CustomEditor(typeof(UserPopupShower))]
    public sealed class UserPopupShowerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var shower = (UserPopupShower) target;
                 
            if(GUILayout.Button("Show"))
                shower.Show();
            
            if (GUILayout.Button("ChangeName"))
                shower.ChangeName();
                 
            if (GUILayout.Button("ChangeDescription"))
                shower.ChangeDescription();
                 
            if (GUILayout.Button("ChangeIcon"))
                shower.ChangeIcon();
            
            if (GUILayout.Button("AddExperience"))
                shower.AddExperience();
                 
            if (GUILayout.Button("AddStat"))
                shower.AddStat();
                 
            if (GUILayout.Button("RemoveStat"))
                shower.RemoveStat();
            
            if (GUILayout.Button("ChangeRandomStat"))
                shower.ChangeRandomStat();
        }
    }
}