using Knife.RealBlood.SimpleController;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class EnemySuit : MonoBehaviour
{
    //ステートの種類
    public enum AIState
    {
        Idle,
        Search,
        Walk,
        Attack,
    }
    public Transform m_Player;
    [Header("感知距離")]
    public float m_viewDistance = 10f;
    [Header("視野角（左右）")]
    public float m_viewAngle = 60f;
    [Header("見失ってから諦めるまでの秒数")]
    public float m_LostPlayerDelay = 3f;
    //現在のステート
    private AIState currentState = AIState.Idle;
    private Animator m_Animator;
    private NavMeshAgent m_navMeshAgent;
    //script参照
    private PlayerDetectionState m_playerDetectionState;
    //追跡中か
    private bool m_LookPlayer=false;
    //最後にみた時間
    private float m_LastTimePlayer = 0f;
    private void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        //初期ステート
        ChangeState(AIState.Idle);
    }
    private void Update()
    {
        //ステートに応じた処理
        if (currentState == AIState.Idle) Mode_Idle();
        if (currentState == AIState.Search) Mode_Search();
        if (currentState == AIState.Walk) Mode_Walk();
        if (currentState == AIState.Attack) Mode_Attack();
        //見えない状態
        bool isInvisible = m_playerDetectionState != null && m_playerDetectionState.IsInvisible();
        // 敵からプレイヤーの方向と距離
        Vector3 toPlayer = m_Player.position - transform.position;
        float distance = toPlayer.magnitude;
        bool isInView = false;
        //プレイヤーが透明化してなくて感知距離にいる場合
        if (!isInvisible && distance <= m_viewDistance)
        {
            //敵の視野角の判定
            Vector3 forward = transform.forward;
            Vector3 dirToPlayer = toPlayer.normalized;
            float dot = Vector3.Dot(forward, dirToPlayer);
            float threshold = Mathf.Cos(m_viewAngle * 0.5f * Mathf.Deg2Rad);
            //視野に入ったら
            if (dot >= threshold)
            {
                // プレイヤーが視界内にいる
                isInView = true;
                m_LookPlayer = true;
                m_LastTimePlayer = Time.time;

                if (currentState != AIState.Attack)
                    ChangeState(AIState.Attack); // 攻撃状態に移行
            }
        }

        // 追跡中
        if (m_LookPlayer)
        {
            if ((isInvisible || !isInView) && Time.time - m_LastTimePlayer > m_LostPlayerDelay)
            {
                // 見失った
                m_LookPlayer = false;
                ChangeState(AIState.Idle);
            }
            else
            {
                // 追跡続行
                m_navMeshAgent.SetDestination(m_Player.position);
            }
        }
    }



    void ChangeState(AIState newState)
    {
        currentState = newState;
        Debug.Log("状態変更" + newState);
        m_Animator.SetInteger("State", (int)newState);
    }
    void Mode_Idle()
    {
        Debug.Log("Idle中");
    }
    void Mode_Search()
    {
        Debug.Log("Search中");
    }
    void Mode_Walk()
    {
        Debug.Log("Walk中");
    }
    void Mode_Attack()
    {
        Debug.Log("Attack中");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f); // 赤の半透明



        // 視野角の線を描く
        Vector3 forward = transform.forward;

        // 視野角の左右の方向を計算
        Quaternion leftRayRotation = Quaternion.Euler(0, -m_viewAngle * 0.5f, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, m_viewAngle * 0.5f, 0);


        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * m_viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * m_viewDistance);


    }
}

