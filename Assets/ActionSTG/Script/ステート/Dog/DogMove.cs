using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogMove : State<EnemyAI>
    {
        public DogMove(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            Debug.Log("DogMove‚ÉˆÚs");
            //ˆÚ“®script‚ğ“±“ü
            EnemyPatrol_Waypoint col = owner.GetComponent<EnemyPatrol_Waypoint>();
        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}

