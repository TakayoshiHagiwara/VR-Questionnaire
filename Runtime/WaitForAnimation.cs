// --------------------------------------------------
// Created:     2020/11/22
// Summary:  Custom coroutine that waits for animation transitions to finish
// Reference: http://tsubakit1.hateblo.jp/entry/2016/02/11/021743
// --------------------------------------------------

using UnityEngine;

public class WaitForAnimation : CustomYieldInstruction
{
    Animator animator;
    int lastStateHash = 0;
    int layerNo = 0;

    public WaitForAnimation(Animator animator, int layerNo)
    {
        Init(animator, layerNo, animator.GetCurrentAnimatorStateInfo(layerNo).shortNameHash);
    }

    void Init(Animator animator, int layerNo, int hash)
    {
        this.layerNo = layerNo;
        this.animator = animator;
        lastStateHash = hash;
    }

    public override bool keepWaiting
    {
        get
        {
            var currentAnimatorState = animator.GetCurrentAnimatorStateInfo(layerNo);
            return currentAnimatorState.fullPathHash == lastStateHash &&
                (currentAnimatorState.normalizedTime < 1);
        }
    }
}
