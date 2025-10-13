using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("最初のクエストデータ")]
    public QuestData m_StartQuest;
    private QuestData m_questData;

     void Start()
    {
        StartQuest(m_StartQuest);
    }

    void StartQuest(QuestData quest)
    {
        //最初のクエストを入れる
        m_questData = quest;
        Debug.Log($"クエスト開始:{m_questData.m_QuestName}");
    }

    public void OnItemObtained(string itemName)
    {
        //現在のクエストがないならなにもしない
        if (m_questData == null) return;
        //現在のクエストのアイテムと一致したら
        if (itemName == m_questData.m_Item)
        {
            Debug.Log($"クエスト達成: {m_questData.m_NextQuest}");
            //次のクエストが設定されてたら次のクエストへ
            if (m_questData.m_NextQuest != null)
            {
                StartQuest(m_questData.m_NextQuest);
            }
            else
            {
                Debug.Log("全クエスト完了！");
            }
        }
    }
}
