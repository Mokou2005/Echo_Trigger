using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("カメラの注視ターゲット（プレイヤーの頭）")]
    public Transform m_Target;

    [Header("カメラの理想距離")]
    public float m_Distance = 4.0f;

    [Header("高さの補正（プレイヤーの背中より少し上）")]
    public Vector3 m_Offset = new Vector3(0, 2, 0);

    [Header("壁との余白距離")]
    public float m_SafetyMargin = 0.2f;

    [Header("追従スピード")]
    public float m_MoveSpeed = 10f;

    private void LateUpdate()
    {
        // 注視点（プレイヤーの頭位置）
        Vector3 targetPosition = m_Target.position + m_Offset;

        // 理想的なカメラ位置
        Vector3 desiredCameraPos = targetPosition - m_Target.forward * m_Distance;

        RaycastHit hit;
        if (Physics.Linecast(targetPosition, desiredCameraPos, out hit))
        {
            // 壁に当たったら手前にずらす
            Vector3 hitPoint = hit.point + hit.normal * m_SafetyMargin;
            transform.position = Vector3.Lerp(transform.position, hitPoint, Time.deltaTime * m_MoveSpeed);
        }
        else
        {
            // 壁に当たらなければ普通に追従
            transform.position = Vector3.Lerp(transform.position, desiredCameraPos, Time.deltaTime * m_MoveSpeed);
        }

        // プレイヤーの頭方向を見る
        transform.LookAt(targetPosition);
    }
}
