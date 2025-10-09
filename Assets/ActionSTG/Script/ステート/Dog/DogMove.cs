using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogMove : State<EnemyAI>
    {
        public DogMove(EnemyAI owner) : base(owner) { }
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

