using UnityEngine;
using UnityEngine.Events;
public class KeyPadRock : MonoBehaviour
{
    [Header("正解のコード")]
    public int KeypadCode = 1234;
    //正解の場合
    private UnityEvent m_Success;
    //失敗の場合
    private UnityEvent m_Failure;
    //通常の色
    private Color m_UsuallyColor;
    //正解の色
    private Color m_SuccessColor;
    //失敗の色
    private Color m_FailureColor;
    //ボタンの音
    private AudioClip m_PushBottunSound;
    //正解の音
    private AudioClip m_SuccessSound;
    //不正解の音
    private AudioClip m_FailureSound;
    //エリアに入ったかどうか？
    private bool m_Aria = false;
    //Keypadの画像の大きさ
    private Vector3 m_originalScale;
    // 一度拡大したかどうか
    private bool m_Expanded = false;    

    void Start()
    {
        // 最初の大きさを保存しておく
        m_originalScale = transform.localScale;
    }
    private void Update()
    {
        // Eキーを押したら大きくする
        if (m_Aria&&Input.GetKeyDown(KeyCode.E)&&!m_Expanded)
        {
            Debug.Log("KEYPADを選択！");
            //拡大した
            m_Expanded = true;
            // 今のスケールを取得
            Vector3 nowScale = transform.localScale;

            // 大きさを2倍にする
            transform.localScale = nowScale * 2f;
        }
        else if (Input.GetKeyDown(KeyCode.Tab)&&m_Expanded)
        {
            Debug.Log("KEYPADから退出！");
            //縮小した
            m_Expanded = false;
            transform.localScale = m_originalScale;
        }

        }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Keypadの範囲に入りました！");
            //エリアに入った
            m_Aria = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Keypadの範囲から出ました！");
            //エリアに入った
            m_Aria = false;
        }
    }
}
