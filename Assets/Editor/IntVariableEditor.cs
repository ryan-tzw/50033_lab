using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IntVariable))]
public class IntVariableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var intVar = (IntVariable)target;
        
        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            EditorGUI.BeginChangeCheck();
            // check if the value changed in the inspector
            var newVal = EditorGUILayout.IntField("Runtime value", intVar.Value);
            
            // if changed then set the value for real
            if (EditorGUI.EndChangeCheck())
            {
                intVar.SetValue(newVal);
            }
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return Application.isPlaying;
    }
}