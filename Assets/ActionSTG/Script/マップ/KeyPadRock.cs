using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class KeyPadRock : MonoBehaviour
{
    [Header("正解のコード")]
    public string m_KeypadCode = "1234";
    [Header("ディスプレイに表示するテキスト")]
    public TextMeshProUGUI m_DisplayText;
    [Header("正解の効果音")]
    public AudioClip m_Success;
    [Header("失敗の効果音")]
    public AudioClip m_Failure;
    [Header("選択効果音")]
    public AudioClip m_Push;
    //エリアに入ったかどうか？
    private bool m_Aria = false;
    //Keypadの画像の大きさ
    private Vector3 m_originalScale;
    // 一度拡大したかどうか
    private bool m_Expanded = false;
    //入力の文字列
    private string m_Input = "";
    // 効果音を鳴らすための AudioSource
    private AudioSource m_AudioSource;


    void Start()
    {
        // 最初の大きさを保存しておく
        m_originalScale = transform.localScale;
        UpdateDisplay(); // 最初にリセット表示
                         // AudioSource を取得
        m_AudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        // Eキーを押したら大きくする
        if (m_Aria && Input.GetKeyDown(KeyCode.E) && !m_Expanded)
        {
            Debug.Log("KEYPADを選択！");
            //拡大した
            m_Expanded = true;
            // 今のスケールを取得
            Vector3 nowScale = transform.localScale;
            // 大きさを2倍にする
            transform.localScale = nowScale * 2f;
            //入力をリセット
            m_Input = "";
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && m_Expanded)
        {
            Debug.Log("KEYPADから退出！");
            //縮小した
            m_Expanded = false;
            transform.localScale = m_originalScale;
            UpdateDisplay();
        }
        //拡大中の時だけ入力を受け付ける
        if (m_Expanded)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (m_Input.Length < 4) 
                {
                    if (Input.GetKeyDown(i.ToString()))
                    {
                        m_AudioSource.PlayOneShot(m_Push);
                        m_Input += i.ToString();
                        UpdateDisplay();
                    }
                }
            }
            //Backspaceキーで削除
            if (Input.GetKeyDown(KeyCode.Backspace) && m_Input.Length > 0)
            {
                m_AudioSource.PlayOneShot(m_Push);
                m_Input = m_Input.Substring(0, m_Input.Length - 1);
                UpdateDisplay();
            }
            //エンターキーで判定
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (m_Input == m_KeypadCode)
                {
                    Debug.Log("正解！");
                    m_AudioSource.PlayOneShot(m_Success);
                }
                else
                {
                    Debug.Log("不正解");
                    m_AudioSource.PlayOneShot(m_Failure);

                }
                // 判定後にリセットするならここ
                m_Input = "";
                UpdateDisplay();
            }

        }

    }
    private void UpdateDisplay()
    {
        if (m_DisplayText != null)
        {
            // 入力中は入力値を表示(最初は____に設定)
            m_DisplayText.text = m_Input.PadRight(4, '_');
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
