using UnityEngine;

[CreateAssetMenu(menuName ="クエストシステム/クエスト")]
public class QuestData :ScriptableObject
{
    [Header("クエスト名")]
    public string m_QuestName;
    [Header("説明文")]
    [TextArea]
    public string m_Description;
    [Header("達成条件（アイテム）")]
    public string m_Item;
    [Header("次のクエストデータ")]
    public QuestData m_NextQuest;
}
