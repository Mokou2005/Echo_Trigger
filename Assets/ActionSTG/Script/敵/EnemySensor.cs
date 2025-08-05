using UnityEngine;
using UnityEngine.AI;

public class EnemySensor : MonoBehaviour
{
    public Transform m_Player;
    [Header("移動の速さ")]
    public float m_speed;
    //感知の距離
    public float m_viewDistance = 10f;
    //視野角（左右も角度）
    public float m_viewAngle = 60f;
    // クールタイムの長さ
    [Header("弾のクールタイム")]
    public float m_ShotCooldown;
    [Header("敵が持つ銃")]
    public GameObject m_Gun;
    // 次に撃てる長さ
    private float m_NextShotTime = 0f;　
    private NavMeshAgent m_navmeshagent;
    private void Start()
    {
        m_navmeshagent = GetComponent<NavMeshAgent>();
        m_navmeshagent.speed = m_speed;
    }

    private void Update()
    {
        //敵からプレイヤーの方向と距離
        Vector3 toPlayer=m_Player.position-transform.position;  
        float distance=toPlayer.magnitude;
        //プレイヤーが感知距離の入ってたら
        if (distance <= m_viewDistance)
        {
            //敵がプレイヤーの方に向く
            Vector3 forward = transform.forward;
            Vector3 dirToPlayer = toPlayer.normalized;
            //向きを調べる
            float dot=Vector3.Dot(forward, dirToPlayer);
            float threshold = Mathf.Cos(m_viewAngle * 0.5f * Mathf.Deg2Rad);
            if (dot >= threshold)
            {
                Debug.Log("攻撃状態に入る！");
                //playerに追跡
                m_navmeshagent.SetDestination(m_Player.position);
                //m_Gunがあるかとm_NextShotTimeより大きかったら
                if (m_Gun != null && Time.time >= m_NextShotTime)
                {
                    AssaultRifle rifle = m_Gun.GetComponent<AssaultRifle>();
                    if (rifle != null)
                    {
                        rifle.Shot();  // 正しく発射処理を呼び出す
                        m_NextShotTime = Time.time + m_ShotCooldown;
                    }
                }

            }
            else
            {
                Debug.Log("プレイヤーは範囲内だが見えていない");
            }
        }
        else
        {
            Debug.Log("プレイヤーは遠すぎて感知できない");
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

