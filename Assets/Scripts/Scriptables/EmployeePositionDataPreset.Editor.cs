using UnityEditor;
using UnityEngine;

public partial class EmployeePositionDataPreset 
{
    [CustomEditor(typeof(EmployeePositionDataPreset))]
    public class EmployeePositionDataEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var script = target as IEmployeePositionData;

            //Allow custom editor to register undo
            Undo.RecordObject(target, "Changed Employee Position Data");

            DrawSeniorityInfo(script.juniorsInfo, "Juniors");
            DrawSeniorityInfo(script.semiSeniorsInfo, "Semi Seniors");
            DrawSeniorityInfo(script.seniorsInfo, "Seniors");

            //Save custom editor fields
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }

        void DrawSeniorityInfo(SeniorityInfo info, string tittle)
        {
            EditorGUILayout.LabelField(tittle);
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Base Salary", new GUILayoutOption[] { GUILayout.Width(200) });
            info.baseSalary = EditorGUILayout.FloatField(info.baseSalary, new GUILayoutOption[] { GUILayout.MaxWidth(200) });
            if (info.baseSalary < 0) info.baseSalary = 0;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Salary Increment %", new GUILayoutOption[] { GUILayout.Width(200) });
            info.salaryIncrementPct = EditorGUILayout.Slider(info.salaryIncrementPct, 0, 100, new GUILayoutOption[] { GUILayout.MaxWidth(200) });
            if (info.salaryIncrementPct < 0) info.salaryIncrementPct = 0;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Salary after increment: " + info.baseSalary * (1 + (info.salaryIncrementPct / 100f)));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(25);
        }
    }
}
