using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(BlockController))]
public class BlockControllerEditor : Editor {
    BlockController m_Target;

    public override void OnInspectorGUI()
    {
        m_Target = (BlockController)target;

        // DrawDefaultInspector() 會將原本 Inspector 上有的東西先畫出來。
        // 這麼一來可能會造成 Inspector 中的 Types 欄位被畫兩次(Default一次，下面 Editor Script 又會在畫一次)
        // 因此，你可以在原先 public List<BrickType> Types 的欄位前加入 [HideInInspector]，把 Default Inspector 關掉
        DrawDefaultInspector();
        DrawTypesInspector();
    }
	
    void DrawTypesInspector()
    {
        GUILayout.Space(5);
        GUILayout.Label("State", EditorStyles.boldLabel);

        for(int i=0; i< m_Target.Types.Count; i++)
        {
            DrawType(i);
        }

        DrawAddTypeButton();
    }

    void DrawType(int index)
    {
        if (index < 0 || index >= m_Target.Types.Count)
            return;
        
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Name", EditorStyles.label, GUILayout.Width(50));

            // BeginChangeCheck() 用來檢查在 BeginChangeCheck() 和 EndChangeCheck() 之間是否有 Inspector 變數改變
            EditorGUI.BeginChangeCheck();
            string newName = GUILayout.TextField(m_Target.Types[index].Name, GUILayout.Width(120));
            Color newColor = EditorGUILayout.ColorField(m_Target.Types[index].HitColor);
            
            m_Target.Types[index].Name = newName;
            m_Target.Types[index].HitColor = newColor;

            // 如果 Inspector 變數有改變，EndChangeCheck() 會回傳 True，才有必要去做變數存取
            if (EditorGUI.EndChangeCheck())
            {
                // 在修改之前建立 Undo/Redo 記錄步驟
                Undo.RecordObject(m_Target, "Modify Types");

                m_Target.Types[index].Name = newName;
                m_Target.Types[index].HitColor = newColor;

                // 每當直接修改 Inspector 變數，而不是使用 serializedObject 修改時，必須要告訴 Unity 這個 Compoent 已經修改過了
                // 在下一次存檔時，必須要儲存這個變數
                EditorUtility.SetDirty(m_Target);
            }

            if (GUILayout.Button("Remove"))
            {
                // 系統會 "登" 一聲
                EditorApplication.Beep();
                                
                // 顯示對話框功能(帶有 OK 和 Cancel 兩個按鈕)
                if (EditorUtility.DisplayDialog("Really?", "Do you really want to remove the state '" + m_Target.Types[index].Name + "'?", "Yes", "No") == true)
                {
                    m_Target.Types.RemoveAt(index);
                    EditorUtility.SetDirty(m_Target);
                }

            }
        }
        GUILayout.EndHorizontal();
    }

    void DrawAddTypeButton()
    {
        if (GUILayout.Button("Add new State", GUILayout.Height(30)))
        {
            Undo.RecordObject(m_Target, "Add new Type");

            m_Target.Types.Add(new BrickType { Name = "New State" });
            EditorUtility.SetDirty(m_Target);
        }
    }
}