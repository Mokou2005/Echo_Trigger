using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{
    public class IdleSuitType : State<EnemyAI>
    {
        [Header("見失ってから諦めるまでの秒数")]
        public float m_LostPlayerDelay = 3f;
        [Header("警戒度の上昇速度(1秒あたり)")]
        public float m_VigilanceLevelIncreaseCount = 30f;
        [Header("警戒度の減少速度（1秒あたり）")]
        public float m_VigilanceLevelDecreaseCount = 5f;
        private NavMeshAgent m_navMeshAgent;
        //script参照
        private PlayerDetectionState m_playerDetectionState;
        //警戒度
        private float m_VigilanceLevel = 0f;
        //警戒度MAX
        private float m_VigilanceLevelMax = 100f;
        //最後にみた時間
        private float m_LastTimePlayer = 0f;
        //コンストラクタ
        public IdleSuitType(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            m_playerDetectionState = owner.m_Player.GetComponent<PlayerDetectionState>();
            m_navMeshAgent = owner.GetComponent<NavMeshAgent>();
        }
    
        public override void Stay()
        {
            //見えない状態
            bool isInvisible = m_playerDetectionState != null && m_playerDetectionState.IsInvisible();
            // 敵からプレイヤーの方向と距離
            Vector3 toPlayer = owner.m_Player.position - owner.transform.position;
            float distance = toPlayer.magnitude;
            bool isInView = false;
            //プレイヤーが透明化してなくて感知距離にいる場合
            if (!isInvisible && distance <=owner. m_viewDistance)
            {
                //敵の視野角の判定
                Vector3 forward = owner.transform.forward;
                Vector3 dirToPlayer = toPlayer.normalized;
                float dot = Vector3.Dot(forward, dirToPlayer);
                float threshold = Mathf.Cos(owner.m_viewAngle * 0.5f * Mathf.Deg2Rad);
                //視野に入ったら
                if (dot >= threshold)
                {
                    // プレイヤーが視界内に入った
                    isInView = true;
                    m_LastTimePlayer = Time.time;
                    owner.ChangeState(AIState.SearchSuitType);
                }
                else
                {

                    Debug.Log("プレイヤーは見えない");

                    //警戒度減少
                    m_VigilanceLevel -= m_VigilanceLevelDecreaseCount * Time.deltaTime;
                    //警戒度の上限設定
                    m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                    Debug.Log($"警戒中.警戒度:{m_VigilanceLevel}");
                }
            }
        }
        public override void Exit() { }

    }

}