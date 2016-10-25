using UnityEngine;
using UnityEditor;
using System.Collections;

public class PreviewTime
{
    public static float Time
    {
        get
        {
            if (Application.isPlaying == true)
            {
                return UnityEngine.Time.timeSinceLevelLoad;
            }
            
            // EditorPrefs 就像 PlayerPrefs 一樣，只是它是用在 Editor 中。
            // 它可以永久的儲存一個變數，即便你把 Editor 關掉也是一樣
            return EditorPrefs.GetFloat("PreviewTime", 0f);
        }
        set
        {
            EditorPrefs.SetFloat("PreviewTime", value);
        }
    }
}
