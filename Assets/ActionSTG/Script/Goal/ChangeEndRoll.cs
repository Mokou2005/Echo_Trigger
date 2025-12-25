using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeEndRoll : MonoBehaviour
{
    /// <summary>
    /// アニメーションイベントでSecenを移動
    /// </summary>
    public void ChangeEndRollSecen()
    {
        Debug.Log(111);
        SceneManager.LoadScene("EndRoll");
    }
}
