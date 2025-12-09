using UnityEngine;

public class Gameexit : MonoBehaviour
{
    
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("ゲーム終了");
        Application.Quit();
    }
}
