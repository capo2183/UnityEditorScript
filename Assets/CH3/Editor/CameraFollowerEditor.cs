using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CameraFollow))]
public class CameraFollowerEditor : Editor
{
    CameraFollow m_Target;

    private bool _isTrackingXAxis;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Target = (CameraFollow)target;

        // Toggle(標題, 預設值)，勾選框元件
        _isTrackingXAxis = EditorGUILayout.Toggle("Tracking X Axis", m_Target.isTrackingXAxis);
        m_Target.isTrackingXAxis = _isTrackingXAxis;

        // 從 BeginDisabledGroup(Boolean) 到 EndDisabledGroup() 中間的範圍是否可以被選取
        // 取決於 BeginDisabledGroup 傳入的布林參數
        EditorGUI.BeginDisabledGroup(_isTrackingXAxis == false);

        // FloatField(標題, 預設值)，浮點數輸入元件
        // 原本的目標物件(Camera)裡的變數都要設定為 Inspector 欄位中修改的數值
        m_Target.xMargin = EditorGUILayout.FloatField("Margin", m_Target.xMargin);
        m_Target.xSmooth = EditorGUILayout.FloatField("Smooth", m_Target.xSmooth);
        m_Target.minMaxX.x = EditorGUILayout.FloatField("Min position", m_Target.minMaxX.x);
        m_Target.minMaxX.y = EditorGUILayout.FloatField("Max positio", m_Target.minMaxX.y);

        // 我們要用 Slider 來控制攝影機的位置，在此要先取得目標物件(Camera)的 Position
        Vector3 cameraPosition = m_Target.transform.position;
        // Slider(標題, 預設值, 最小值, 最大值)，滑桿元件
        cameraPosition.x = EditorGUILayout.Slider("Camera X Position", cameraPosition.x, m_Target.minMaxX.x, m_Target.minMaxX.y);
        EditorGUI.EndDisabledGroup();

        if (cameraPosition.x != m_Target.transform.position.x)
        {
            // 在修改目標物件(Camera)的位移前，先記錄到 Undo List 中。開發者可以藉由 Undo 的功能回到 Transform 尚未位移前的狀態
            Undo.RecordObject(m_Target.transform, "Change Camera X Position");
            // 原本的目標物件(Camera)裡的 position 都要設定為 Inspector 滑桿中修改的數值
            m_Target.transform.position = cameraPosition;            
        }

        // 每一次都重畫場景中的物件(為了處理 Gizmos)
        SceneView.RepaintAll();
    }
}