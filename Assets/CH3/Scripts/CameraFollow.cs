using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // X 軸方向的追蹤
    public bool isTrackingXAxis; // 是否開啟X 軸方向的追蹤
    public float xMargin = 1f;   // 角色離攝影機中心多遠以上會開始追蹤
    public float xSmooth = 8f;   // 攝影機追蹤的速度                                                                                                                                                                                                                                                                                                                                                                                                                                                    
    public Vector2 minMaxX;      // 攝影機追蹤的最小最大值 (最小值, 最大值)
    public float xPos;           // 攝影機實際的 X 位置

    // Y 軸方向的追蹤
    public bool isTrackingYAxis;
    public float yMargin = 1f;
    public float ySmooth = 8f;
    public Vector2 minMaxY;
    public float yPos;

    private Transform m_Player; // 玩家角色的 Transform

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // 檢查位置 X 是否超出範圍
    private bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - m_Player.position.x) > xMargin;
    }

    // 檢查位置 Y 是否超出範圍
    private bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin;
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
            targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth * Time.deltaTime);

        if (CheckYMargin())
            targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth * Time.deltaTime);

        // 攝影機的位置必須要在最小和最大值之間
        targetX = Mathf.Clamp(targetX, minMaxX.x, minMaxX.y);
        targetY = Mathf.Clamp(targetY, minMaxY.x, minMaxY.y);

        // 設定攝影機位置
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}