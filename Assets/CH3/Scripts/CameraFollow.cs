using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // X 軸方向的追蹤
    [HideInInspector]
    public bool isTrackingXAxis; // 是否開啟X 軸方向的追蹤
    [HideInInspector]
    public float xMargin = 1f;   // 角色離攝影機中心多遠以上會開始追蹤
    [HideInInspector]
    public float xSmooth = 8f;   // 攝影機追蹤的速度
    [HideInInspector]
    public Vector2 minMaxX;      // 攝影機追蹤的最小最大值 (最小值, 最大值)
    [HideInInspector]
    public float xPos;           // 攝影機實際的 X 位置

    // Y 軸方向的追蹤
    [HideInInspector]
    public bool isTrackingYAxis;
    [HideInInspector]
    public float yMargin = 1f;
    [HideInInspector]
    public float ySmooth = 8f;
    [HideInInspector]
    public Vector2 minMaxY;
    [HideInInspector]
    public float yPos;

    public GameObject m_PlayerGO; // 玩家角色的 Transform

    private void Awake()
    {
    }

    // 檢查位置 X 是否超出範圍
    private bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - m_PlayerGO.transform.position.x) > xMargin;
    }

    // 檢查位置 Y 是否超出範圍
    private bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - m_PlayerGO.transform.position.y) > yMargin;
    }


    private void Update()
    {
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
            targetX = Mathf.Lerp(transform.position.x, m_PlayerGO.transform.position.x, xSmooth * Time.deltaTime);

        if (CheckYMargin())
            targetY = Mathf.Lerp(transform.position.y, m_PlayerGO.transform.position.y, ySmooth * Time.deltaTime);

        // 攝影機的位置必須要在最小和最大值之間
        targetX = Mathf.Clamp(targetX, minMaxX.x, minMaxX.y);
        targetY = Mathf.Clamp(targetY, minMaxY.x, minMaxY.y);

        // 設定攝影機位置
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    void OnDrawGizmos()
    {
        Camera cam = Camera.main;
        float cHeight = 2f * cam.orthographicSize;
        
        Vector3 pos = this.transform.position - new Vector3(0.0f, 0.0f, 1.0f);
        float region_width = xMargin * 2.0f;

        if (xMargin < 0.0f)
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        else
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f));
    }
}