using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵方向UI表示機能
/// プレイヤーが指定キーを押している間だけ敵の方向を表示
/// </summary>
public class EnemyIndicator : MonoBehaviour
{
    [Header("矢印Prefab"), SerializeField]
    GameObject m_ArrowPrefab;

    [Header("中心基準点となる親オブジェクト"), SerializeField]
    RectTransform m_CenterScreen;

    [Header("プレイヤーのTransform"), SerializeField]
    Transform m_Player;

    [Header("UIを配置する半径"), SerializeField]
    float m_Radius = 350;

    [Header("インジケーター表示キー"), SerializeField]
    KeyCode m_ShowIndicatorKey = KeyCode.Tab;

    [Header("アビリティのクールタイム"), SerializeField]
    private float m_LookAbilityCoolingTime = 60f;

    [Header("アビリティの持ち時間"), SerializeField]
    private float m_LookAbilityNowTime = 10f;

    [Tooltip("敵とUIのペア辞書")]
    Dictionary<Transform, RectTransform> m_Indicators = new Dictionary<Transform, RectTransform>();

    //タイマーを設定
    [SerializeField] private float m_AbilityNowTime;
    [SerializeField] private float m_AbilityCoolingTime;

    /// <summary>
    /// インジケーターが表示中かどうか（アビリティ発動中か）
    /// </summary>
    bool m_IsActive = false;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //最初はクールタイムは完了状態に
        m_AbilityCoolingTime = m_LookAbilityCoolingTime;

        //初期値に設定
        m_AbilityNowTime = 0f;

        //非表示
        m_IsActive = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //表示なら発動時間へ
        if (m_IsActive)
        {
            NowTime();
        }
        else
        {
            //クールタイムへ
            CoolingTime();
            // 発動していないときは隠す
            HideAllIndicators(); 
        }

    }

    /// <summary>
    /// クールタイムの時間を計測
    /// </summary>
    void CoolingTime()
    {
        // クールタイムがまだ完了していないなら加算する
        if (m_AbilityCoolingTime < m_LookAbilityCoolingTime)
        {
            m_AbilityCoolingTime += Time.deltaTime;
        }
        else
        {
            // クールタイム完了済み。キー入力を待つ
            m_AbilityCoolingTime = m_LookAbilityCoolingTime;

            if (Input.GetKeyDown(m_ShowIndicatorKey))
            {
                Debug.Log("アビリティ発動！");
                m_IsActive = true;
                m_AbilityNowTime = 0f;
            }
        }
    }

    /// <summary>
    /// アビリティが発動中の時間
    /// </summary>
    void NowTime()
    {
        // 時間を加算
        m_AbilityNowTime += Time.deltaTime;

        // インジケーターを更新
        UpdateIndicators();

        // 制限時間を超えたら終了
        if (m_AbilityNowTime >= m_LookAbilityNowTime)
        {
            Debug.Log("アビリティ終了！クールダウンに入ります。");

            // 状態をリセット
            m_AbilityNowTime = 0f;
            m_IsActive = false;

            // クールタイムを0からスタートさせる
            m_AbilityCoolingTime = 0f;
        }
    }

    /// <summary>
    /// 全てのインジケーターを非表示にする
    /// </summary>
    void HideAllIndicators()
    {
        foreach (var pair in m_Indicators)
        {
            if (pair.Value != null)
            {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 敵がカメラ外にいる場合、方向矢印を計算
    /// </summary>
    void UpdateIndicators()
    {
        ManagePool();

        // カメラの前方向（0度）を基準として定義
        Vector3 camForward = Camera.main.transform.forward;
        // 水平方向のみ考慮
        camForward.y = 0f;

        foreach (var pair in m_Indicators)
        {
            // 敵の座標（角度計算に使用）
            Transform enemy = pair.Key;
            // 矢印UIの座標
            RectTransform arrow = pair.Value;

            // 敵が既に破棄されている場合はスキップ
            if (enemy == null || arrow == null) continue;

            // 3D空間の敵座標をスクリーン座標（ピクセル）に変換
            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position);

            // 敵が画面外にいるかどうか判定
            bool isOffScreen = screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

            // 画面外なら表示
            if (isOffScreen)
            {
                arrow.gameObject.SetActive(true);

                // カメラから敵への方向を計算
                Vector3 direction = enemy.position - m_Player.position;
                // 水平方向のみ
                direction.y = 0;

                // 相対角度を計算
                float angle = Vector3.SignedAngle(camForward, direction, Vector3.up);

                // 度数をラジアンに変換
                float rad = angle * Mathf.Deg2Rad;

                // 中心からの円周上の座標を計算
                arrow.anchoredPosition = new Vector2(Mathf.Sin(rad) * m_Radius, Mathf.Cos(rad) * m_Radius);

                // 矢印を敵の方向に回転
                arrow.localRotation = Quaternion.Euler(0, 0, -angle);
            }
            else
            {
                // 画面内の敵は非表示
                arrow.gameObject.SetActive(false);
            }
        }
    }

    void ManagePool()
    {
        // BattleManagerが存在しない場合は処理しない
        if (BattleManager.m_BattleInstance == null) return;

        // リストにあるがUIがない敵を追加
        foreach (var enemy in BattleManager.m_BattleInstance.m_ActiveEnemies)
        {
            // 敵が未登録なら追加
            if (!m_Indicators.ContainsKey(enemy))
            {
                // 生成
                GameObject newArrow = Instantiate(m_ArrowPrefab, m_CenterScreen);

                // 敵を登録
                m_Indicators.Add(enemy, newArrow.GetComponent<RectTransform>());
            }
        }

        // 削除対象を一時リストに保存
        List<Transform> toRemove = new List<Transform>();

        foreach (var key in m_Indicators.Keys)
        {
            // keyがnullか、辞書の敵がリストにいない（死亡、倒した等）
            if (key == null || !BattleManager.m_BattleInstance.m_ActiveEnemies.Contains(key))
            {
                // 削除リストに追加
                toRemove.Add(key);
            }
        }

        foreach (var key in toRemove)
        {
            // 削除リストにある敵のUIを破棄
            if (m_Indicators[key] != null)
            {
                Destroy(m_Indicators[key].gameObject);
            }

            // ペアリストから削除
            m_Indicators.Remove(key);
        }
    }
}
