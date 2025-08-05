using UnityEngine;
using UnityEngine.AI;

public class EnemySensor : MonoBehaviour
{
    [Header("Playerのオブジェクト")]
    public Transform m_Player;
    [Header("移動の速さ")]
    public float m_speed;
    [Header("感知の距離")]
    public float m_viewDistance = 10f;
    [Header("視野角（左右も角度）")]
    public float m_viewAngle = 60f;
    // クールタイムの長さ
    [Header("弾のクールタイム")]
    public float m_ShotCooldown;
    [Header("敵が持つ銃")]
    public GameObject m_Gun;
    [Header("見失ってから諦めるまでの秒数")]
    public float m_LostPlayerDelay = 3f;
    [Header("パトロールポイント")]
    public Transform[] m_PatrolPoints;
    [Header("パトロール時の待機秒数")]
    public float m_WaitTime = 2f;
    public Animator m_Animator;
    //どれだけ待機するかのタイマー
    private float m_WaitTimer = 0f;
    //待機状態か？
    private bool m_Wait = false;
    //パトロールポイントの 今向かっている位置(番号)
    private int m_PatrolIndex = 0;
    //パトロール中か？
    private bool m_Patrol = true;
    //最後に視界内で見た時間
    private float m_LastTimePlayer = 0f;
    // 次に撃てる長さ
    private float m_NextShotTime = 0f;
    //一度でも感知したかどうか
    private NavMeshAgent m_navmeshagent;
    //playを検知したか？
    private bool m_LookPlayer= false;
    //scriptを参照
    private Parameta m_parameta;
    private PlayerDetectionState m_PlayerDetection;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_parameta = GetComponent<Parameta>();
        m_navmeshagent = GetComponent<NavMeshAgent>();
        m_navmeshagent.speed = m_speed;
        m_PlayerDetection = m_Player.GetComponent<PlayerDetectionState>();
    }

    private void Update()
    {
        if (m_Animator != null)
        {
            //NavMeshAgent が 動いているかどうか
            bool isWalking = m_navmeshagent.velocity.magnitude > 0.1f;
            m_Animator.SetBool("Walking", isWalking);
        }
        //HPがゼロなら
        if (m_parameta.m_Hp <= 0)
        {
            m_LookPlayer = false;
            //停止
            m_navmeshagent.ResetPath();
            return;
        }
        //プレイヤーが不可視状態か
        bool isInvisible = m_PlayerDetection != null && m_PlayerDetection.IsInvisible();

        // 敵からプレイヤーの方向と距離
        Vector3 toPlayer = m_Player.position - transform.position;
        float distance = toPlayer.magnitude;
        bool isInView = false;

        if (!isInvisible && distance <= m_viewDistance)
        {
            //敵を正面に設定
            Vector3 forward = transform.forward;
            //normalizedを使って敵とのベクトルを求める
            Vector3 dirToPlayer = toPlayer.normalized;
            //敵の正面方向とプレイヤーの方向がどれだけ近いか
            float dot = Vector3.Dot(forward, dirToPlayer);
            //視野角の判定基準
            float threshold = Mathf.Cos(m_viewAngle * 0.5f * Mathf.Deg2Rad);
            //敵の視野角にいたら
            if (dot >= threshold)
            {
                //プレイヤーが視界に入った
                isInView = true;
                m_LookPlayer = true;
                m_LastTimePlayer = Time.time;
                m_Patrol = false;
                Debug.Log("攻撃状態に入る！");
                m_navmeshagent.SetDestination(m_Player.position);
            }
            else
            {
                Debug.Log("プレイヤーは範囲内だが見えていない");
            }
        }
        else if (!isInvisible)
        {
            Debug.Log("プレイヤーは遠すぎて感知できない");
        }

        // 追跡状態
        if (m_LookPlayer)
        {
            // 視界から外れ、もしくは不可視状態ならカウントを進める
            if ((isInvisible || !isInView) && Time.time - m_LastTimePlayer > m_LostPlayerDelay)
            {
                Debug.Log("プレイヤーを見失ったので追跡を終了します");
                m_LookPlayer = false;
            }
            else if (!isInvisible)
            {
                // プレイヤーが見えている or 猶予時間内は追跡継続
                m_navmeshagent.SetDestination(m_Player.position);

                if (m_Gun != null && Time.time >= m_NextShotTime)
                {
                    AssaultRifle rifle = m_Gun.GetComponent<AssaultRifle>();
                    if (rifle != null)
                    {
                        rifle.Shot();
                        m_NextShotTime = Time.time + m_ShotCooldown;
                    }
                }
            }
        }

        // パトロール実行
        if (!m_LookPlayer && m_Patrol && m_PatrolPoints.Length > 0)
        {
            PatrolMove();
        }
    }
    //パトロールの関数
    private void PatrolMove()
    {
        if (m_Wait)
        {
            m_WaitTimer -= Time.deltaTime;
            if (m_WaitTimer <= 0f)
            {
                m_Wait = false;

                // 次のポイントへ進む
                m_PatrolIndex = (m_PatrolIndex + 1) % m_PatrolPoints.Length;
                m_navmeshagent.SetDestination(m_PatrolPoints[m_PatrolIndex].position);
            }
            return;
        }

        // パトロールポイントに到達したら待機状態に入る
        if (!m_navmeshagent.pathPending && m_navmeshagent.remainingDistance < 0.5f)
        {
            m_Wait = true;
            m_WaitTimer = m_WaitTime;
            m_navmeshagent.ResetPath(); // 一旦停止
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

