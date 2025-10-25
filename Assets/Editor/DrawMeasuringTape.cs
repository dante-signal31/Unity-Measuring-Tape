using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(MeasuringTape))]
// Leave this script under the default namespace. I don't know why, but Gizmos are not
// drawn if this script is under Editor namespace.
public class DrawMeasuringTape : UnityEditor.Editor
{
    private SerializedProperty _localPositionA;
    private SerializedProperty _localPositionB;
    private SerializedProperty _color;
    private SerializedProperty _thickness;
    private SerializedProperty _endWidth;
    private SerializedProperty _endAlignment;
    private SerializedProperty _textSize;   
    private SerializedProperty _textDistance;
    
    private void OnEnable()
    {
        _localPositionA = serializedObject.FindProperty("localPositionA");
        _localPositionB = serializedObject.FindProperty("localPositionB");
        _color = serializedObject.FindProperty("color");
        _thickness = serializedObject.FindProperty("thickness");
        _endWidth = serializedObject.FindProperty("endWidth");
        _endAlignment = serializedObject.FindProperty("endAlignment");
        _textSize = serializedObject.FindProperty("textSize");
        _textDistance = serializedObject.FindProperty("textDistance");
    }
    
    /// <summary>
    /// Create a custom inspector to show global positions of handles.
    /// </summary> 
    public override VisualElement CreateInspectorGUI()
    {
        
        VisualElement root = new()
        {
            style =
            {
                // Add elements in descending column order.
                flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column)
            }
        };

        MeasuringTape tape = (MeasuringTape) serializedObject.targetObject;
        
        // Just add those properties we want to be shown in its default view.
        var localPositionAField = new PropertyField(_localPositionA);
        root.Add(localPositionAField);
        var localPositionBField = new PropertyField(_localPositionB);
        root.Add(localPositionBField);
        
        // Create the panel where we will show global position fields.
        VisualElement globalPositionFields = new VisualElement();
        root.Add(globalPositionFields);
        
        // Populate the panel with global positions fields.
        UpdateGlobalPositionFields(globalPositionFields, tape);
        
        // When changed local position fields, or their handles, update global position
        // fields.
        localPositionAField.RegisterValueChangeCallback(_ => 
            UpdateGlobalPositionFields(globalPositionFields, tape));
        localPositionBField.RegisterValueChangeCallback(_ => 
            UpdateGlobalPositionFields(globalPositionFields, tape));
        
        // Add every other property we want to show in its default view.
        root.Add(new PropertyField(_color));
        root.Add(new PropertyField(_thickness));
        root.Add(new PropertyField(_endWidth));
        root.Add(new PropertyField(_endAlignment));
        root.Add(new PropertyField(_textSize));
        root.Add(new PropertyField(_textDistance));
        
        return root;
    }


    /// <summary>
    /// Updates the global position fields in a UI element to display the world
    /// coordinates of the tape's endpoints.
    /// </summary>
    /// <param name="panel">The UI panel where the global position fields will be
    /// updated.</param>
    /// <param name="tape">The MeasureTape component providing the global positions to
    /// display.</param>
    private void UpdateGlobalPositionFields(VisualElement panel, MeasuringTape tape)
    {
        panel.Clear();
        
        panel.style.flexDirection = 
            new StyleEnum<FlexDirection>(FlexDirection.Column);
        
        var positionAField = new Vector3Field("Global Position A");
        positionAField.value = tape.PositionA;
        positionAField.SetEnabled(false);
        panel.Add(positionAField);
        
        var positionBField = new Vector3Field("Global Position B");
        positionBField.value = tape.PositionB;
        positionBField.SetEnabled(false);
        panel.Add(positionBField);
    }

    // Use DrawGizmo to draw the tape even when the tape is not selected.
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawTapeGizmos(MeasuringTape tape, GizmoType gizmoType)
    {
        // Draw a line between handles.
        Handles.color = tape.color;
        Handles.DrawLine(tape.PositionA, tape.PositionB, tape.thickness);
        
        // Draw lines to highlight handles.
        Handles.DrawWireDisc(
            tape.PositionA,  
            Vector3.forward, 
            0.1f, 
            tape.thickness);
        Handles.DrawWireDisc(
            tape.PositionB, 
            Vector3.forward, 
            0.1f, 
            tape.thickness);
        
        float distance = Vector3.Distance(tape.PositionA, tape.PositionB);
        Vector3 direction = (tape.PositionB - tape.PositionA).normalized;
        // Normal vector is calculated to draw tape ends. It is perpendicular to the
        // direction vector, but it must be perpendicular to the camera's forward vector
        // to maximize the tape's ends visibility. So, to find a vector perpendicular to
        // both direction and camera axis, we use the cross-product.
        Vector3 normalVector = Vector3.Cross(
            direction, 
            SceneView.currentDrawingSceneView.camera.transform.forward).normalized;
        
        // Draw a label to show the distance between handles.
        Vector3 middlePosition = (tape.PositionA + tape.PositionB) / 2;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = tape.color;
        style.fontSize = tape.textSize;
        Vector3 labelPosition = middlePosition + normalVector * tape.textDistance;
        Handles.Label(labelPosition, distance.ToString("F2"), style);
        
        // Draw tape ends.
        Vector3 semiEnd = normalVector * tape.endWidth / 2;
        Handles.DrawLine(
            tape.PositionA - (semiEnd) + (semiEnd) * tape.endAlignment, 
            tape.PositionA + (semiEnd) + (semiEnd) * tape.endAlignment);
        Handles.DrawLine(
            tape.PositionB - (semiEnd) + (semiEnd) * tape.endAlignment,
            tape.PositionB + (semiEnd) + (semiEnd) * tape.endAlignment);
    }
    
    /// <summary>
    /// Show handles to locate ends of the tape.
    /// </summary>
    private void OnSceneGUI()
    {
        var tape = (MeasuringTape)target;
        
        EditorGUI.BeginChangeCheck();
        
        // Place handles to locate ends.
        Vector3 positionAHandle = Handles.PositionHandle(
            tape.PositionA, 
            Quaternion.identity);
        Vector3 positionBHandle = Handles.PositionHandle(
            tape.PositionB, 
            Quaternion.identity);
        
        if (EditorGUI.EndChangeCheck())
        {
            // Once handles have been moved. Update tape's endpoints.
            Undo.RecordObject(tape, $"Changed tape ends.");
            tape.PositionA = positionAHandle;
            tape.PositionB = positionBHandle;
        }
    }
}
