using StateMachineAI;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    [Header("検知する対象")]
    public string m_targetTag = "Player";
    [Header("感知距離")]
    public float m_viewDistance = 10f;
    [Header("視野角（左右）")]
    public float m_viewAngle = 60f;
    //プレイヤーを見たら
    public bool m_Look;
    private EnemyAI m_EnemyAI;
    private Transform m_Target;
    private AlertLevel m_AlertLevel;
    private SphereCollider m_Collider;

    private void Awake()
    {
        m_EnemyAI = GetComponent<EnemyAI>();
        m_Collider = GetComponent<SphereCollider>();
        m_Collider.isTrigger = true;
        m_Collider.radius = m_viewDistance;
        m_AlertLevel = GetComponent<AlertLevel>();
    }

    private void OnTriggerStay(Collider other)
    {
        //Tagがプレイヤーなら
        if (!other.CompareTag(m_targetTag)) return;
        //ターゲットの位置を格納
        m_Target = other.transform;
        //センサーを設定
        Vector3 dirToTarget = (m_Target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, m_Target.position);
        float dot = Vector3.Dot(transform.forward, dirToTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // プレイヤーが視野内にいる場合
        if (angle < m_viewAngle * 0.5f && distance < m_viewDistance)
        {
            m_Look = true;
            m_EnemyAI.ChangeState(AIState.Search);        
          
        }
        else
        {
            m_Look = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_targetTag))
        {
            m_Look = false;
            m_Target = null;
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
