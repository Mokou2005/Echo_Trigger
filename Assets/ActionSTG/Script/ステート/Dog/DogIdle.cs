using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogIdle : State<EnemyAI>
    {
        public DogIdle(EnemyAI owner) : base(owner) { }
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
