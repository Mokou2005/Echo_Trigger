using StateMachineAI;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [Header("検知する対象")]
    public string m_targetTag = "Player";
    [Header("感知距離")]
    public float m_viewDistance = 10f;
    [Header("視野角（左右）")]
    public float m_viewAngle = 60f;

    public bool m_Look;
    private EnemyAI m_EnemyAI;
    private AlertLevel m_AlertLevel;
    private Transform m_Target;

    private void Awake()
    {
        m_EnemyAI = GetComponent<EnemyAI>();
        m_AlertLevel = GetComponent<AlertLevel>();
    }

    private void Update()
    {
        DetectTarget();
    }

    private void DetectTarget()
    {
        GameObject targetObj = GameObject.FindGameObjectWithTag(m_targetTag);
        if (targetObj == null)
        {
            m_Look = false;
            m_Target = null;
            return;
        }

        m_Target = targetObj.transform;

        Vector3 dirToTarget = (m_Target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, m_Target.position);

        // 高さ差を追加
        float heightDifference = Mathf.Abs(m_Target.position.y - transform.position.y);
        float maxHeightDifference = 2f; // 高さの最大差（必要に応じて調整）

        float dot = Vector3.Dot(transform.forward, dirToTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < m_viewAngle * 0.5f && distance < m_viewDistance && heightDifference <= maxHeightDifference)
        {
            m_Look = true;
            m_EnemyAI.ChangeState(AIState.Search);
        }
        else
        {
            m_Look = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_viewDistance);

        Vector3 leftDir = Quaternion.Euler(0, -m_viewAngle / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, m_viewAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftDir * m_viewDistance);
        Gizmos.DrawRay(transform.position, rightDir * m_viewDistance);
    }
}
