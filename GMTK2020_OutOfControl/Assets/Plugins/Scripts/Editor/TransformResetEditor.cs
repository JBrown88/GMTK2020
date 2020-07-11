using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform), true)]
public class TransformResetEditor : Editor
{
    private SerializedProperty _positionProp;
    private SerializedProperty _rotationProp;
    private SerializedProperty _scaleProp;


    private void OnEnable()
    {
        _positionProp = serializedObject.FindProperty("m_LocalPosition");
        _rotationProp = serializedObject.FindProperty("m_LocalRotation");
        _scaleProp = serializedObject.FindProperty("m_LocalScale");
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 15f;

        serializedObject.Update();

        GUILayout.Space(2f);

        DrawPosition();

        GUILayout.Space(1f);

        DrawRotation();

        GUILayout.Space(1f);

        DrawScale();

        GUILayout.Space(2f);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawPosition()
    {
        GUILayout.BeginHorizontal();

        var reset = GUILayout.Button(new GUIContent(" P", "Reset position"), GUI.skin.FindStyle("ToolbarButton"),
            GUILayout.MaxWidth(18f));

        EditorGUILayout.PropertyField(_positionProp.FindPropertyRelative("x"));
        EditorGUILayout.PropertyField(_positionProp.FindPropertyRelative("y"));
        EditorGUILayout.PropertyField(_positionProp.FindPropertyRelative("z"));

        GUILayout.EndHorizontal();

        if (reset)
        {
            _positionProp.vector3Value = Vector3.zero;
        }
    }


    private void DrawScale()
    {
        GUILayout.BeginHorizontal();

        var reset = GUILayout.Button(new GUIContent(" S", "Reset scale"), GUI.skin.FindStyle("ToolbarButton"),
            GUILayout.MaxWidth(18f));

        EditorGUILayout.PropertyField(_scaleProp.FindPropertyRelative("x"));
        EditorGUILayout.PropertyField(_scaleProp.FindPropertyRelative("y"));
        EditorGUILayout.PropertyField(_scaleProp.FindPropertyRelative("z"));

        GUILayout.EndHorizontal();

        if (reset) _scaleProp.vector3Value = Vector3.one;
    }

    [Flags]
    private enum Axes
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4,
        All = 7,
    }

    private Axes CheckDifference(Transform t, Vector3 original)
    {
        var next = t.localEulerAngles;

        var axes = Axes.None;

        if (Differs(next.x, original.x)) axes |= Axes.X;
        if (Differs(next.y, original.y)) axes |= Axes.Y;
        if (Differs(next.z, original.z)) axes |= Axes.Z;

        return axes;
    }

    private Axes CheckDifference(SerializedProperty property)
    {
        var axes = Axes.None;

        if (!property.hasMultipleDifferentValues)
            return axes;

        var original = property.quaternionValue.eulerAngles;

        foreach (var obj in serializedObject.targetObjects)
        {
            axes |= CheckDifference(obj as Transform, original);
            if (axes == Axes.All) break;
        }

        return axes;
    }

    private static bool FloatField(string name, ref float value, bool hidden, GUILayoutOption opt)
    {
        var newValue = value;
        GUI.changed = false;

        if (!hidden)
        {
            newValue = EditorGUILayout.FloatField(name, newValue, opt);
        }
        else
        {
            float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
        }

        if (GUI.changed && Differs(newValue, value))
        {
            value = newValue;
            return true;
        }

        return false;
    }

    private static bool Differs(float a, float b)
    {
        return Mathf.Abs(a - b) > 0.0001f;
    }

    private static float WrapAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    private static void RegisterUndo(string name, params Object[] objects)
    {
        if (objects == null || objects.Length <= 0)
            return;

        Undo.RecordObjects(objects, name);

        foreach (var obj in objects)
        {
            if (obj == null) continue;
            EditorUtility.SetDirty(obj);
        }
    }

    private void DrawRotation()
    {
        GUILayout.BeginHorizontal();

        var reset = GUILayout.Button(new GUIContent(" R", "Reset rotation"), GUI.skin.FindStyle("ToolbarButton"),
            GUILayout.Width(18f));

        var visible = ((Transform) serializedObject.targetObject).localEulerAngles;

        visible.x = WrapAngle(visible.x);
        visible.y = WrapAngle(visible.y);
        visible.z = WrapAngle(visible.z);

        var changed = CheckDifference(_rotationProp);
        var altered = Axes.None;


        var opt = GUILayout.MinWidth(30f);

        if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, opt))
            altered |= Axes.X;
        if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, opt))
            altered |= Axes.Y;
        if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, opt))
            altered |= Axes.Z;

        if (reset)
        {
            _rotationProp.quaternionValue = Quaternion.identity;
        }
        else if (altered != Axes.None)
        {
            RegisterUndo("Change Rotation", serializedObject.targetObjects);

            foreach (var obj in serializedObject.targetObjects)
            {
                var t = obj as Transform;
                if (t == null)
                    continue;

                var v = t.localEulerAngles;

                if ((altered & Axes.X) != 0) v.x = visible.x;
                if ((altered & Axes.Y) != 0) v.y = visible.y;
                if ((altered & Axes.Z) != 0) v.z = visible.z;

                t.localEulerAngles = v;
            }
        }

        GUILayout.EndHorizontal();
    }
}