using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogSearch : State<EnemyAI>
    {
        public DogSearch(EnemyAI owner) : base(owner) { }
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

