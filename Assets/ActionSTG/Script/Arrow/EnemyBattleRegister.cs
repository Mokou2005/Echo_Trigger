using UnityEngine;

/// <summary>
/// 敵がプレイヤーを発見した時にBattleManagerに登録するスクリプト
/// AlertLevelの攻撃モードと連携して自動登録/解除を行う
/// </summary>
public class EnemyBattleRegister : MonoBehaviour
{
    [Header("AlertLevel参照（自動取得可）")]
    [SerializeField] AlertLevel m_AlertLevel;

    /// <summary>
    /// 前フレームの攻撃モード状態
    /// </summary>
    bool m_WasAttackMode = false;

    /// <summary>
    /// BattleManagerに登録されているか
    /// </summary>
    bool m_IsRegistered = false;

    void Update()
    {
        // AlertLevelが未設定なら自動取得
        if (m_AlertLevel == null)
        {
            m_AlertLevel = GetComponent<AlertLevel>();
        }

        if (m_AlertLevel == null) return;

        // 攻撃モードに入った瞬間を検知
        if (m_AlertLevel.m_AttackMode && !m_WasAttackMode)
        {
            RegisterToBattle();
        }
        // 攻撃モードから出た瞬間を検知
        else if (!m_AlertLevel.m_AttackMode && m_WasAttackMode)
        {
            UnregisterFromBattle();
        }

        m_WasAttackMode = m_AlertLevel.m_AttackMode;
    }

    /// <summary>
    /// BattleManagerに敵を登録
    /// </summary>
    public void RegisterToBattle()
    {
        if (BattleManager.m_BattleInstance != null && !m_IsRegistered)
        {
            BattleManager.m_BattleInstance.EnemyFoundPlayer(transform);
            m_IsRegistered = true;
            Debug.Log($"{gameObject.name} がBattleManagerに登録されました");
        }
    }

    /// <summary>
    /// BattleManagerから敵を解除
    /// </summary>
    public void UnregisterFromBattle()
    {
        if (BattleManager.m_BattleInstance != null && m_IsRegistered)
        {
            BattleManager.m_BattleInstance.EnemyLostPlayer(transform);
            m_IsRegistered = false;
            Debug.Log($"{gameObject.name} がBattleManagerから解除されました");
        }
    }

    /// <summary>
    /// 敵が死亡した時に呼び出す
    /// </summary>
    public void OnEnemyDeath()
    {
        if (BattleManager.m_BattleInstance != null && m_IsRegistered)
        {
            BattleManager.m_BattleInstance.EnemyDeath(transform);
            m_IsRegistered = false;
            Debug.Log($"{gameObject.name} が死亡によりBattleManagerから削除されました");
        }
    }

    void OnDestroy()
    {
        // オブジェクト破棄時に自動解除
        UnregisterFromBattle();
    }
}
