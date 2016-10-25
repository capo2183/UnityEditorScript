using UnityEngine;
using UnityEditor;
using System.Collections;


// EditorWindow 是另外一個好用的Unity插件模式，它可以跳出自定義視窗
// 就像 Scene View 或 Inspector View 那樣的視窗
public class PreviewPlaybackWindow : EditorWindow
{
    // 設定視窗可以在 Menu Bar 中的哪一個路徑中將它開啟
    [MenuItem("Window/Preview Playback Window")]
    static void OpenPreviewPlaybackWindow()
    {
        EditorWindow.GetWindow<PreviewPlaybackWindow>(false, "Playback");
    }

    float m_PlaybackModifier;
    float m_LastTime;

    void OnEnable()
    {
        //This update callback is called 30 times per second in the editor. Basically it's 
        //an Update() function you can use at edit time
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }

    void OnUpdate()
    {
        if (m_PlaybackModifier != 0f)
        {
            PreviewTime.Time += (Time.realtimeSinceStartup - m_LastTime) * m_PlaybackModifier;
            
            // 我們需要強制讓 Window 內的物件重畫，才能看到 Preview Time 在計時。否則，Unity 只有在特定的時間會重畫 Windows
            // 例如 : 移動視窗的時候
            Repaint();

            // 我們也需要重畫場景，才能預覽畫面
            SceneView.RepaintAll();
        }

        m_LastTime = Time.realtimeSinceStartup;
    }

    void OnGUI()
    {
        float seconds = Mathf.Floor(PreviewTime.Time % 60);
        float minutes = Mathf.Floor(PreviewTime.Time / 60);

        GUILayout.Label("Preview Time: " + minutes + ":" + seconds.ToString("00"));
        GUILayout.Label("Playback Speed: " + m_PlaybackModifier);

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("|<", GUILayout.Height(30)))
            {
                PreviewTime.Time = 0f;
                SceneView.RepaintAll();
            }

            if (GUILayout.Button("<<", GUILayout.Height(30)))
            {
                m_PlaybackModifier = -5f;
            }

            if (GUILayout.Button("<", GUILayout.Height(30)))
            {
                m_PlaybackModifier = -1f;
            }

            if (GUILayout.Button("||", GUILayout.Height(30)))
            {
                m_PlaybackModifier = 0f;
            }

            if (GUILayout.Button(">", GUILayout.Height(30)))
            {
                m_PlaybackModifier = 1f;
            }

            if (GUILayout.Button(">>", GUILayout.Height(30)))
            {
                m_PlaybackModifier = 5f;
            }
        }
        GUILayout.EndHorizontal();
    }
}
