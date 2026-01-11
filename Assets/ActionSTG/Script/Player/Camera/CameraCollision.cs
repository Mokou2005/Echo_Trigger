using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("カメラの注視ターゲット（プレイヤー）")]
    public Transform m_Target;

    [Header("カメラの理想距離")]
    public float m_Distance = 4.0f;

    [Header("高さの補正（プレイヤーの背中より少し上）")]
    public Vector3 m_Offset = new Vector3(0, 2, 0);

    [Header("壁との余白距離")]
    public float m_SafetyMargin = 0.2f;

    [Header("追従スピード")]
    public float m_MoveSpeed = 10f;

    [Header("カメラの衝突半径（SphereCast用）")]
    public float m_CameraRadius = 0.3f;

    [Header("衝突検出するレイヤー（壁・床など）")]
    public LayerMask m_CollisionLayers = ~0; // デフォルトは全レイヤー

    [Header("上下回転設定")]
    public float pitchSpeed = 60f;   // 視線の上下スピード
    public float minPitch = -30f;    // 下を向く限界角度
    public float maxPitch = 45f;     // 上を向く限界角度

    [Header("Parametaをアタッチ")]
    public Parameta m_Parameta;

    private float m_Pitch = 0f;      // 現在の上下回転角度

    private void Start()
    {
        if (m_Target == null) return;

        // 初期位置をターゲットの後方に設定
        Vector3 targetPosition = m_Target.position + m_Offset;
        Vector3 initialPos = targetPosition - m_Target.forward * m_Distance;
        transform.position = initialPos;
        transform.LookAt(targetPosition);
    }

    private void Update()
    {
        // Parameta があって、死んでいたらこのスクリプトを無効化
        if (m_Parameta != null && m_Parameta.m_IsDie)
        {
            this.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (m_Target == null) return;

        // マウス入力で上下角度を調整
        float mouseY = Input.GetAxis("Mouse Y");
        m_Pitch -= mouseY * pitchSpeed * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, minPitch, maxPitch);

        // 注視点（プレイヤーの頭位置）
        Vector3 targetPosition = m_Target.position + m_Offset;

        // カメラの回転を計算（プレイヤーの回転 + 上下角度）
        Quaternion baseRot = m_Target.rotation;
        Quaternion pitchRot = Quaternion.Euler(m_Pitch, 0f, 0f);
        Quaternion finalRot = baseRot * pitchRot;

        // カメラの後方方向（回転を考慮）
        Vector3 backDirection = finalRot * Vector3.back;

        // カメラの理想位置（プレイヤーの後方）
        Vector3 desiredCameraPos = targetPosition + backDirection * m_Distance;

        // 最終的なカメラ位置
        Vector3 finalCameraPos = desiredCameraPos;

        // SphereCastで壁との衝突を検出
        Vector3 direction = (desiredCameraPos - targetPosition).normalized;
        float rayDistance = m_Distance;
        
        RaycastHit hit;
        if (Physics.SphereCast(targetPosition, m_CameraRadius, direction, out hit, rayDistance, m_CollisionLayers))
        {
            // 壁に当たったら、衝突点の手前にカメラを配置
            float safeDistance = hit.distance - m_SafetyMargin;
            safeDistance = Mathf.Max(safeDistance, m_CameraRadius); // 最低距離を確保
            finalCameraPos = targetPosition + direction * safeDistance;
        }

        // スムーズに追従
        transform.position = Vector3.Lerp(transform.position, finalCameraPos, Time.deltaTime * m_MoveSpeed);

        // カメラの回転を適用
        transform.rotation = finalRot;
    }
}


