using UnityEngine;

namespace StateMachineAI
{

    public class SuitAttackType : State<EnemySuit>
    {

        //コンストラクタ
        public SuitAttackType(EnemySuit owner) : base(owner) { }


    }

}
