using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogAttack : State<EnemyAI>
    {
        private Animator m_Animator;
        private DogSound m_DogSound;
        //‰“–i‚¦‚ð‚µ‚½‚©
        private bool m_Howl=false;
        public DogAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            m_Animator=owner.GetComponent<Animator>();
            m_DogSound= owner.GetComponent<DogSound>();

        }

        public override void Stay()
        {
            //‚Ü‚¾–i‚¦‚Ä–³‚¯‚ê‚Î
            if (m_Howl==false)
            {
                m_Animator.SetTrigger("Howling");
                m_DogSound.m_Source.PlayOneShot(m_DogSound.m_Howling);
                m_Howl=true;
            }
        }

        public override void Exit()
        {
        }
    }

}
