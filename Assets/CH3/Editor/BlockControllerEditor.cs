using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BlockController))]
public class BlockControllerEditor : Editor {
    BlockController m_Target;

    public override void OnInspectorGUI()
    {
        m_Target = (BlockController)target;

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

        // DrawAddTypeButton();
    }

    void DrawType(int index)
    {
        if (index < 0 || index >= m_Target.Types.Count)
            return;

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Name", EditorStyles.label, GUILayout.Width(50));
            string newName = GUILayout.TextField(m_Target.Types[index].Name, GUILayout.Width(120));
            Color newColor = EditorGUILayout.ColorField(m_Target.Types[index].HitColor);

            m_Target.Types[index].Name = newName;
            m_Target.Types[index].HitColor = newColor;

            if (GUILayout.Button("Remove"))
            {
                EditorApplication.Beep();

                //DisplayDialog is a very useful method to create a simple popup check. You can use this to simply display information to the user 
                //that something has changed which the user has to confirm by clicking OK, or you can even ask simple Yes, No questions to, 
                //like in this example, ask if the user really wants to delete something
                if (EditorUtility.DisplayDialog("Really?", "Do you really want to remove the state '" + m_Target.Types[index].Name + "'?", "Yes", "No") == true)
                {
                    m_Target.Types.RemoveAt(index);
                    EditorUtility.SetDirty(m_Target);
                }

            }
        }
        GUILayout.EndHorizontal();
    }
}
