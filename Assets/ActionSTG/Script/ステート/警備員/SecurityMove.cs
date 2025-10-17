using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{
    public class SecurityMove : State<EnemyAI>
    {
        [Header("感知の距離")]
        public float m_viewDistance = 10f;
        [Header("視野角（左右も角度）")]
        public float m_viewAngle = 60f;
        [Header("Playerのオブジェクト")]
        public Transform m_Player;
        public Animator m_Animator;
        //一度でも感知したかどうか
        private NavMeshAgent m_navmeshagent;
        //playを検知したか？
        private bool m_LookPlayer = false;
        //scriptを参照
        private Parameta m_parameta;
        private PlayerDetectionState m_PlayerDetection;
        public SecurityMove(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            m_Animator = owner.GetComponent<Animator>();
            m_parameta = owner.GetComponent<Parameta>();
            m_PlayerDetection = owner.m_Player.GetComponent<PlayerDetectionState>();
        }
        public override void Stay()
        {
            if (m_Animator != null)
            {
                //NavMeshAgent が 動いているかどうか
                bool isWalking = m_navmeshagent.velocity.magnitude > 0.1f;
                //止まったらIdelのアニメーション
                m_Animator.SetBool("idle", isWalking);

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
            Vector3 toPlayer = m_Player.position - owner.transform.position;
            float distance = toPlayer.magnitude;
            bool isInView = false;
            if (!isInvisible && distance <= m_viewDistance)
            {

                //敵を正面に設定
                Vector3 forward = owner.transform.forward;
                //normalizedを使って敵とのベクトルを求める
                Vector3 dirToPlayer = toPlayer.normalized;
                //敵の正面方向とプレイヤーの方向がどれだけ近いか
                float dot = Vector3.Dot(forward, dirToPlayer);
                //視野角の判定基準
                float threshold = Mathf.Cos(m_viewAngle * 0.5f * Mathf.Deg2Rad);
                //敵の視野角にいたら
                if (dot >= threshold)
                {
                    Debug.Log("SecuritySearchに移行");
                    owner.ChangeState(AIState.Search);
                }
            }
        }

        public override void Exit() { }
    }
}
