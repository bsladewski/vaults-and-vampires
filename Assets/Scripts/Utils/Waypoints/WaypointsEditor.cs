#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;

namespace Utils
{
    [CustomEditor(typeof(Waypoints))]
    public class WaypointsEditor : Editor
    {
        private Waypoints waypoints;

        private GUISkin style;

        private bool showPositionHandles;

        private bool positionRelativeRotation;

        private bool showRotationHandles;

        private bool showColliderHints = true;

        private readonly Color buttonEnabledColor = Color.green;

        private readonly Color buttonDisabledColor = new Color(1f, 0.25f, 0.25f, 1f);

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawPositionControlGUI();
            GUILayout.Space(2);
            DrawRotationControlGUI();
            GUILayout.Space(2);
            DrawOtherControlGUI();
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

            Vector3 positionOffset = Selection.activeTransform.position;
            Collider collider = Selection.activeGameObject.GetComponent<Collider>();

            for (int i = 0; i < waypoints.GetLength(); i++)
            {
                // draw index labels for waypoints
                Waypoint waypoint = waypoints.GetWaypoint(i);
                DrawWaypointLabel(waypoint, positionOffset, string.Format("WP: {0}", i.ToString()));

                if (showPositionHandles)
                {
                    DrawWaypointPositionHandle(waypoint, positionOffset);
                }

                if (showRotationHandles)
                {
                    DrawWaypointRotationHandle(waypoint, positionOffset);
                }

                if (showColliderHints)
                {
                    DrawColliderHint(waypoint, collider);
                }
            }

            DrawWaypointPath();
        }

        private void DrawPositionControlGUI()
        {
            GUILayout.BeginVertical("Position Controls", "window");

            Color initialColor = GUI.color;
            GUI.color = showPositionHandles ? buttonEnabledColor : buttonDisabledColor;
            if (GUILayout.Button("Toggle Position Handles"))
            {
                showRotationHandles = false;
                showPositionHandles = !showPositionHandles;
                Tools.hidden = showPositionHandles;
                SceneView.RepaintAll();
            }
            GUI.color = initialColor;

            bool value = GUILayout.Toggle(positionRelativeRotation, "Relative to Rotation");
            if (value != positionRelativeRotation)
            {
                positionRelativeRotation = value;
                SceneView.RepaintAll();
            }

            GUILayout.EndVertical();
        }

        private void DrawRotationControlGUI()
        {
            GUILayout.BeginVertical("Rotation Controls", "window");

            Color initialColor = GUI.color;
            GUI.color = showRotationHandles ? buttonEnabledColor : buttonDisabledColor;
            if (GUILayout.Button("Toggle Rotation Handles"))
            {
                showPositionHandles = false;
                showRotationHandles = !showRotationHandles;
                Tools.hidden = showRotationHandles;
                SceneView.RepaintAll();
            }
            GUI.color = initialColor;

            GUILayout.EndVertical();
        }

        private void DrawOtherControlGUI()
        {
            GUILayout.BeginVertical("Other", "window");

            Color initialColor = GUI.color;
            GUI.color = showColliderHints ? buttonEnabledColor : buttonDisabledColor;
            if (GUILayout.Button("Toggle Collider Hints"))
            {
                showColliderHints = !showColliderHints;
                SceneView.RepaintAll();
            }
            GUI.color = initialColor;

            GUI.enabled = showPositionHandles || showRotationHandles;
            if (GUILayout.Button("Hide Handles"))
            {
                showPositionHandles = false;
                showRotationHandles = false;
                Tools.hidden = false;
                SceneView.RepaintAll();
            }
            GUI.enabled = true;

            GUILayout.EndVertical();
        }

        private void DrawWaypointLabel(Waypoint waypoint, Vector3 positionOffset, string label)
        {
            Handles.Label(waypoint.position + positionOffset, label, style.textField);
        }

        private void DrawWaypointPositionHandle(Waypoint waypoint, Vector3 positionOffset)
        {
            // add position handles with undo/redo buffer
            EditorGUI.BeginChangeCheck();
            Vector3 position = waypoint.position;
            Vector3 updatedPosition = Handles.PositionHandle(
                position + positionOffset,
                positionRelativeRotation ? Quaternion.Euler(waypoint.rotation) : Quaternion.identity
            ) - positionOffset;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(waypoints, "Edited waypoint position.");
                waypoint.SetPosition(updatedPosition);
            }
        }

        private void DrawWaypointRotationHandle(Waypoint waypoint, Vector3 positionOffset)
        {
            // add rotation handles undo/redo buffer
            EditorGUI.BeginChangeCheck();
            Quaternion updatedRotation = Handles.RotationHandle(
                Quaternion.Euler(waypoint.rotation),
                waypoint.position + positionOffset
            );
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(waypoints, "Edited waypoint rotation.");
                waypoint.SetRotation(updatedRotation.eulerAngles);
            }
        }

        private void DrawColliderHint(Waypoint waypoint, Collider collider)
        {
            if (collider == null)
            {
                return;
            }

            Bounds bounds = collider.bounds;
            Vector3 center = bounds.center;
            Quaternion rotation = Quaternion.Euler(waypoint.rotation);

            // collider top
            Vector3 upBackLeft = rotation * (new Vector3(bounds.min.x, bounds.max.y, bounds.min.z) - center) +
                center + waypoint.position;
            Vector3 upBackRight = rotation * (new Vector3(bounds.max.x, bounds.max.y, bounds.min.z) - center) +
                center + waypoint.position;
            Vector3 upFrontLeft = rotation * (new Vector3(bounds.min.x, bounds.max.y, bounds.max.z) - center) +
                center + waypoint.position;
            Vector3 upFrontRight = rotation * (new Vector3(bounds.max.x, bounds.max.y, bounds.max.z) - center) +
                center + waypoint.position;

            // collider bottom
            Vector3 downBackLeft = rotation * (new Vector3(bounds.min.x, bounds.min.y, bounds.min.z) - center) +
                center + waypoint.position;
            Vector3 downBackRight = rotation * (new Vector3(bounds.max.x, bounds.min.y, bounds.min.z) - center) +
                center + waypoint.position;
            Vector3 downFrontLeft = rotation * (new Vector3(bounds.min.x, bounds.min.y, bounds.max.z) - center) +
                center + waypoint.position;
            Vector3 downFrontRight = rotation * (new Vector3(bounds.max.x, bounds.min.y, bounds.max.z) - center) +
                center + waypoint.position;

            float screenSpaceSize = 5f;

            // draw top
            Handles.DrawDottedLine(upBackLeft, upBackRight, screenSpaceSize);
            Handles.DrawDottedLine(upBackRight, upFrontRight, screenSpaceSize);
            Handles.DrawDottedLine(upFrontRight, upFrontLeft, screenSpaceSize);
            Handles.DrawDottedLine(upFrontLeft, upBackLeft, screenSpaceSize);

            // draw bottom
            Handles.DrawDottedLine(downBackLeft, downBackRight, screenSpaceSize);
            Handles.DrawDottedLine(downBackRight, downFrontRight, screenSpaceSize);
            Handles.DrawDottedLine(downFrontRight, downFrontLeft, screenSpaceSize);
            Handles.DrawDottedLine(downFrontLeft, downBackLeft, screenSpaceSize);

            // draw middle
            Handles.DrawDottedLine(upBackLeft, downBackLeft, screenSpaceSize);
            Handles.DrawDottedLine(upBackRight, downBackRight, screenSpaceSize);
            Handles.DrawDottedLine(upFrontLeft, downFrontLeft, screenSpaceSize);
            Handles.DrawDottedLine(upFrontRight, downFrontRight, screenSpaceSize);
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
                Handles.DrawAAPolyLine(3f, new[] { Color.green, Color.green }, new[] { from, to });
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
}
#endif
