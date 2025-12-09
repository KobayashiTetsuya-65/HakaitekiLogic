using UnityEngine;
using UnityEngine.UI;

public class Cursorfollower : MonoBehaviour
{
    [SerializeField]
    [Header("カーソルの位置")]
    private RectTransform arrow;

    public void FollowButton(Button button)
    {
        //カーソルを表示
        arrow.gameObject.SetActive(true);
        // ボタンの位置を取得
        RectTransform buttonRect = button.GetComponent<RectTransform>();

        // 矢印をボタンの右横に移動
        arrow.position = buttonRect.position + new Vector3(100f, 0f, 0f);
    }



}
