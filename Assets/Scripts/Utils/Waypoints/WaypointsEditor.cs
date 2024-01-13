using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoints))]
public class WaypointsEditor : Editor
{
    private Waypoints waypoints;

    private GUISkin style;

    private bool showPositionHandles;

    private bool showRotationHandles;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Toggle Position Handles"))
        {
            TogglePositionHandles();
        }

        if (GUILayout.Button("Toggle Rotation Handles"))
        {
            ToggleRotationHandles();
        }

        if (GUILayout.Button("Hide Handles"))
        {
            HideHandles();
        }
    }

    private void OnEnable()
    {
        style = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        Tools.hidden = showPositionHandles;
        waypoints = target as Waypoints;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }

    private void OnSceneGUI()
    {
        if (Selection.activeTransform == null)
        {
            return;
        }

        if (Event.current.type == EventType.ValidateCommand)
        {
            if (Event.current.commandName.Equals("UndoRedoPerformed"))
            {
                DrawWaypointPath();
            }
        }

        for (int i = 0; i < waypoints.GetLength(); i++)
        {
            // draw index labels for waypoints
            Vector3 positionOffset = Selection.activeTransform.position;
            Handles.Label(
                waypoints.GetWaypoint(i).position + positionOffset,
                string.Format("WP: {0}", i.ToString()),
                style.textField);

            if (showPositionHandles)
            {
                // add position handles with undo/redo buffer
                EditorGUI.BeginChangeCheck();
                Vector3 position = waypoints.GetWaypoint(i).position;
                Vector3 updatedPosition = Handles.PositionHandle(
                    position + positionOffset,
                    Quaternion.identity
                ) - positionOffset;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waypoints, "Edited waypoint position.");
                    waypoints.GetWaypoint(i).SetPosition(updatedPosition);
                }
            }

            if (showRotationHandles)
            {
                // add rotation handles undo/redo buffer
                EditorGUI.BeginChangeCheck();
                Quaternion updatedRotation = Handles.RotationHandle(
                    Quaternion.Euler(waypoints.GetWaypoint(i).rotation),
                    waypoints.GetWaypoint(i).position + positionOffset
                );
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waypoints, "Edited waypoint rotation.");
                    waypoints.GetWaypoint(i).SetRotation(updatedRotation.eulerAngles);
                }
            }
        }

        DrawWaypointPath();
    }

    private void TogglePositionHandles()
    {
        showRotationHandles = false;
        showPositionHandles = !showPositionHandles;
        Tools.hidden = showPositionHandles;
        SceneView.RepaintAll();
    }

    private void ToggleRotationHandles()
    {
        showPositionHandles = false;
        showRotationHandles = !showRotationHandles;
        Tools.hidden = showRotationHandles;
        SceneView.RepaintAll();
    }

    private void HideHandles()
    {
        showPositionHandles = false;
        showRotationHandles = false;
        Tools.hidden = false;
        SceneView.RepaintAll();
    }

    private void DrawWaypointPath()
    {
        if (waypoints.GetLength() < 2)
        {
            // if there are fewer than 2 waypoints there is no path to draw
            return;
        }

        Vector3 positionOffset = Selection.activeTransform.position;

        for (int i = 1; i < waypoints.GetLength(); i++)
        {
            // draw path hints
            Vector3 from = waypoints.GetWaypoint(i - 1).position + positionOffset;
            Vector3 to = waypoints.GetWaypoint(i).position + positionOffset;
            Handles.DrawAAPolyLine(3f, from, to);
        }

        if (waypoints.GetIsCircuit())
        {
            // if the path is a circuit connect the end of the path to the beginning
            Vector3 from = waypoints.GetWaypoint(waypoints.GetLength() - 1).position + positionOffset;
            Vector3 to = waypoints.GetWaypoint(0).position + positionOffset;
            Handles.DrawAAPolyLine(3f, new[] { Color.green, Color.green }, new Vector3[] { from, to });
        }

        foreach (Waypoint waypoint in waypoints.GetWaypoints())
        {
            // draw rotation hints
            Vector3 from = waypoint.position + positionOffset;

            Handles.DrawAAPolyLine(
                6f,
                new[] { Color.blue, Color.blue },
                new[] { from, from + Quaternion.Euler(waypoint.rotation) * Vector3.forward }
            );
            Handles.DrawAAPolyLine(
                6f,
                new[] { Color.red, Color.red },
                new[] { from, from + Quaternion.Euler(waypoint.rotation) * Vector3.right }
            );
            Handles.DrawAAPolyLine(
                6f,
                new[] { Color.green, Color.green },
                new[] { from, from + Quaternion.Euler(waypoint.rotation) * Vector3.up }
            );
        }
    }

}
