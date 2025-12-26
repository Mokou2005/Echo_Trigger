using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachineAI
{

    public class DogAttack : State<EnemyAI>
    {
        private Bite m_Bite;
        public DogAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            Bite Bi = owner.GetComponent<Bite>();
            if (Bi == null)
            {
                Debug.Log("BiteÇ™Ç»Ç©Ç¡ÇΩÇÃÇ≈é©ìÆí«â¡ÇµÇ‹Ç∑ÅB");
                Bi = owner.gameObject.AddComponent<Bite>();
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
