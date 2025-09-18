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
        Attack,

    }

    [Header("出現させたい敵のPrefab")]
    public GameObject enemyPrefab;
    [Header("出現位置")]
    public Transform spawnPoint;
    [Header("会社員の位置")]
    public Transform m_suitEnemy;
    [Header("呼ぶ敵")]
    public Transform m_EnemyCall;
    public Transform m_Player;
    [Header("感知距離")]
    public float m_viewDistance = 10f;
    [Header("視野角（左右）")]
    public float m_viewAngle = 60f;
    [Header("見失ってから諦めるまでの秒数")]
    public float m_LostPlayerDelay = 3f;
    [Header("警戒度の上昇速度(1秒あたり)")]
    public float m_VigilanceLevelIncreaseCount = 30f;
    [Header("警戒度の減少速度（1秒あたり）")]
    public float m_VigilanceLevelDecreaseCount = 5f;
    //警戒度
    private float m_VigilanceLevel = 0f;
    //警戒度MAX
    private float m_VigilanceLevelMax = 100f;

    //現在のステート
    private AIState currentState = AIState.Idle;
    private Animator m_Animator;
    private NavMeshAgent m_navMeshAgent;
    //script参照
    private PlayerDetectionState m_playerDetectionState;
    //追跡中か
    private bool m_LookPlayer = false;
    //最後にみた時間
    private float m_LastTimePlayer = 0f;
    //味方を読んだかどうか？
    private bool m_hasCalled = false;
    private void Start()
    {
        m_playerDetectionState = m_Player.GetComponent<PlayerDetectionState>();
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
                // プレイヤーが視界内に入った
                isInView = true;
                m_LastTimePlayer = Time.time;
                if (currentState != AIState.Search)
                    // 呼ぶに移行
                    ChangeState(AIState.Search);
            }
            else
            {

                Debug.Log("プレイヤーは見えない");

                //警戒度減少
                m_VigilanceLevel -= m_VigilanceLevelDecreaseCount * Time.deltaTime;
                //警戒度の上限設定
                m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
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
        Debug.Log("疑っている");

        //警戒度を加算
        m_VigilanceLevel += m_VigilanceLevelIncreaseCount * Time.deltaTime;
        //警戒度の上限を設定
        m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
        Debug.Log($"警戒中.警戒度:{m_VigilanceLevel}");
        //警戒度が100に達したら
        if (m_VigilanceLevel >= m_VigilanceLevelMax)
        {
            Debug.Log("見つかってしまった！！");
            if (currentState != AIState.Attack)
                // 呼ぶに移行
                ChangeState(AIState.Attack);
        }
    }
    void Mode_Attack()
    {
        // まだ呼んでない場合のみ呼ぶ
        if (!m_hasCalled)
        {
            m_hasCalled = true; // 一度だけ呼ぶ

            // 敵を生成
            if (enemyPrefab != null && spawnPoint != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                // EnemySensor の m_Player にプレイヤーを代入
                EnemySensor sensor = enemy.GetComponent<EnemySensor>();
                if (sensor != null && m_Player != null)
                {
                    sensor.m_Player = m_Player;
                }
            }
            else
            {
                Debug.LogWarning("enemyPrefab または spawnPoint が設定されていません");
            }

            // 仲間を移動
            NavMeshAgent agent = m_EnemyCall.GetComponent<NavMeshAgent>();
            if (agent != null && m_suitEnemy != null)
            {
                agent.SetDestination(m_suitEnemy.position);
            }
        }


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

