using UnityEngine;

public class IKBomb : MonoBehaviour
{
    public Transform HandAnchorR = null;
    public Transform HandAnchorL = null;

    private Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, HandAnchorR.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, HandAnchorR.rotation);

        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, HandAnchorL.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, HandAnchorL.rotation);
    }
}
