using StateMachineAI;
using UnityEngine;
using UnityEngine.AI;
namespace StateMachineAI
{
    public class SecuritySearch : State<EnemyAI>
    {
        [Header("ビックリマーク")]
        public GameObject m_ExclamationMark;
        [Header("クエスチョンマーク")]
        public GameObject m_QuestionMark;
        [Header("感知の距離")]
        public float m_viewDistance = 10f;
        [Header("視野角（左右も角度）")]
        public float m_viewAngle = 60f;
        [Header("警戒度の上昇速度(1秒あたり)")]
        public float m_VigilanceLevelIncreaseCount = 30f;
        [Header("警戒度の減少速度（1秒あたり）")]
        public float m_VigilanceLevelDecreaseCount = 5f;
        //警戒度
        private float m_VigilanceLevel = 0f;
        //警戒度MAX
        private float m_VigilanceLevelMax = 50f;
        //最後に視界内で見た時間
        private float m_LastTimePlayer = 0f;
        //playを検知したか？
        private bool m_LookPlayer = false;
        //一度でも感知したかどうか
        private NavMeshAgent m_navmeshagent;
        //パトロール中か？
        private bool m_Patrol = true;
        private PlayerDetectionState m_PlayerDetection;
        public SecuritySearch(EnemyAI owner) : base(owner) { }

        public override void Enter()
        {

        }
        public override void Stay()
        {
            //プレイヤーが不可視状態か
            bool isInvisible = m_PlayerDetection != null && m_PlayerDetection.IsInvisible();
            // 敵からプレイヤーの方向と距離
            Vector3 toPlayer = owner.m_Player.position - owner.transform.position;
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
                    //プレイヤーが視界に入った
                    isInView = true;
                    //警戒度を加算
                    m_VigilanceLevel += m_VigilanceLevelIncreaseCount * Time.deltaTime;
                    //警戒度の上限を設定
                    m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                    //表示
                    m_QuestionMark.SetActive(true);
                    Debug.Log($"警戒中.警戒度:{m_VigilanceLevel}");
                    //警戒度が100に達したら
                    if (m_VigilanceLevel >= m_VigilanceLevelMax)
                    {
                        owner.ChangeState(AIState.Attack);
                        
                    }

                }
                else
                {
                    Debug.Log("プレイヤーは範囲内だが見えていない");
                    //非表示
                    m_QuestionMark.SetActive(false);
                    m_ExclamationMark.SetActive(false);
                }
            }
            else if (!isInvisible)
            {
                Debug.Log($"プレイヤーは遠すぎて感知できない.警戒度:{m_VigilanceLevel}");
                //警戒度減少
                m_VigilanceLevel -= m_VigilanceLevelDecreaseCount * Time.deltaTime;
                //警戒度の上限設定
                m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                //非表示
                m_QuestionMark.SetActive(false);
                m_ExclamationMark.SetActive(false);

            }
        }

        public override void Exit()
        {

        }

    }
}



