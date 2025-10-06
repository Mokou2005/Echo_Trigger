using System;
using System.Reflection;
using UnityEngine;
namespace StateMachineAI
{
    public enum AIState
    {
        IdleSuitType,
        SearchSuitType,
        AttackSuitType,
    }
    public class EnemyAI : StatefulObjectBase<EnemyAI, AIState>
    {
        public Transform m_Player;
        [Header("呼ぶ敵")]
        public Transform m_EnemyCall;
        [Header("出現させたい敵のPrefab")]
        public GameObject enemyPrefab;
        [Header("出現位置")]
        public Transform spawnPoint;
        [Header("会社員の位置")]
        public Transform m_suitEnemy;
        [Header("感知距離")]
        public float m_viewDistance = 10f;
        [Header("視野角（左右）")]
        public float m_viewAngle = 60f;
        private Animator m_Animator;

        public Respon RSP;

        public bool m_StartFlag = false;
        /*
        private void Update()
        {
            //存在していないクラスが指定されたら本体消滅
            // if (!AddStateByName("IdleType")) Destroy(gameObject);
            // if (!AddStateByName("SearchType")) Destroy(gameObject);
            // if (!AddStateByName("AttackType")) Destroy(gameObject);
            //ステートマシーンを自身として設定
            //stateMachine = new StateMachine<EnemySuitAI>();
            //m_Animator = GetComponent<Animator>();
            //初期起動時は、A_Modeに移行させる
            //ChangeState(AIState.IdleType);

        }
        */

        public void AISetUp()
        {

                //ステートマシーンを自身として設定
                stateMachine = new StateMachine<EnemyAI>();
                m_Animator = GetComponent<Animator>();
                //初期起動時は、A_Modeに移行させる
                ChangeState(AIState.IdleSuitType);
        }





        public bool AddStateByName(string className)
        {
            try
            {
                // 現在のアセンブリからクラスを取得
                Type stateType = Assembly.GetExecutingAssembly().GetType($"StateMachineAI.{className}");

                // クラスが見つからなかった場合の対処
                if (stateType == null)
                {
                    Debug.LogError($"{className} クラスが見つかりませんでした。");
                    return false;
                }

                // 型チェック
                if (!typeof(State<EnemyAI>).IsAssignableFrom(stateType))
                {
                    Debug.LogError($"{className} は State<EnemySuitAI> 型ではありません。");
                    return false;
                }

                // コンストラクタ取得
                ConstructorInfo constructor = stateType.GetConstructor(new[] { typeof(EnemyAI) });
                if (constructor == null)
                {
                    Debug.LogError($"{className} のコンストラクタが見つかりませんでした。");
                    return false;
                }

                // インスタンス生成
                State<EnemyAI> stateInstance =
                    constructor.Invoke(new object[] { this }) as State<EnemyAI>;

                if (stateInstance != null)
                {
                    // ステートリストに追加
                    stateList.Add(stateInstance);
                    Debug.Log($"{className} をステートリストに追加しました。");
                    return true;
                }
                else
                {
                    Debug.LogError($"{className} のインスタンス生成に失敗しました。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"エラーが発生しました。: {ex.Message}");
                return false;
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.2f); // 赤の半透明



            // 視野角の線を描く
            Vector3 forward = transform.forward;

            // 視野角の左右の方向を計算
            Quaternion leftRayRotation = Quaternion.Euler(0, -m_viewAngle * 0.5f, 0);
            Quaternion rightRayRotation = Quaternion.Euler(0, m_viewAngle * 0.5f, 0);


            Vector3 leftRayDirection = leftRayRotation * forward;
            Vector3 rightRayDirection = rightRayRotation * forward;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * m_viewDistance);
            Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * m_viewDistance);


        }
    }
}




