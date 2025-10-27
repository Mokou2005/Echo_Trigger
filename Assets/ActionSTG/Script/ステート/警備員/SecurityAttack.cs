using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class SecurityAttack : State<EnemyAI>
    {
        public SecurityAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            Shooting Sho = owner.GetComponent<Shooting>();
            if (Sho == null)
            {
                Debug.Log("Shooting‚ª‚È‚©‚Á‚½‚Ì‚ÅŽ©“®’Ç‰Á‚µ‚Ü‚·");
            Sho = owner.gameObject.AddComponent<Shooting>();
            }
        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}