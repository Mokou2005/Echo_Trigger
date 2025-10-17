using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    [Header("検知する対象")]
    public string m_targetTag="Player";
    [Header("警戒度の上昇速度(1秒あたり)")]
    public float m_VigilanceLevelIncreaseCount = 30f;
    [Header("警戒度の減少速度（1秒あたり）")]
    public float m_VigilanceLevelDecreaseCount = 5f;
    [Header("感知距離")]
    public float m_viewDistance = 10f;
    [Header("視野角（左右）")]
    public float m_viewAngle = 60f;
    //警戒度
    private float m_VigilanceLevel = 0f;
    //警戒度MAX
    private float m_VigilanceLevelMax = 100f;
    private Transform m_Target;
    private bool isVisible = false;

    private void Awake()
    {
        //検知するため
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = m_viewDistance;
    }

    private void OnTriggerStay(Collider other)
    {
        //検知内にプレイヤーがいたら
        if (other.CompareTag(m_targetTag))
        {
            m_Target = other.transform;

            // 向きと距離で視野内チェック
            Vector3 dirToTarget = (m_Target.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, m_Target.position);
            float dot = Vector3.Dot(transform.forward, dirToTarget);

            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle < m_viewAngle * 0.5f && distance < m_viewDistance)
            {
                // プレイヤーが視野内にいる場合、警戒度を上げる
                m_VigilanceLevel += m_VigilanceLevelIncreaseCount * Time.deltaTime;
                Debug.Log($"警戒中（視認中）: {m_VigilanceLevel}");
            }
            else
            {
                // 視野外なら警戒度を下げる
                m_VigilanceLevel -= m_VigilanceLevelDecreaseCount * Time.deltaTime;
                Debug.Log($"警戒中（未視認）: {m_VigilanceLevel}");
            }
            // デバッグ用
            if (m_VigilanceLevel >= m_VigilanceLevelMax)
            {

                Debug.Log("敵が完全にプレイヤーを発見！");
            }

            m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0, m_VigilanceLevelMax);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_targetTag))
        {
            m_Target = null;
        }
    }



    public bool IsPlayerVisible()
    {
        return isVisible;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_viewDistance);

        // 扇形の視野可視化
        Vector3 leftDir = Quaternion.Euler(0, -m_viewAngle / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, m_viewAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftDir * m_viewDistance);
        Gizmos.DrawRay(transform.position, rightDir * m_viewDistance);
    }
}

