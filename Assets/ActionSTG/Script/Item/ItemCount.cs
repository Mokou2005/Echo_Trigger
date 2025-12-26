using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ItemCount : MonoBehaviour
{
    [Header("もっているアイテムの数")]
    public float m_ItemCount=3f;
    [Header("アイテムの数のText")]
    [SerializeField] private Text m_ItemText;
    [Header("数がない時の警告表示")]
    [SerializeField] private Image m_WarningImage;
    //アイテムを使ったかどうか
    public bool m_UseItem=false;
    //実行中かどうか
    public bool m_IsWarningRunning = false;
    private void Start()
    {
        //非表示
        m_WarningImage.enabled = false;
    }
    //アイテムの消費関数
    public void UseStoneItem()
    {
        //アイテムを一個以上持っているか
        if (m_ItemCount >=0)
        {
            //アイテムを一つ減らす
            if (m_UseItem)
            {
                m_ItemCount -= 1;
                m_UseItem = false;
                Debug.Log("残りアイテム数:" + m_ItemCount);
                UpdateItemText();
            }
        }
        else
        {
            Debug.Log("アイテムが０個です！");
        }

    }
    //Textに残り数を変更
    void UpdateItemText()
    {
        m_ItemText.text="×"+((int)m_ItemCount).ToString();
    }
    public IEnumerator ItemCoroutine()
    {
        // すでに実行中なら何もしない
        if (m_IsWarningRunning) yield break;
        m_WarningImage.enabled = true;
        m_IsWarningRunning = true;
        //５秒間表示して消す
        Debug.Log("警告表示");
        m_WarningImage.enabled = true;      
        yield return new WaitForSeconds(5f);
        m_WarningImage.enabled = false;
        m_IsWarningRunning = false;
    }
}
