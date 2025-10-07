using StateMachineAI;
using UnityEngine;
using UnityEngine.AI;
namespace StateMachineAI
{
    public class SecurityAttack : State<EnemyAI>
    {
        [Header("ビックリマーク")]
        public GameObject m_ExclamationMark;
        [Header("クエスチョンマーク")]
        public GameObject m_QuestionMark;
        //最後に視界内で見た時間
        private float m_LastTimePlayer = 0f;
        //パトロール中か？
        private bool m_Patrol = true;
        //playを検知したか？
        private bool m_LookPlayer = false;
        //一度でも感知したかどうか
        private NavMeshAgent m_navmeshagent;
        public SecuritySearch m_securitySearch;
        public SecurityAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {

        }
        public override void Stay()
        {
            //追跡開始
            m_LookPlayer = true;
            //パトロール中止
            m_Patrol = false;
            m_LastTimePlayer = Time.time;
            Debug.Log("攻撃状態に入る！");
            //プレイヤーのところまで行け
            m_navmeshagent.SetDestination(owner.m_Player.position);
            //表示
            m_ExclamationMark.SetActive(true);
            //非表示
            m_QuestionMark.SetActive(false);
        }
        
        public override void Exit()
        {

        }

    }
}


