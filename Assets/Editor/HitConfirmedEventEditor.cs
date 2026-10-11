using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitConfirmedEvent))]
public class HitConfirmedEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var hitEvent = (HitConfirmedEvent)target;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime debugging",  EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.IntField("Raise count", hitEvent.RaiseCount);

            if (hitEvent.RaiseCount > 0)
            {
                EditorGUILayout.ObjectField("Attacker time", hitEvent.LastEvent.AttackerTime, typeof(LocalTime), true);
                EditorGUILayout.ObjectField("Target time", hitEvent.LastEvent.TargetTime, typeof(LocalTime), true);
                EditorGUILayout.FloatField("Hitstop duration", hitEvent.LastEvent.Hit.HitstopDuration);
                EditorGUILayout.Vector3Field("Hit position", hitEvent.LastEvent.Position);
            }
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return Application.isPlaying;
    }
}