// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Manager for state transition animation.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private List<Animator> animator = new List<Animator>();
    private bool isAnimating = false;

    private Animator anim;

    /// <summary>
    /// Initialize manager
    /// </summary>
    /// <param name="questionnaireList">GameObject list of questionnaires</param>
    public void Init(List<GameObject> questionnaireList)
    {
        foreach (GameObject q in questionnaireList)
        {
            q.TryGetComponent(out anim);
            animator.Add(anim);
        }
    }

    /// <summary>
    /// Display the first questionnaires. First of the randamized questionnaires.
    /// </summary>
    /// <param name="num">Questionnaire number, corresponding to the list number of "questionnaires"</param>
    public void SetFirstQuestionnaire(int num)
    {
        StartCoroutine(ChangeAnimation(num));
    }

    /// <summary>
    /// Animated questionnaire transitions
    /// </summary>
    /// <param name="currentNum">Current questionnaire order</param>
    /// <param name="nextNum">Next questionnaire order</param>
    public void ChangeAnimationState(int currentNum, int nextNum)
    {  
        if(!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(ChangeAnimation(currentNum));
            StartCoroutine(ChangeAnimation(nextNum));
        }
    }

    /// <summary>
    /// Coroutine for animation transition
    /// </summary>
    /// <param name="targetNum">Target animator number</param>
    /// <returns></returns>
    public IEnumerator ChangeAnimation(int targetNum)
    {
        animator[targetNum].SetBool("isChange", true);
        yield return null;
        yield return new WaitForAnimation(animator[targetNum], 0);

        animator[targetNum].SetBool("isChange", false);

        // yield return new WaitForSeconds(0.1f);
        isAnimating = false;
    }
}
