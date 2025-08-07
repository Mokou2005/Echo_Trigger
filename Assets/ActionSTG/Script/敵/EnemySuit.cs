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
    //現在のステート
    private AIState currentState = AIState.Idle;
    private Animator m_Animator;
    private NavMeshAgent m_navMeshAgent;
    //script参照
    private PlayerDetectionState m_playerDetectionState;
    private void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        //初期ステート
        ChangeState(AIState.Idle);
    }
    private void Update()
    {
        if (currentState == AIState.Idle)
        {
            Mode_Idle();
        }
        //見えない状態
        bool isInvisible = m_playerDetectionState != null && m_playerDetectionState.IsInvisible();
        // 敵からプレイヤーの方向と距離
        Vector3 toPlayer = m_Player.position - transform.position;
        float distance = toPlayer.magnitude;
        bool isInView = false;
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
    void Moge_Attack()
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

