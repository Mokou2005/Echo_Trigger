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
    //プレイヤーを見たかどうか
    public bool m_Look;
    [SerializeField] private EnemyAI m_EnemyAI;
    [SerializeField] private AlertLevel m_AlertLevel;
    [SerializeField] private Transform m_Target;

    private void Awake()
    {
        //格納
        m_EnemyAI = GetComponent<EnemyAI>();
        m_AlertLevel = GetComponent<AlertLevel>();
    }

    private void Update()
    {
        //この関数に移動
        DetectTarget();
    }

    private void DetectTarget()
    {
        //Playerのタグを探す
        GameObject targetObj = GameObject.FindGameObjectWithTag(m_targetTag);
        //無ければ発見をないことにする
        if (targetObj == null)
        {
            m_Look = false;
            m_Target = null;
            return;
        }
        //プレイヤーにtransformをつける
        m_Target = targetObj.transform;
        //自分から敵のベクトルを計算
        Vector3 dirToTarget = (m_Target.position - transform.position).normalized;
        //ターゲットまでの距離を取得
        float distance = Vector3.Distance(transform.position, m_Target.position);
        //センサーの高さを2mに調整
        float heightDifference = Mathf.Abs(m_Target.position.y - transform.position.y);
        float maxHeightDifference = 2f;
        //forward方向との角度を計算
        float dot = Vector3.Dot(transform.forward, dirToTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 視野角＆距離＆高さチェック
        if (angle < m_viewAngle * 0.5f && distance < m_viewDistance && heightDifference <= maxHeightDifference)
        {
            // 視界に遮蔽物がないことをチェック
            if (Physics.Raycast(transform.position, dirToTarget, out RaycastHit hit, distance))
            {
                //ヒットしたのがプレイヤーなら
                if (hit.collider.CompareTag(m_targetTag))
                {
                    m_Look = true;
                    m_EnemyAI.ChangeState(AIState.Search);
                    return;
                }
            }
        }

        m_Look = false;
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
