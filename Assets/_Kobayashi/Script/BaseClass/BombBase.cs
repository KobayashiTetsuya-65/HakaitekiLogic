using UnityEngine;
/// <summary>
/// 爆弾の共通クラス
/// </summary>
public abstract class BombBase : MonoBehaviour
{
    [SerializeField, Tooltip("爆発範囲（半径）")] private float _radius = 5f;
    /// <summary>
    /// 爆発範囲表示
    /// </summary>
    public virtual void DisplayRange()
    {

    }
    /// <summary>
    /// 着火
    /// </summary>
    public virtual void Ignition()
    {

    }
    /// <summary>
    /// 爆発
    /// </summary>
    public abstract void Explosion();
}
