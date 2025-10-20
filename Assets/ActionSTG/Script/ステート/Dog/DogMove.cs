using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{
    public class DogMove : State<EnemyAI>
    {
        

        public DogMove(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            //ˆÚ“®script‚ğ“±“ü
            EnemyPatrol_Waypoint col = owner.GetComponent<EnemyPatrol_Waypoint>();
            Sensor Sen = owner.GetComponent<Sensor>();
            AlertLevel level=owner.GetComponent<AlertLevel>();
            if (Sen == null)
            {
                Debug.Log("Sensor‚ª‚È‚©‚Á‚½‚Ì‚Å©“®’Ç‰Á‚µ‚Ü‚·B");
                Sen = owner.gameObject.AddComponent<Sensor>();
            }
            if (col == null)
            {
                Debug.Log("EnemyPatrol_Waypoint‚ª‚È‚©‚Á‚½‚Ì‚Å©“®’Ç‰Á‚µ‚Ü‚·B");
                col = owner.gameObject.AddComponent<EnemyPatrol_Waypoint>();
            }
            if (level==null)
            {
                Debug.Log("AlertLevel‚ª‚È‚©‚Á‚½‚Ì‚Å©“®’Ç‰Á‚µ‚Ü‚·B");
                level = owner.gameObject.AddComponent<AlertLevel>();
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

