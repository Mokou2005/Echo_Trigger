using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogAttack : State<EnemyAI>
    {
        public DogAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {

        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}
