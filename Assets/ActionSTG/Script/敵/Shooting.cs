using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.AudioSettings;

public class Shooting : MonoBehaviour
{
    [Header("Playerの位置")]
    [SerializeField] private Transform m_Target;
    [Header("外す位置（弾）")]
    //[SerializeField] private Transform[] m_RemoveBullet;
    [Header("Playに当たる確率")]
    [SerializeField] private float m_BulletProbability = 0.8f;
    [Header("射程距離を保つ距離")]
    [SerializeField] private float m_CombatDistance = 5f;
    [Header("射程距離内での移動速度")]
    [SerializeField] private float m_MoveSpeed = 2f;
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private Animator m_Animator;
    //射程距離に入ったかどうか
    private bool m_ShootAria = false;
    private int m_Count;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        // Tagからプレイヤーを自動で探す
        if (m_Target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                m_Target = player.transform;
            }
            else
            {
                Debug.LogWarning("Playerタグのオブジェクトが見つかりません！");
            }
        }
    }


    void Update()
    {


        // ターゲットが未設定なら、プレイヤーを探す
        if (m_Target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                m_Target = player.transform;
        }
        //ターゲットがないなら返す
        if (m_Target == null)
        {
            Debug.LogError("ターゲットがありません"); return;
        }
        //自分のポジションと敵のポジションの間を図る
        float distance = Vector3.Distance(transform.position, m_Target.position);

        //射程距離に入ってなければ
        if (!m_ShootAria)
        {
            //射程外なので接近
            m_Animator.SetBool("Run", true);
            m_agent.enabled = true;
            m_agent.speed = 3.5f;
            //プレイヤーの方向まで移動
            m_agent.SetDestination(m_Target.position);
        }
        //射程距離に入ったら
        else
        {
            //射程内なので一定の距離を保つ
            m_Animator.SetBool("Run", true);
            m_agent.enabled = true;
            //速度を射程内の時移動速度に変更
            m_agent.speed = m_MoveSpeed;
            // 距離が近すぎる場合少し離れる
            if (distance < m_CombatDistance - 1f)
            {
                Vector3 dirAway = (transform.position - m_Target.position).normalized;
                Vector3 newPos = transform.position + dirAway * 2f; // 少し後退
                m_agent.SetDestination(newPos);
            }
            // 距離が遠すぎる場合近づく
            else if (distance > m_CombatDistance + 1f)
            {
                m_agent.SetDestination(m_Target.position);
            }
            else
            {
                // ちょうどいい距離 → 立ち止まって攻撃待機
                m_agent.SetDestination(transform.position);
                m_Animator.SetBool("Run", false);
                // 攻撃アニメなどここで
                // m_Animator.SetTrigger("Shoot");
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //エリアに入った
            m_ShootAria = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 射程外に出た
            m_ShootAria = false;
        }
    }
}
