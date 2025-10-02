
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Memo : MonoBehaviour
{
    [Header("Playerアニメーター")]
    public Animator m_Animator;
    [Header("拾う画像")]
    public GameObject m_PickUpImage;
    [Header("紙の画像")]
    public RawImage m_MemoImage;
    [Header("TextUI")]
    public TextMeshProUGUI m_Text;
    //エリアに入ったかどうか
    private bool m_Aria=false;
    //Eキーを押したかどうか
    private bool m_E_KeyPush=false;

    private void Start()
    {
        //非表示
        m_PickUpImage.SetActive(false);
        m_MemoImage.gameObject.SetActive(false);
        //m_Text.enabled=false;
    }
    private void Update()
    {
        if(m_Aria&&!m_E_KeyPush&& Input.GetKeyDown(KeyCode.E))
        {
            m_E_KeyPush=true;
            //画像を表示
            m_MemoImage.gameObject.SetActive(true);
            // m_Text.enabled = true;
            if (m_E_KeyPush&&Input.GetKeyDown(KeyCode.Tab))
            {
                //画像を非表示
                m_MemoImage.gameObject.SetActive(false);
                // m_Text.enabled = false;
            }
        }
     
    }

    private void OnTriggerEnter(Collider other)
    {
        //playerが入ったら
        if (other.CompareTag("Player"))
        {
            m_Aria = true;
            //表示
            m_PickUpImage.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //playerがでたら
        if (other.CompareTag("Player"))
        {
            m_Aria=false;
            //非表示
            m_PickUpImage.SetActive(false);
        }
    }
}
