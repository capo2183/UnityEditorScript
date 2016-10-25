using UnityEngine;
using UnityEditor;
// UnityEditorInternal 雖然有開放給開發者，但他目前似乎是偏向 Unity 內部團隊在使用的。
// 或者是未來會釋出的功能目前還在測試階段中
// 官方文件中也沒有相關的資料，但裡面卻有好玩的東西可以去挖掘。例如下面會用到的 ReorderableList
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(LayerManager))]
public class LayerManagerEditor : Editor
{
    private LayerManager m_Target;
    private ReorderableList list;

    private int init_layer_count;

    private void OnEnable()
    {
        m_Target = (LayerManager)target;
        list = new ReorderableList(serializedObject,
                                   serializedObject.FindProperty("layerDataList"),
                                   true, true, false, false);
        DrawHeaderCallback();
        DrawElementCallback();
    }

    private void DrawHeaderCallback()
    {
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Layer Order Manager");
        };
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(20);
        DrawInitLayerCount();
        EditorGUILayout.BeginHorizontal();
        DrawDetectButton();
        DrawApplyButton();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawInitLayerCount()
    {
        init_layer_count = EditorGUILayout.IntField("Init Layer Count", init_layer_count);
    }

    private void DrawDetectButton()
    {
        if (GUILayout.Button("Detect", GUILayout.Height(20), GUILayout.Width(100)))
        {
            Undo.RecordObject(m_Target, "Detect Layer");
            m_Target.layerDataList.Clear();
            SpriteRenderer[] sr = m_Target.gameObject.GetComponents<SpriteRenderer>();
            for (int i = 0; i < sr.Length; i++)
            {
                SpriteRenderer target_sr = sr[i];
                m_Target.layerDataList.Add(new LayerData(target_sr.gameObject, target_sr.sortingLayerName, target_sr.sortingOrder));
            }
            SpriteRenderer[] src = m_Target.gameObject.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < src.Length; i++)
            {
                SpriteRenderer target_sr = src[i];
                m_Target.layerDataList.Add(new LayerData(target_sr.gameObject, target_sr.sortingLayerName, target_sr.sortingOrder));
            }
            EditorUtility.SetDirty(m_Target);
        }
    }

    private void DrawApplyButton()
    {
        if (GUILayout.Button("Apply", GUILayout.Height(20), GUILayout.Width(100)))
        {
            Undo.RecordObject(m_Target, "Apply Layer");
            for (int i = 0; i < m_Target.layerDataList.Count; i++)
            {
                LayerData _target_layer_data = m_Target.layerDataList[i];
                SpriteRenderer target_sr = _target_layer_data.go.GetComponent<SpriteRenderer>();

                _target_layer_data.LayerOrder = _target_layer_data.tempLayerOrderBeforeChange;
                target_sr.sortingLayerName = _target_layer_data.LayerName;
                target_sr.sortingOrder = _target_layer_data.LayerOrder;
            }
            EditorUtility.SetDirty(m_Target);
        }
    }

    private void DrawElementCallback()
    {
        // List 中的每一個元件要被畫時，會去 Call 以下的程式碼
        // [參數]
        //   rect : 原本每一個元件預設的位置和寬高
        //   index : 元件是 List 中的第幾個
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            // 將元件的序列化參數資料提出來。
            // 若用序列化參數來做修改，可以直接改變原本 List 中的參數值，不需要再將欄位的值存回去。
            // 另外一個好處是，它可以支援 Undo 的功能
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            // 每一個元件都間隔 2px
            rect.y += 2;

            // [1] 輸入 GameObject 欄位。該欄位是使用 PropertyField 的序列化參數來做修改
            //     不需要再將欄位的值存回去，而且可以支援 Undo 的功能
            float _width = rect.width / 10.0f;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, _width * 3, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("go"),
                GUIContent.none);

            // [2] 輸入圖層欄位。
            //     GetSortingLayerNames() 可以取得目前專案使用的所有Layer字串
            //     因為這裡並不是使用序列化參數來做修改，所以需要再將欄位的值存回 m_Target
            //     而且它不支援 Undo 的功能
            string[] sorting_layer_ary = GetSortingLayerNames();
            int target_layer_name_index = System.Array.IndexOf(sorting_layer_ary, m_Target.layerDataList[index].LayerName);
            int _layer_index = EditorGUI.Popup(
                new Rect(rect.x + _width * 3, rect.y, _width * 3, EditorGUIUtility.singleLineHeight),
                target_layer_name_index,
                sorting_layer_ary
                );
            m_Target.layerDataList[index].LayerName = GetSortingLayerNames()[_layer_index];
            EditorUtility.SetDirty(m_Target);

            // [3] 顯示目前圖層
            EditorGUI.LabelField(
                new Rect(rect.x + _width * 7, rect.y, _width * 1, EditorGUIUtility.singleLineHeight),
                m_Target.layerDataList[index].LayerOrder.ToString()
                );

            // [4] 和前一個 Index element 的圖層相隔多少
            EditorGUI.LabelField(
                new Rect(rect.x + _width * 8, rect.y, 15, EditorGUIUtility.singleLineHeight),
                "+"
                );
            if (index == 0)
                m_Target.layerDataList[index].AddSpaceLayer = 0;
            EditorGUI.PropertyField(
                new Rect(rect.x + _width * 8 + 15, rect.y, _width - 15, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("AddSpaceLayer"),
                GUIContent.none);

            // [5] 修正完後的圖層
            int _finalLayer = (index == 0) ? init_layer_count + m_Target.layerDataList[index].AddSpaceLayer : m_Target.layerDataList[index - 1].tempLayerOrderBeforeChange + m_Target.layerDataList[index].AddSpaceLayer;
            EditorGUI.LabelField(
                new Rect(rect.x + _width * 9 + 15, rect.y, _width * 1, EditorGUIUtility.singleLineHeight),
                _finalLayer.ToString()
                );
            m_Target.layerDataList[index].tempLayerOrderBeforeChange = _finalLayer;
        };
    }

    public string[] GetSortingLayerNames()
    {
        System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty(
                                                "sortingLayerNames",
                                                BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}
