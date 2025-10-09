using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Header("敵の移動ポイントを導入")]
    public Transform[] m_Waypoints;

    void Awake()
    {
        // 子オブジェクト自動登録
        if (m_Waypoints == null || m_Waypoints.Length == 0)
        {
            m_Waypoints = GetComponentsInChildren<Transform>();
        }
        else
        {
            Debug.Log("敵のポイント移動が設定されていない、またはポイントがアタッチされていないです。");
        }
    }
}
